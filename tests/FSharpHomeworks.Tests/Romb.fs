module FSharpHomeworks.Tests.Romb

open Expecto
open Introduction

let isWhiteSpace =
    function
    | WhiteSpace -> true
    | _ -> false

let isStar =
    function
    | Star -> true
    | _ -> false

let lengthCheck actualLine =
    let leftWhiteSpaceLength = List.takeWhile isWhiteSpace actualLine |> List.length

    let rightWhiteSpaceLength =
        actualLine |> List.rev |> List.takeWhile isWhiteSpace |> List.length

    "Length must be the same"
    |> Expect.equal leftWhiteSpaceLength rightWhiteSpaceLength

let makeTest (n: uint) =
    Romb.create <| int n |> List.iter lengthCheck

let lengthPropertyTest = makeTest |> testProperty "length property test"

let createModuleTest name expected n =
    testCase
        name
        (fun () ->
            let actual = Romb.create n

            List.iter2
                (fun actualLine expectedLine ->
                    "Lines must be the same"
                    |> Expect.sequenceEqual actualLine expectedLine
                )
                actual
                expected
        )

let moduleTests =
    [ let expected = []

      createModuleTest "zero length" expected 0

      let expected = []

      createModuleTest "negative length" expected -3

      let expected =
          [ [ WhiteSpace; Star; WhiteSpace ]
            [ Star; Star; Star ]
            [ WhiteSpace; Star; WhiteSpace ] ]

      createModuleTest "length = 2" expected 2 ]
    |> testList "Module"

let allTests = testList "Romb" [ moduleTests; lengthPropertyTest ]
