namespace Introduction

type BinOp =
    | Add
    | Sub
    | Mul

    member this.ToFunction() =
        match this with
        | Add -> (+)
        | Sub -> (-)
        | Mul -> (*)


type ArithmeticTree<'a> =
    | Literal of 'a
    | BinOp of ArithmeticTree<'a> * BinOp * ArithmeticTree<'a>

module ArithmeticTree =
    let rec eval =
        function
        | BinOp(left, op, right) ->
            let leftResult = eval left
            let rightResult = eval right

            op.ToFunction () leftResult rightResult
        | Literal value -> value
