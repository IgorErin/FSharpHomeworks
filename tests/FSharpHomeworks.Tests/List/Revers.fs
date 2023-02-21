module FSharpHomeworks.Tests.List

open Expecto

let checkResult list result =
    "List must be the same"
    |> Expect.equal result (List.rev list)

let makeTest<'a when 'a: equality>
    (reversFun: 'a list -> 'a list) (list: 'a list) =
    reversFun list
    |> checkResult list 

let testFixture =
    [ makeTest<int> Introduction.List.revers
      |> testProperty "int test"
      
      makeTest<float> Introduction.List.revers
      |> testProperty "float test"
      
      makeTest<bool> Introduction.List.revers
      |> testProperty "bool test"
      
      makeTest<byte> Introduction.List.revers
      |> testProperty "byte test" ]
    
let tests = testList "List.revers tests" testFixture
