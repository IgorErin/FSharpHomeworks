namespace Introduction

/// <summary>
/// Fibonacci module.
/// </summary>
[<RequireQualifiedAccess>]
module Fibonacci =
    /// <summary>
    /// Get fibonacci sequence with specified lenght.
    /// </summary>
    /// <param name="length">
    /// Sequence lenght.
    /// </param>
    let run length =
        let rec run count fst snd =
            seq {
                if count > 0 then
                    yield fst
                    yield! run (count - 1) snd (fst + snd)
            }

        run length 0 1
