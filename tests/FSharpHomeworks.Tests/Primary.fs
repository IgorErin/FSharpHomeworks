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
let test = testProperty "Primary numbers test" <| checkResult Numbers.primary
