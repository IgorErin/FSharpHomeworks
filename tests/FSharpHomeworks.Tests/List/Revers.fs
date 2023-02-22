module FSharpHomeworks.Tests.List

open Expecto

/// <summary>
/// Check revert function result.
/// </summary>
/// <param name="list"></param>
/// <param name="result"></param>
let checkResult list result =
    "List must be the same" |> Expect.sequenceEqual result (List.rev list)

/// <summary>
/// Make test for function that revers list.
/// </summary>
/// <param name="reversFun">Revers function.</param>
/// <param name="list">List to revers.</param>
let makeTest<'a when 'a: equality> (reversFun: 'a list -> 'a list) (list: 'a list) =
    reversFun list |> checkResult list

/// <summary>
/// Test fixtures for different data types.
/// </summary>
let testFixture =
    let config = Utils.defaultConfig

    [ makeTest<int> Introduction.List.revers
      |> testPropertyWithConfig config "int test"

      makeTest<bool> Introduction.List.revers
      |> testPropertyWithConfig config "bool test"

      makeTest<byte> Introduction.List.revers
      |> testPropertyWithConfig config "byte test" ]

/// <summary>
/// All <see cref="List.revers"/> tests.
/// </summary>
let tests = testList "List.revers tests" testFixture
