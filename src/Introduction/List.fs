namespace Introduction

/// <summary>
/// List module.
/// </summary>
[<RequireQualifiedAccess>]
module List =
    /// <summary>
    /// Find the position of an item in the list.
    /// </summary>
    /// <param name="item">Item for position search.</param>
    /// <param name="list">The list where the search will be performed.</param>
    let tryFindIndex item list =
        let rec tryFindIndexRec acc list item =
            match list with
            | head :: _ when head = item -> Some acc
            | _ :: tail -> tryFindIndexRec (acc + 1) tail item
            | [] -> None

        tryFindIndexRec 0 list item
