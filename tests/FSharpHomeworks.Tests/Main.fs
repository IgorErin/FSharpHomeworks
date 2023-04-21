open Expecto


[<Tests>]
let tests = testList "All tests" [ FSharpHomeworks.Tests.SuperMap.tests]

[<EntryPoint>]
let main argv =
    runTestsWithCLIArgs [] argv tests
