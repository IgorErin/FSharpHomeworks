// For more information see https://aka.ms/fsharp-console-apps

open Introduction.Net


type ParseResult = Success of OS | Error of string
    
let map = function
    | "linux" -> Success Linux
    | "windows" -> Success Windows
    | "osX" -> Success OsX
    | string -> Error string
    
[<EntryPoint>]
let main args =
    
    let 

    
