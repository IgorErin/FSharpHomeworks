open Expecto
open FSharpHomeworks.Tests

[<Tests>]
let tests = testList "All tests" [ Parenthesis.unitTests ]

[<EntryPoint>]
let main argv =
    runTestsWithCLIArgs [] argv tests
