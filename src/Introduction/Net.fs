namespace Introduction.Net

open FsCheck

type Probability =
    | Always
    | Often
    | Rarely
    | Never

type Id = string

type Predicate(probability: Probability) =
    let trueGen = Gen.constant true
    let falseGen = Gen.constant false

    let maxProbability = 100

    let frequency count =
        [ (count, trueGen); (maxProbability - count, falseGen) ]

    // map Gen frequency to bool
    let frequencyToResult frequency =
        frequency |> Gen.frequency |> Gen.sample 1 1 |> List.last

    member this.Run() =
        match probability with
        | Always -> true
        | Often ->
            // hardcoded frequency
            frequency 75 |> frequencyToResult
        | Rarely ->
            // agan
            frequency 25 |> frequencyToResult
        | Never -> false

    member this.IsZeroProbability =
        match probability with
        | Never -> true
        | _ -> false

type IInfectNode =
    abstract IsInfected: bool

    abstract Id: Id

    abstract TryInfect: unit -> unit

    abstract CanGetInfected: bool

type InfectNode(isInfected, predicate: Predicate, id: Id) =
    let mutable isInfected = isInfected

    interface IInfectNode with
        member this.IsInfected = isInfected

        member val Id = id with get

        member val CanGetInfected = not predicate.IsZeroProbability

        member this.TryInfect() =
            if not isInfected then
                isInfected <- predicate.Run()

type IInfectNet =
    abstract Infect: unit -> unit

    abstract InfectionIsOver: unit -> bool

    abstract State: unit -> (Id * bool) list

type InfectNet(table: (IInfectNode * IInfectNode list) list) =
    let extractInfectedNodeNeighbors () =
        table
        // extract neighbors of ill nodes
        |> List.filter (fst >> fun node -> node.IsInfected)
        |> List.map snd
        // concat them
        |> List.concat
        |> List.distinctBy (fun node -> node.Id)

    interface IInfectNet with
        member this.InfectionIsOver() =
            extractInfectedNodeNeighbors ()
            // among them we are looking for those who may be infected
            |> List.exists (fun node -> node.CanGetInfected && not <| node.IsInfected)
            |> not

        member this.Infect() =
            extractInfectedNodeNeighbors ()
            // try infect them
            |> List.iter (fun node -> node.TryInfect())

        member this.State() =
            let firstItems = List.map fst table
            let secondItems = List.map snd table |> List.concat

            let allNodes =
                List.concat [ firstItems; secondItems ]
                |> List.distinctBy (fun node -> node.Id)

            allNodes |> List.map (fun node -> node.Id, node.IsInfected)
