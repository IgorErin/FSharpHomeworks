namespace Introduction

/// <summary>
/// Binary operation identifier.
/// </summary>
type BinOp =
    | Add
    | Sub
    | Mul

    /// <summary>
    /// Convert identifier to function.
    /// </summary>
    member this.ToFunction() =
        match this with
        | Add -> (+)
        | Sub -> (-)
        | Mul -> (*)

/// <summary>
/// Arithmetic tree.
/// </summary>
type ArithmeticTree<'a> =
    | Literal of 'a
    | BinOp of ArithmeticTree<'a> * BinOp * ArithmeticTree<'a>

/// <summary>
/// Arithmetic tree module.
/// </summary>
module ArithmeticTree =
    let rec eval =
        function
        | BinOp(left, op, right) ->
            let leftResult = eval left
            let rightResult = eval right

            op.ToFunction () leftResult rightResult
        | Literal value -> value
