// For more information see https://aka.ms/fsharp-console-apps

open Introduction

[<EntryPoint>]
let main _ =
    let showList list =
        List.iter (fun record -> printfn $"%A{record}") list
        Success list

    let showError = printfn "error %s"
    let showData = printfn "data %A"

    Interactive.run showData showList showError ()

    0
