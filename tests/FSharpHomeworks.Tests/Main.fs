open Expecto
open FSharpHomeworks.Tests

[<Tests>]
let tests = testList "All tests" [ Round.tests; Calculate.allTests ]

[<EntryPoint>]
let main argv =
    runTestsWithCLIArgs [] argv tests
