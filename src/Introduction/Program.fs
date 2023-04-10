// For more information see https://aka.ms/fsharp-console-apps

open Introduction.Net

module InfectNet =
    let ill = "ill"
    
    let notIll = "not ill"
    
    let maxIterations = 10000
    
    let run nodes adjacencyMatrix =
        let net = InfectNet(adjacencyMatrix, nodes)
        
        let rec helper count (net: IInfectNet) =
            if count < maxIterations then
                let existsNotIllNodes =
                    net.GetState()
                    |> Array.exists (not << snd)
                    
                if existsNotIllNodes then
                    // print result
                    net.GetState()
                    |> Array.map (fun (id, isIll) -> $"node {id} is {if isIll then ill else notIll}")
                    |> String.concat "\n"
                    |> printfn "%s"
                    
                    net.RunStep()
                    helper (count + 1)  net
                
                else
                    printfn "All nodes are infected"
                    
            else
                printfn "Too much work, it's time for computers to rest"
                
        helper 0 net 
