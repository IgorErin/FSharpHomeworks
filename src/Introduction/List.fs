namespace Introduction

/// <summary>
/// List module.
/// </summary>
[<RequireQualifiedAccess>]
module List =
    let rec private revRec list acc =
        match list with
        | head :: tail -> revRec tail (head :: acc)
        | [] -> acc

    /// <summary>
    /// Reverse list.
    /// </summary>
    let revers =
        function
        | [] -> []
        | list -> revRec list []
