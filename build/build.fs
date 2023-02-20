open System
open Fake.Core
open Fake.DotNet
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.IO.Globbing.Operators
open Fake.Core.TargetOperators
open Fake.BuildServer

let environVarAsBoolOrDefault varName defaultValue =
    let truthyConsts = [
        "1"
        "Y"
        "YES"
        "T"
        "TRUE"
    ]
    try
        let envvar = (Environment.environVar varName).ToUpper()
        truthyConsts |> List.exists((=)envvar)
    with
    | _ ->  defaultValue

//-----------------------------------------------------------------------------
// Metadata and Configuration
//-----------------------------------------------------------------------------

let productName = "FSharpHomeworks"
let sln = __SOURCE_DIRECTORY__ </> ".." </> "FSharpHomeworks.sln"

let src = __SOURCE_DIRECTORY__ </> ".." </> "src"

let srcCodeGlob =
    !! ( src  @@ "**/*.fs")
    ++ ( src  @@ "**/*.fsx")
    -- ( src  @@ "**/obj/**/*.fs")

let testsCodeGlob =
    !! (__SOURCE_DIRECTORY__ </> ".." </> "tests/**/*.fs")
    ++ (__SOURCE_DIRECTORY__ </> ".." </> "tests/**/*.fsx")
    -- (__SOURCE_DIRECTORY__ </> ".." </> "tests/**/obj/**/*.fs")

let srcGlob = src @@ "**/*.??proj"
let testsGlob = __SOURCE_DIRECTORY__ </> ".." </> "tests/**/*.??proj"

let mainApp = src @@ productName

let srcAndTest =
    !! srcGlob
    ++ testsGlob

let gitOwner = "IgorErin"
let gitRepoName = "FSharpHomeworks"

let gitHubRepoUrl = sprintf "https://github.com/%s/%s" gitOwner gitRepoName

let tagFromVersionNumber versionNumber = sprintf "v%s" versionNumber
let targetFramework =  "net7.0"

// RuntimeIdentifiers: https://docs.microsoft.com/en-us/dotnet/core/rid-catalog
// dotnet-packaging Tasks: https://github.com/qmfrederik/dotnet-packaging/blob/0c8e063ada5ba0de2b194cd3fad8308671b48092/Packaging.Targets/build/Packaging.Targets.targets
let runtimes = [
    "linux-x64", "CreateTarball"
    "osx-x64", "CreateTarball"
    "win-x64", "CreateZip"
]

let githubToken = Environment.environVarOrNone "GITHUB_TOKEN"

//-----------------------------------------------------------------------------
// Helpers
//-----------------------------------------------------------------------------
let invokeAsync f = async { f () }

let configuration (_ : Target list) =
    match Environment.environVarOrDefault "CONFIGURATION" "Debug" with
    | "Debug" -> DotNet.BuildConfiguration.Debug
    | "Release" -> DotNet.BuildConfiguration.Release
    | config -> DotNet.BuildConfiguration.Custom config

let failOnBadExitAndPrint (p : ProcessResult) =
    if p.ExitCode <> 0 then
        p.Errors |> Seq.iter Trace.traceError
        failwithf "failed with exitcode %d" p.ExitCode

let rec retryIfInCI times fn =
    match Environment.environVarOrNone "CI" with
    | Some _ ->
        if times > 1 then
            try
                fn()
            with
            | _ -> retryIfInCI (times - 1) fn
        else
            fn()
    | _ -> fn()

module dotnet =
    let watch cmdParam program args =
        DotNet.exec cmdParam (sprintf "watch %s" program) args

    let tool optionConfig command args =
        DotNet.exec optionConfig (sprintf "%s" command) args
        |> failOnBadExitAndPrint

    let reportgenerator optionConfig args =
        tool optionConfig "reportgenerator" args

    let fsharpAnalyzer optionConfig args =
        tool optionConfig "fsharp-analyzers" args

    let fantomas args =
        DotNet.exec id "fantomas" args

//-----------------------------------------------------------------------------
// Target Implementations
//-----------------------------------------------------------------------------

let clean _ =
    ["bin"; "temp" ;]
    |> Shell.cleanDirs

    !! srcGlob
    ++ testsGlob
    |> Seq.collect(fun p ->
        ["bin";"obj"]
        |> Seq.map(fun sp ->
            IO.Path.GetDirectoryName p @@ sp)
        )
    |> Shell.cleanDirs

    [
        "paket-files/paket.restore.cached"
    ]
    |> Seq.iter Shell.rm

let dotnetRestore _ =
    [sln]
    |> Seq.map(fun dir -> fun () ->
        let args =
            [
            ]
        DotNet.restore(fun c ->
            { c with
                Common =
                    c.Common
                    |> DotNet.Options.withAdditionalArgs args
            }) dir)
    |> Seq.iter(retryIfInCI 10)

let dotnetBuild ctx =
    DotNet.build(fun c ->
        { c with
            Configuration = configuration (ctx.Context.AllExecutingTargets)
            Common =
                c.Common
                |> DotNet.Options.withAdditionalArgs [ "--no-restore" ]
        }) sln

let dotnetTest ctx =
    DotNet.test(fun c ->
        { c with
            Configuration = configuration (ctx.Context.AllExecutingTargets)
            Common =
                c.Common
                |> DotNet.Options.withAdditionalArgs [ "--no-build" ]
            }) sln

let formatCode _ =
    let result =
        [
            srcCodeGlob
            testsCodeGlob
        ]
        |> Seq.collect id
        // Ignore AssemblyInfo
        |> Seq.filter(fun f -> f.EndsWith("AssemblyInfo.fs") |> not)
        |> String.concat " "
        |> dotnet.fantomas

    if not result.OK then
        printfn "Errors while formatting all files: %A" result.Messages

let checkFormatCode _ =
    let result =
        [
            srcCodeGlob
            testsCodeGlob
        ]
        |> Seq.collect id
        // Ignore AssemblyInfo
        |> Seq.filter(fun f -> f.EndsWith("AssemblyInfo.fs") |> not)
        |> String.concat " "
        |> sprintf "%s --check"
        |> dotnet.fantomas

    if result.ExitCode = 0 then
        Trace.log "No files need formatting"
    elif result.ExitCode = 99 then
        failwith "Some files need formatting, check output for more info"
    else
        Trace.logf "Errors while formatting: %A" result.Errors

let initTargets () =
    BuildServer.install [
        GitHubActions.Installer
    ]
    /// Defines a dependency - y is dependent on x
    let (==>!) x y = x ==> y |> ignore
    /// Defines a soft dependency. x must run before y, if it is present, but y does not require x to be run.
    let (?=>!) x y = x ?=> y |> ignore
//-----------------------------------------------------------------------------
// Hide Secrets in Logger
//-----------------------------------------------------------------------------
    Option.iter(TraceSecrets.register "<GITHUB_TOKEN>" ) githubToken

    //-----------------------------------------------------------------------------
    // Target Declaration
    //-----------------------------------------------------------------------------

    Target.create "Clean" clean
    Target.create "DotnetRestore" dotnetRestore
    Target.create "DotnetBuild" dotnetBuild
    Target.create "DotnetTest" dotnetTest
    Target.create "FormatCode" formatCode
    Target.create "CheckFormatCode" checkFormatCode

    //-----------------------------------------------------------------------------
    // Target Dependencies
    //-----------------------------------------------------------------------------

    "Clean"
        ==> "DotnetRestore"
        ==> "CheckFormatCode"
        ==> "DotnetBuild"
        ==>! "DotnetTest"

//-----------------------------------------------------------------------------
// Target Start
//-----------------------------------------------------------------------------

[<EntryPoint>]
let main argv =
    argv
    |> Array.toList
    |> Context.FakeExecutionContext.Create false "build.fsx"
    |> Context.RuntimeContext.Fake
    |> Context.setExecutionContext
    initTargets ()
    Target.runOrDefaultWithArguments "DotnetTest"

    0


