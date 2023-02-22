module FSharpHomeworks.Tests.List.Find

open Expecto
open FSharpHomeworks.Tests

/// <summary>
/// Check test result.
/// </summary>
/// <param name="item">Item to find.</param>
/// <param name="list">The list where the search will be conducted.</param>
/// <param name="actual">Actual value.</param>
let checkResult item list actual =
    let expected = List.tryFindIndex (fun listItem -> listItem = item) list

    "The positions found must be the same" |> Expect.equal actual expected

/// <summary>
/// Make test.
/// </summary>
let makeTest<'a when 'a: equality> findFunction (item: 'a) list =
    findFunction item list |> checkResult item list

/// <summary>
/// Test fixtures with different types.
/// </summary>
let testFixtures =
    let config = Utils.defaultConfig

    [ makeTest<int> Introduction.List.tryFindIndex
      |> testPropertyWithConfig config "int test"

      makeTest<byte> Introduction.List.tryFindIndex
      |> testPropertyWithConfig config "byte test"

      makeTest<bool> Introduction.List.tryFindIndex
      |> testPropertyWithConfig config "bool test" ]

/// <summary>
/// <see cref=" Introduction.List.tryFindIndex"/> tests.
/// </summary>
let tests = testList "List.tryFindIndex" testFixtures
