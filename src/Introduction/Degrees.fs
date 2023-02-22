namespace Introduction

type Either<'a> =
    | Left of string
    | Right of 'a

[<RequireQualifiedAccess>]
module Degrees =
    let private degreeOf2 count =
        let rec degreeOf2Rec count multiplier acc =
            if count > 0 then
                if count % 2 <> 0 then acc * multiplier else acc
                |> degreeOf2Rec (count / 2) (multiplier * multiplier)
            else
                acc

        degreeOf2Rec count 2 1

    let rec private seqOfMul prevItem m =
        seq {
            if m > 0 then
                let newItem = prevItem * 2

                yield newItem
                yield! seqOfMul newItem <| m - 1
        }

    let seqOf2 n m =
        if n >= 0 && m >= 0 then
            let firstItem = degreeOf2 n

            seq {
                yield firstItem
                yield! seqOfMul firstItem m
            }
            |> Seq.toList
            |> Right
        else
            Left <| "Invalid input data: n or m less than 0"
