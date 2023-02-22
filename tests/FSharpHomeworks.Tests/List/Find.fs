module FSharpHomeworks.Tests.List.Find

open Expecto
open FSharpHomeworks.Tests

let checkResult item list actual =
    let expected =
        List.tryFindIndex (fun listItem -> listItem = item) list
    
    "The positions found must be the same"
    |> Expect.equal actual expected

let makeTest<'a when 'a : equality> findFunction (item: 'a) list =
    findFunction item list
    |> checkResult item list
    
let testFixtures =
    let config = Utils.defaultConfig
    
    [ makeTest<int> Introduction.List.tryFindIndex
      |> testPropertyWithConfig config "int test"
      
      makeTest<byte> Introduction.List.tryFindIndex
      |> testPropertyWithConfig config "byte test"
      
      makeTest<bool> Introduction.List.tryFindIndex
      |> testPropertyWithConfig config "bool test" ]

let tests = testList "List.tryFindIndex" testFixtures
