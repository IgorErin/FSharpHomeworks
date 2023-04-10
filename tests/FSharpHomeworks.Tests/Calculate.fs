module FSharpHomeworks.Tests.Calculate

open Expecto
open Introduction.Calculate

let makeTest leftInt rightInt =

    let leftString = string leftInt
    let rightString = string rightInt

    let actual =
        calculate {
            let! left = leftString
            let! right = rightString

            return left + right
        }

    let expected = Success(leftInt + rightInt)

    "results must be the same" |> Expect.equal actual expected

let sumTests = makeTest |> testProperty "sum"

let failTest =
    testCase
        "success result"
        (fun () ->
            let actual =
                calculate {
                    let! left = "notNumber"
                    let! right = "2"

                    let result = left + right

                    return result
                }

            let expected = Error <| "notNumber is not a number"

            "Fail computations" |> Expect.equal actual expected

        )

let allTests = testList "Computations" [ sumTests; failTest ]
