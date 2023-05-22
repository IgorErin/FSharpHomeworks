module FSharpHomeworks.Tests.InfectNet

open Expecto
open Introduction.Net

let createTest
    name
    (net: IInfectNet)
    (expectedState: (Id * bool) list)
    (expectedIterationCount: int option)
    =
    test name {
        let maxIterationCount = 1000

        let mutable iterationCount = 0

        while not <| net.InfectionIsOver() && iterationCount < maxIterationCount do

            net.Infect()
            iterationCount <- iterationCount + 1

        match expectedIterationCount with
        | Some value ->
            "Iteration count must be the same" |> Expect.equal iterationCount value
        | None -> ()

        List.iter
            (fun (id, isInfect) ->
                let actual = List.find ((>>) fst <| (=) id) <| net.State() |> snd

                $"Result must be the same. Node id: %A{id}"
                |> Expect.equal actual isInfect
            )
            expectedState
    }

let createLinkedNet predicate =
    let predicate = Predicate(predicate)

    let firstNode: IInfectNode = InfectNode(false, predicate, "first")
    let secondNode: IInfectNode = InfectNode(false, predicate, "second")
    let thirdNode: IInfectNode = InfectNode(false, predicate, "third")

    let forthNode: IInfectNode = InfectNode(true, predicate, "forth")

    let net =
        [ forthNode, [ thirdNode ]
          thirdNode, [ secondNode ]
          secondNode, [ firstNode ] ]

    InfectNet(net)

let linkedTests =
    [
      // always
      let alwaysNet = createLinkedNet Always
      let expectedResult = [ "first", true; "second", true; "third", true; "forth", true ]
      let expectedIterationCount = 3

      createTest "always" alwaysNet expectedResult <| Some expectedIterationCount

      // often
      let oftenNet = createLinkedNet Often

      createTest "often" oftenNet expectedResult None

      // rare
      let rareNet = createLinkedNet Rarely

      createTest "rare" rareNet expectedResult None

      // never
      let expectedResult =
          [ "first", false; "second", false; "third", false; "forth", true ]

      let neverNet = createLinkedNet Never

      createTest "never" neverNet expectedResult None ]
    |> testList "linked"

let createBipartiteNet predicate =
    let predicate = Predicate(predicate)

    let firstNode: IInfectNode = InfectNode(false, predicate, "first")
    let secondNode: IInfectNode = InfectNode(false, predicate, "second")
    let thirdNode: IInfectNode = InfectNode(false, predicate, "third")

    let forthNode: IInfectNode = InfectNode(true, predicate, "forth")

    let net =
        [ forthNode, [ thirdNode ]
          thirdNode, [ forthNode ]

          secondNode, [ firstNode ]
          firstNode, [ secondNode ] ]

    InfectNet(net)

let bipartiteTests =
    [ let alwaysNet = createBipartiteNet Always

      let expectedResult =
          [ "first", false
            "second", false

            "third", true
            "forth", true ]

      createTest "always" alwaysNet expectedResult None

      // often
      let oftenNet = createBipartiteNet Often

      createTest "often" oftenNet expectedResult None

      // rare
      let rareNet = createBipartiteNet Rarely

      createTest "rare" rareNet expectedResult None

      // never
      let expectedResult =
          [ "first", false; "second", false; "third", false; "forth", true ]

      let neverNet = createLinkedNet Never

      createTest "never" neverNet expectedResult None ]
    |> testList "bipartite"

let allTests = testList "Infect net" [ linkedTests; bipartiteTests ]
