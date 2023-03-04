module FSharpHomeworks.Tests.ArithmeticTree

open Introduction
open Expecto

let config = Utils.defaultConfig

/// <summary>
/// Evaluate arithmetic tree.
/// </summary>
/// <param name="tree">Tree to eval.</param>
let rec evalTree tree =
    match tree with
    | Literal value -> value
    | BinOp(leftTree, op, rightTree) ->
        let leftResult = evalTree leftTree
        let rightResult = evalTree rightTree

        match op with
        | Add -> leftResult + rightResult
        | Sub -> leftResult - rightResult
        | Mul -> leftResult * rightResult

/// <summary>
/// Check result.
/// </summary>
/// <param name="isEqual">Is equal function.</param>
/// <param name="tree">Tree to eval.</param>
let inline checkResult isEqual tree =
    let actualResult = ArithmeticTree.eval tree
    let expectedResult = evalTree tree

    "Results must be the same"
    |> Expect.isTrue (isEqual actualResult expectedResult)

let test =
    checkResult (=) |> testPropertyWithConfig config "Int arithmetic tree test"
