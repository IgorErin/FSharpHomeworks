module FSharpHomeworks.Tests.SuperMap

open Expecto
open Introduction

let createTest name expected list superFun =
    testCase name (fun () ->
        let actual = Super.map list superFun
        
        "Results must be the same"
        |> Expect.sequenceEqual actual expected 
    )
    
let tests =
    [   let list = [1; 2; 3 ]
        let expected = list
        
        createTest "id list test" expected list (fun x -> [ x ])
        
        let expected = [ -1; 1; -2; 2; -3; 3; ]
        
        createTest "minus plus pair" expected list (fun x -> [ -x; x ])
        
        createTest "empty fun" [] list (fun _ -> [])
        
        createTest "empty list" [] [] (fun x -> [x]) ]
    |> testList "Super.map tests"
    
