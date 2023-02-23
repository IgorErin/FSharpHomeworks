namespace Introduction

type Either<'a> =
    | Left of string
    | Right of 'a

/// <summary>
/// Factorial module.
/// </summary>
module Factorial =
    /// <summary>
    /// Run factorial computation.
    /// </summary>
    /// <param name="count">
    /// Number whose factorial will be calculated.
    /// </param>
    let run count =
        if count > 0 then
            Right <| Seq.reduce (*) [ 1..count ]
        elif count = 0 then
            Right <| 1
        else
            Left <| "Invalid data: factorial is not defined for negative"
