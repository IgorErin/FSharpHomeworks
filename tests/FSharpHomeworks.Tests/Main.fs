open Expecto
open FSharpHomeworks.Tests

[<Tests>]
let tests = testList "All tests" [ Lazy.allTests ]

[<EntryPoint>]
let main argv =
    runTestsWithCLIArgs [] argv tests
