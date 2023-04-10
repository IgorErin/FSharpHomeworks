module FSharpHomeworks.Tests.InfectNet

open Expecto
open Introduction.Net

let createTest name matrix nodes expectedResult =
    testCase name (fun () -> 
        let net = InfectNet(matrix, nodes)
        
        (net :> IInfectNet).RunStep()
        
        let actualResult = (net :> IInfectNet).GetState()
        
        "Results must be the same"
        |> Expect.equal actualResult expectedResult)
let sourceMatrix: bool [,] =
    Array2D.init 3 3 (fun id1 id2 -> id1 = id2)
    

let tests =
    [ 
      createTest "bfs test" 
      
       ]
