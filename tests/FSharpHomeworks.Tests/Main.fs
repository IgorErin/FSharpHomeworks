open Expecto

open FSharpHomeworks.Tests

[<Tests>]
let tests = testList "All tests" [ ArithmeticTree.test ]

[<EntryPoint>]
let main argv =
    runTestsWithCLIArgs [] argv tests
