module FSharpHomeworks.Tests.Primary

open Expecto
open Introduction

/// <summary>
/// Check test result.
/// </summary>
/// <param name="actualSeq">Actual seq.</param>
/// <param name="n">Number of items to be checked.</param>
let checkResult actualSeq (n: uint) =
    let isFirstNNumbersIsPrime =
        Seq.take (int n) actualSeq |> Seq.forall Numbers.isPrime

    "All numbers must be prime" |> Expect.isTrue isFirstNNumbersIsPrime

/// <summary>
/// Test for <see cref="Introduction.Numbers.primary"/>
/// </summary>
let seqTest = testProperty "Primary numbers test" <| checkResult Numbers.primary

let createTest expected count =
    test (count.ToString()) {
        let actual = Seq.take count Numbers.primary

        "Result must be the same" |> Expect.sequenceEqual actual expected
    }

let moduleTests =
    [ createTest [ 2 ] 1
      createTest [ 2; 3 ] 2
      createTest [ 2; 3; 5; 7; 11 ] 5
      createTest [ 2; 3; 5; 7; 11; 13; 17; 19; 23 ] 9 ]
    |> testList "module"

let allTests = testList "Numbers.primary" [ seqTest; moduleTests ]
