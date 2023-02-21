open Expecto
open FSharpHomeworks.Tests

[<Tests>]
let tests =
    testList "All tests" [ Fibonacci.tests ]

[<EntryPoint>]
let main argv =
    runTestsWithCLIArgs [] argv tests
