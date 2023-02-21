namespace Introduction

[<RequireQualifiedAccess>]
module Fibonacci =
    let run count = 
        let rec run count fst snd = seq {
            if count > 0 then 
                yield fst
                yield! run (count - 1) snd (fst + snd)
        }
        
        run count 0 1 
