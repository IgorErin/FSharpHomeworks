module FSharpHomeworks.Tests.SeqOf2

open Expecto
open Introduction

/// <summary>
/// Get specified power of 2.
/// </summary>
/// <param name="count">
/// Power degrees.
/// </param>
let getPowOf2 count =
    let rec getPowOf2 count acc =
        if count > 0 then getPowOf2 (count - 1) <| acc * 2 else acc

    getPowOf2 count 1

/// <summary>
/// Check result recursively.
/// </summary>
/// <param name="expectedItem">
/// Expected item on first step --- first sequence item.
/// </param>
let rec checkResultRec expectedItem =
    function
    | head :: tail ->
        "Items should be equal " |> Expect.equal expectedItem head

        checkResultRec (expectedItem * 2) tail
    | [] -> ()

/// <summary>
/// Check test result.
/// </summary>
/// <param name="n">First item power of 2</param>
/// <param name="m">Length of sequence.</param>
/// <param name="result">Actual test result.</param>
let checkResult n m result =
    match result with
    | Right result -> Seq.toList result |> checkResultRec (getPowOf2 n)
    | Left message ->
        "The condition of incorrect input must be met" |> Expect.isTrue (n < 0 || m < 0)

        "Incorrect input should return an error message"
        |> Expect.equal message "Invalid input data: n or m less than 0"

/// <summary>
/// Make test with specified function.
/// </summary>
let makeTest n m =
    Degrees.seqOf2 n m |> checkResult n m

/// <summary>
/// All tests for <see cref="Degrees.seqOf2"/>
/// </summary>
let tests = makeTest |> testProperty "Test of the sequence of powers of two"
