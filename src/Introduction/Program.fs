// For more information see https://aka.ms/fsharp-console-apps
open Introduction
open Round

let result =
    rounding 3 {
        let! a = 2.0 / 12.0
        let! b = 3.5
        return a / b
    }

printfn "round result: %A" result
