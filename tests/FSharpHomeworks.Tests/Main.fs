open Expecto
open FSharpHomeworks.Tests

[<Tests>]
let tests = testList "All tests" [ EvenNumbersCount.tests ]

[<EntryPoint>]
let main argv =
    runTestsWithCLIArgs [] argv tests
