namespace Introduction

module Numbers =
    /// <summary>
    /// Int sqrt.
    /// </summary>
    let intSqrt = int << sqrt << float

    /// <summary>
    /// Is prime.
    /// </summary>
    /// <param name="n"></param>
    let isPrime n =
        Seq.forall (fun listItem -> n % listItem <> 0) [ 2 .. intSqrt n ]

    /// <summary>
    /// All primary numbers in form of seq.
    /// </summary>
    let primary = Seq.initInfinite ((+) 2) |> Seq.filter isPrime
