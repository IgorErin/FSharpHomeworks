namespace Introduction

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
        if count > 0 then Seq.reduce (*) [ 1..count ] else 1
