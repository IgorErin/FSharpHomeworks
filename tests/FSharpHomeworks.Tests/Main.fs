open Expecto

[<Tests>]
let tests = testList "All tests" []

[<EntryPoint>]
let main argv =
    runTestsWithCLIArgs [] argv tests
