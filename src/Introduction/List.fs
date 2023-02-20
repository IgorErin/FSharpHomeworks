namespace Introduction

[<RequireQualifiedAccess>]
module List =
    let rec private revAcc list acc =
        match list with
        | head :: tail -> revAcc tail (head :: acc)
        | [] -> acc

    let revers =
        function
        | [] -> []
        | [ _ ] as list -> list
        | fst :: snd :: tail -> revAcc tail [ snd ; fst ]
