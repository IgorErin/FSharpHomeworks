// For more information see https://aka.ms/fsharp-console-apps

open Introduction

[<EntryPoint>]
let main _ =
    Seq.iter (printf "%A ") <| Fibonacci.run 100
    
    1
