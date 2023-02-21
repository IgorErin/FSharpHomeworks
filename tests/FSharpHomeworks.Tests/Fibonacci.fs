module FSharpHomeworks.Tests.Fibonacci

open Expecto
open Introduction 

let rec checkResultRec = function
    | fst :: snd :: thd :: tail ->
        "Fibonacci properties must be met"
        |> Expect.equal (fst + snd) thd
        
        checkResultRec (snd :: thd :: tail)        
    | _ -> ()
    
let checkResult count list =
    let count = max count 0
    
    "Lengths must be the same"
    |> Expect.equal count (List.length list)
    
    match list with 
    | [] -> ()
    | [ item ] ->
        "The first element must be zero"
        |> Expect.equal item 0 
    | [fst; snd] ->
        "The first element must be zero"
        |> Expect.equal fst 0
        
        "the second element must be a 1"
        |> Expect.equal snd 1
    | _ -> checkResultRec list
        
let makeTest fibonacciFun count =
    fibonacciFun count
    |> Seq.toList
    |> checkResult count 

let tests = testProperty "Fibonacci test" <| makeTest Fibonacci.run
