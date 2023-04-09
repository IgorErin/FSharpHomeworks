namespace Introduction.Net

open Introduction

type Predicate = unit -> bool 

type OS =
    | Windows
    | Linux
    | OsX
    
type Id = string 
    
type Node(isIll, predicate: Predicate, id: Id) =    
    let mutable isIll = isIll
    
    member this.IsIll with get() = isIll
    
    member val Id = id with get
    
    member this.TryInfect() = if not this.IsIll then isIll <- predicate ()
                        
type InfectNet(adjacencyMatrix: bool [,], nodes: Node []) =   
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
        
        
