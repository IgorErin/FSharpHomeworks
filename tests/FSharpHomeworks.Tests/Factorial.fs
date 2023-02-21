module FSharpHomeworks.Tests.Factorial

open Introduction
open Expecto

let factorial count =
    let rec recFactorial count acc =
        if count < 1 then
            acc
        else
            recFactorial (count - 1) (acc * count)

    recFactorial count 1

let checkResult count result =
    "Results must be the same" |> Expect.equal (factorial count) result

let makeTest factorialFunction count =
    factorialFunction count |> checkResult count

let tests =
    [ makeTest Factorial.run |> testProperty "factorial test" ]
    |> testList "Introduction.Factorial tests"
