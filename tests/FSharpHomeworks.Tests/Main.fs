open Expecto
open FSharpHomeworks.Tests

[<Tests>]
let tests = testList "All tests" [ Factorial.tests ]

[<EntryPoint>]
let main argv =
    runTestsWithCLIArgs [] argv tests
