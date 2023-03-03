module FSharpHomeworks.Tests.TreeMap

open Introduction
open Expecto

let config = Utils.defaultConfig

let rec checkResult map sourceTree actualTree =
    match sourceTree, actualTree with
    | Node(leftSource, rightSource, valueSource),
      Node(leftActual, rightActual, actualValue) ->
        let expectedValue = map valueSource

        "Values must be the same" |> Expect.equal expectedValue actualValue

        checkResult map leftSource leftActual
        checkResult map rightSource rightActual
    | Nil, Nil -> ()
    | _, _ -> failwith "Different pattern"

let makeTest map tree =
    Tree.map map tree |> checkResult map tree

let testFixtures =
    let intMap = fun item -> item + 1

    [ makeTest intMap |> testPropertyWithConfig config "Int increment test" ]

let tests = testList "Tree map" testFixtures
