namespace Introduction

/// <summary>
/// Either type.
/// </summary>
/// <remarks>
/// Left contain error message.
/// Right contain result value.
/// </remarks>
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

    /// <summary>
    /// Gives a sequence of powers of two.
    /// </summary>
    /// <example>
    /// For some n and m ...
    /// result = [ 2^n; 2^(n + 1); 2^(m + 2); ... ; 2^(n + m) ]
    /// </example>
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
