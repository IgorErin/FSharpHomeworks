open Expecto
open FSharpHomeworks.Tests

[<Tests>]
let tests = testList "All tests" [ InfectNet.allTests ]

[<EntryPoint>]
let main argv =
    runTestsWithCLIArgs [] argv tests
