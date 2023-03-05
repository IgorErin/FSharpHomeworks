open Expecto

open FSharpHomeworks.Tests

[<Tests>]
let tests = testList "All tests" [ Primary.test ]

[<EntryPoint>]
let main argv =
    runTestsWithCLIArgs [] argv tests
