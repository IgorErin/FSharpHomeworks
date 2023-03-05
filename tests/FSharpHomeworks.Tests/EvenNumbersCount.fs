module FSharpHomeworks.Tests.EvenNumbersCount

open Expecto
open Introduction

let config = Utils.defaultConfig

let makeTest firstFun secondFun (list: int list) =
    let firstResult = firstFun list
    let secondResult = secondFun list

    $"Results must be the same" |> Expect.equal firstResult secondResult

let testFixtures functionPair name =
    makeTest (fst functionPair) (snd functionPair)
    |> testPropertyWithConfig config name

let tests =
    [ "mapFold and FilterFold"
      |> testFixtures (EvenNumbers.countMapFold, EvenNumbers.countFilterFold)

      "mapFold and FilterMapFold"
      |> testFixtures (EvenNumbers.countMapFold, EvenNumbers.countFilterMapFold)

      "filterFold and filterMapFold"
      |> testFixtures (EvenNumbers.countFilterFold, EvenNumbers.countFilterMapFold) ]
    |> testList "All equal EvenNumbers tests"
