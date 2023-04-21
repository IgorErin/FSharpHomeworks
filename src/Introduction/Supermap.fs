module Introduction.Super

let map list superFun =
    List.map superFun list |> List.concat
