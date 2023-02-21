module FSharpHomeworks.Tests.List

open Expecto

let checkResult list result =
    "List must be the same" |> Expect.sequenceEqual result (List.rev list)

let makeTest<'a when 'a: equality> (reversFun: 'a list -> 'a list) (list: 'a list) =
    reversFun list |> checkResult list

let testFixture =
    let config = Utils.defaultConfig

    [ makeTest<int> Introduction.List.revers
      |> testPropertyWithConfig config "int test"

      makeTest<bool> Introduction.List.revers
      |> testPropertyWithConfig config "bool test"

      makeTest<byte> Introduction.List.revers
      |> testPropertyWithConfig config "byte test" ]

let tests = testList "List.revers tests" testFixture
