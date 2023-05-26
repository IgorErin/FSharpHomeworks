open Expecto
open FSharpHomeworks.Tests

[<Tests>]
let tests = testList "All tests" [ Crawler.test ]

[<EntryPoint>]
let main argv =
    runTestsWithCLIArgs [] argv tests
