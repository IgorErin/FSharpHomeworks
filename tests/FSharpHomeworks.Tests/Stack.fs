module FSharpHomeworks.Tests.Stack

open Expecto
open Introduction.Stack

// single thread tests
let createTest name action stack expected =
    testCase
        name
        (fun () ->
            let actual = action stack

            "Results must be the same" |> Expect.equal actual expected
        )

let singleThreadTests =
    [ let stack = FStack()

      stack.Push(1)
      stack.Push(2)

      createTest "pop" (fun (stack: FStack<_>) -> stack.TryPop()) stack <| Some 2

      let stack = FStack<int>()

      createTest "pop on empty" (fun (stack: FStack<_>) -> stack.TryPop()) stack None ]
    |> testList "single thread tests"
