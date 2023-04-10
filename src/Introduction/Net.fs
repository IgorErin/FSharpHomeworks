namespace Introduction.Net

open Introduction
open Expecto
open FsCheck

type OS =
    | Windows
    | Linux
    | OsX
    
type Id = string
    
type Predicate(probability: float) =
    do
        if probability < 0
            || probability > 1 then
                failwith "Probability must be in range"
    
    let floatIsEqual x y =
        abs (x - y) < Accuracy.medium.absolute
        || x.Equals y
        
    let trueGen = Gen.constant true
    let falseGen = Gen.constant false
    
    let count = int <| probability * 100.0
    
    let frequency = [(count, trueGen); (100 - count, falseGen)]
    
    member this.Run () =
        Gen.frequency frequency
        |> Gen.sample 1 1
        |> List.last 
        
    member this.IsZeroProbability = floatIsEqual probability 0.0
    
type INode =
    abstract IsIll: bool
    
    abstract Id: Id
    
    abstract TryInfect: unit -> unit
    
    abstract CanIll: bool
    
type Node(isIll, predicate: Predicate, id: Id) =    
    let mutable isIll = isIll 
    
    interface INode with 
        member this.IsIll with get() = isIll
        
        member val Id = id with get
        
        member val CanIll = predicate.IsZeroProbability
        
        member this.TryInfect() = isIll <- predicate.Run()
                
type IInfectNet =
    abstract RunStep: unit -> unit
    
    abstract GetState: unit -> (Id * bool) array 
                
type InfectNet(adjacencyMatrix: bool [,], nodes: INode []) =   
    do
        // validate
        if Array2D.length1 adjacencyMatrix <> Array2D.length2 adjacencyMatrix
            then failwith "The matrix must be square"
        
        if Array2D.length1 adjacencyMatrix <> Array.length nodes
            then failwith "The length of the matrix must be equal to the number of nodes"
        
        nodes
        |> Array.distinctBy (fun node -> node.Id)
        |> Array.length
        |> fun length -> if length <> Array.length nodes then failwith "Node IDs should be different"
       
    /// create lookup table consisting of (node, his neighbors)
    let table =
        (adjacencyMatrix, nodes)
        ||> Array2D.mapByRow (fun isConnected node -> if isConnected then Some node else None)   
        |> Array.map (Array.choose id) 
        |> Array.map2 (fun node neighbors -> node, neighbors) nodes
        
    interface IInfectNet with 
        member this.RunStep() =
            table
            // extract neighbors of ill nodes
            |> Array.filter (fst >> fun node -> node.IsIll)
            |> Array.map snd
            |> Array.concat
            // try infect them
            |> Array.iter (fun node -> node.TryInfect())
            
        member this.GetState() =
            nodes |> Array.map (fun node -> node.Id, node.IsIll)
        
        
