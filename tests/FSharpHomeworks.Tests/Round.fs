module FSharpHomeworks.Tests.Round

open System
open Expecto
open Introduction.Round

let floatIsEqual x y =
    abs (x - y) < Accuracy.medium.absolute || x.Equals y

let tests =
    [ testCase
          "actual result"
          (fun () ->
              let result =
                  rounding 3 {
                      let! a = 2.0 / 12.0
                      let! b = 3.5
                      return a / b
                  }

              "Result should be the same" |> Expect.isTrue (floatIsEqual result 0.048)
          )

      testCase
          "div by zero"
          (fun () ->
              let result =
                  rounding 1 {
                      let! a = (/) 1.0 <| float 0

                      return a
                  }

              Expect.isTrue (Double.IsInfinity(result)) "infinity result"
          )

      testCase
          "round to -1"
          (fun () ->
              let test () =
                  rounding -1 {
                      let! a = 1.0 / 0.0

                      return a
                  }
                  |> ignore

              Expect.throws test "expected exception"
          ) ]
    |> testList "Round tests"
