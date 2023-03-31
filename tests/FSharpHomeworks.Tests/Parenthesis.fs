module FSharpHomeworks.Tests.Parenthesis

open Expecto
open Introduction

let createTest pairs parenthesis expected =
    testCase
        $"test on {parenthesis}"
        (fun () ->
            let actual = Parenthesis.parse parenthesis pairs

            "Results must be the same" |> Expect.equal expected actual
        )

let unitTests =
    [ let chars = "(())"
      let pairs = [ '(', ')' ]
      let expected = Success

      createTest pairs chars expected

      let chars = "()(){}{{(({}))}}{}"
      let pairs = [ '(', ')'; '{', '}' ]
      let expected = Success

      createTest pairs chars expected

      let chars = "((}}"
      let pairs = [ '(', ')'; '{', '}' ]
      let expected = Error "Expected: ')', actual: '}'"

      createTest pairs chars expected

      let chars = ""
      let pairs = [ '(', '}'; '{', '}' ]
      let expected = Error "Ambiguity, the symbol } occurs 2 times"

      createTest pairs chars expected

      let chars = "(("
      let pairs = [ '(', ')'; '{', '}' ]
      let expected = Error "Incorrect number of brackets"

      createTest pairs chars expected

      let chars = "((}"
      let pairs = [ '(', ')'; '{', '}' ]
      let expected = Error "Expected: ')', actual: '}'"

      createTest pairs chars expected

      let chars = "(()"
      let pairs = [ '(', ')'; '{', '}' ]
      let expected = Error "Incorrect number of brackets"

      createTest pairs chars expected ]
    |> testList "parse unit tests"
