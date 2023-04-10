module Introduction.Round

type RoundBuilder(roundOrder: int) =
    member this.Bind(value: float, f) =
        f <| System.Math.Round(value, roundOrder)

    member this.Return(value: float) =
        System.Math.Round(value, roundOrder)


let rounding order = RoundBuilder(order)
