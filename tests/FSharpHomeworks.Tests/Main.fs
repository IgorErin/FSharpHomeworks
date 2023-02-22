open Expecto

open FSharpHomeworks.Tests

[<Tests>]
let tests = testList "All tests" [ SeqOf2.tests ]

[<EntryPoint>]
let main argv =
    runTestsWithCLIArgs [] argv tests
