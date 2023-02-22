namespace Introduction

[<RequireQualifiedAccess>]
module List = 
    let tryFindIndex item list =
       let rec tryFindIndexRec acc list item =    
         match list with
         | head :: _ when head = item -> Some acc
         | _ :: tail -> tryFindIndexRec (acc + 1) tail item
         | [] -> None
         
       tryFindIndexRec 0 list item    
