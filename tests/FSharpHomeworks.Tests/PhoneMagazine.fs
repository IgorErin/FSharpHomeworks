module FSharpHomeworks.Tests.PhoneMagazine

open Expecto
open Introduction

let creatTest name command sourceList expectedResult =
    testCase
        name
        (fun () ->
            let actual = Magazine.run sourceList command

            "Results must be the same" |> Expect.equal actual expectedResult
        )

let tests =
    [ let someRecord =
          { Name = "Igor"
            Number = "80090898" }

      // single addition
      creatTest "single add" (Add someRecord) [] <| Success [ someRecord ]

      let anotherRecord =
          { Name = "Test"
            Number = "434213" }

      // twos addition
      creatTest "addition pair" (Add anotherRecord) [ someRecord ]
      <| Success [ anotherRecord; someRecord ]

      let sourceList = [ someRecord; anotherRecord ]
      let expectedResult = SuccessWith(Number <| anotherRecord.Number, sourceList)

      // search by name
      creatTest "searchByName" (SearchNumber anotherRecord.Name) sourceList expectedResult

      let expectedResult = SuccessWith(Name <| anotherRecord.Name, sourceList)

      // search by number
      creatTest
          "searchByNumber"
          (SearchName anotherRecord.Number)
          sourceList
          expectedResult

      let show (list: Record list) =
          "lists must be equal" |> Expect.equal list sourceList
          Success list

      // show test
      creatTest "show test" (Show show) sourceList <| Success sourceList

      ]
    |> testList "Phone magazine tests"
