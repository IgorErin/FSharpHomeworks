namespace Introduction

/// <summary>
/// Type represent binary tree.
/// </summary>
type Tree<'a> =
    | Node of Tree<'a> * Tree<'a> * 'a
    | Nil

/// <summary>
/// Module for <see cref="Tree"/> processing. 
/// </summary>
module Tree =
    let rec map mapFun =
        function
        | Node(left, right, value) ->
            let map = map mapFun

            (map left, map right, mapFun value) |> Node
        | Nil -> Nil
