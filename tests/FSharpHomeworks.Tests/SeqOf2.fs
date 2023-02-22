module FSharpHomeworks.Tests.SeqOf2

open Expecto
open Introduction

let getPowOf2 count =
    let rec getPowOf2 count acc =
        if count > 0 then getPowOf2 (count - 1) <| acc * 2 else acc

    getPowOf2 count 1

let rec checkResultRec expectedItem =
    function
    | head :: tail ->
        "Items should be equal " |> Expect.equal expectedItem head

        checkResultRec (expectedItem * 2) tail
    | [] -> ()

let checkResult n m result =
    match result with
    | Right result -> Seq.toList result |> checkResultRec (getPowOf2 n)
    | Left message ->
        "The condition of incorrect input must be met" |> Expect.isTrue (n < 0 || m < 0)

        "Incorrect input should return an error message"
        |> Expect.equal message "Invalid input data: n or m less than 0"

let makeTest n m =
    Degrees.seqOf2 n m |> checkResult n m

let tests = makeTest |> testProperty "Test of the sequence of powers of two"
