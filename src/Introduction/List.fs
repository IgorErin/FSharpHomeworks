namespace Introduction

[<RequireQualifiedAccess>]
module List =
    let rec find<'a when 'a : equality> (item: 'a) list =
        match list with
        | head :: _ when head = item ->
            Some item
        | _ :: tail ->  find item tail
        | [] -> None
        
