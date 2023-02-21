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
        | list -> revAcc list []
