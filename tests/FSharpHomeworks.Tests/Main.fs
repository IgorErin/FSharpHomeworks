open Expecto
open FSharpHomeworks.Tests

let tests =
    testList "All tests"
        [ List.tests ]

[<EntryPoint>]
let main argv = runTestsWithCLIArgs [] argv tests
