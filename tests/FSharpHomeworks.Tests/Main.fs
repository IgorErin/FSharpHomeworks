open Expecto
open FSharpHomeworks.Tests

/// <summary>
/// All tests.
/// </summary>
[<Tests>]
let tests = testList "All tests" [ List.Find.tests ]

[<EntryPoint>]
let main argv =
    runTestsWithCLIArgs [] argv tests
