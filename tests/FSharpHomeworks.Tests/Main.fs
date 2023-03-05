open Expecto

open FSharpHomeworks.Tests

[<Tests>]
let tests = testList "All tests" [ TreeMap.tests ]

[<EntryPoint>]
let main argv =
    runTestsWithCLIArgs [] argv tests
