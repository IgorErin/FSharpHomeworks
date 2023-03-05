namespace Introduction

/// <summary>
/// Binary operation identifier.
/// </summary>
type BinOp =
    | Add
    | Sub
    | Mul

/// <summary>
/// Arithmetic tree.
/// </summary>
type ArithmeticTree<'a> =
    | Literal of 'a
    | BinOp of ArithmeticTree<'a> * BinOp * ArithmeticTree<'a>

/// <summary>
/// Arithmetic tree module.
/// </summary>
module Arithmetic =
    /// <summary>
    /// Eval <see cref="Introduction.BinOp"/>.
    /// </summary>
    /// <param name="op">Operation to eval.</param>
    let inline evalBinOp op =
        match op with
        | Add -> (+)
        | Sub -> (-)
        | Mul -> (*)

    /// <summary>
    /// Eval tree.
    /// </summary>
    let rec evalTree tree =
        match tree with
        | BinOp(left, op, right) ->
            let leftResult = evalTree left
            let rightResult = evalTree right

            evalBinOp op leftResult rightResult
        | Literal value -> value
