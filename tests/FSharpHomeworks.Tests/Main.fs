open Expecto
open FSharpHomeworks.Tests

[<Tests>]
let tests = testList "All tests" [ Interpreter.tests ]

[<EntryPoint>]
let main argv =
    runTestsWithCLIArgs [] argv tests
