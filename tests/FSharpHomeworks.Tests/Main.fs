open Expecto


[<Tests>]
let tests =
    testList
        "All tests"
        [ FSharpHomeworks.Tests.SuperMap.tests; FSharpHomeworks.Tests.Romb.allTests ]

[<EntryPoint>]
let main argv =
    runTestsWithCLIArgs [] argv tests
