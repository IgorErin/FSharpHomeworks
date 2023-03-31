open Expecto
open FSharpHomeworks.Tests

[<Tests>]
let tests = testList "All tests" [ PointFree.test ]

[<EntryPoint>]
let main argv =
    runTestsWithCLIArgs [] argv tests
