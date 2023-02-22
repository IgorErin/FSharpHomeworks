namespace Introduction

type Either<'a> = Left of string | Right of 'a 

[<RequireQualifiedAccess>]
module Degrees =
    let private intPow value count = 
        let rec pow value count result =
            if count > 0 then
                pow value (count - 1) (result * value)
            else
                result
        
        pow value count 1
        
    let private degree2 count =
        let rec degreeOf2Rec n powCount result =
            if n > 0 then 
                if n % 2 <> 0 then result * (intPow 2 powCount) else result
                |> degreeOf2Rec (n / 2) (powCount + 1)
            else
                result 
            
        degreeOf2Rec count 1 1
        
    let rec private seqOfMulBy2 itemInNDegree m =
        seq {
            if m > 0 then
                let newItem = itemInNDegree * 2
                
                yield newItem
                yield! seqOfMulBy2 newItem m - 1
        }
    
    let seqOf2 n m =
        if n > 0 && m > 0 then
            let firstItem = degree2 n
            seq {
                yield firstItem
                yield! seqOfMulBy2 firstItem m 
            }
            |> Right
        else Left <| "Invalid input data: n or m less than 0" 
