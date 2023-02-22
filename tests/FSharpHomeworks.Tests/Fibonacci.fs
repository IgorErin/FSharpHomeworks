module FSharpHomeworks.Tests.Fibonacci

open Expecto
open Introduction

/// <summary>
/// Check fibonacci sequence in form of list.
/// </summary>
let rec checkResultRec =
    function
    | fst :: snd :: thd :: tail ->
        "Fibonacci properties must be met" |> Expect.equal (fst + snd) thd

        checkResultRec (snd :: thd :: tail)
    | _ -> ()

// TODO(type reference)
/// <summary>
/// Check <see cref="Introduction.Fibonacci.run"/> result.
/// </summary>
/// <param name="count">Lenght of fibonacci sequence.</param>
/// <param name="list">Fibonacci sequence in form of list.</param>
let checkResult count list =
    let count = max count 0

    "Lengths must be the same" |> Expect.equal count (List.length list)

    match list with
    | [] -> ()
    | [ item ] -> "The first element must be zero" |> Expect.equal item 0
    | [ fst; snd ] ->
        "The first element must be zero" |> Expect.equal fst 0

        "the second element must be a 1" |> Expect.equal snd 1
    | _ -> checkResultRec list

/// <summary>
/// Make fibonacci function test.
/// </summary>
/// <param name="fibonacciFun">Fibonacci function.</param>
/// <param name="count">Fibonacci lenght.</param>
let makeTest fibonacciFun count =
    fibonacciFun count |> Seq.toList |> checkResult count

/// <summary>
/// Fibonacci tests.
/// </summary>
let tests = testProperty "Fibonacci test" <| makeTest Fibonacci.run
