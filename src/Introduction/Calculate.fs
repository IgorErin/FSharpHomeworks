module Introduction.Calculate

open System

type Result =
    | Success of int
    | Error of string

let tryConvertString (str: string) =
    let mutable result = ref 0

    if Int32.TryParse(str, result) then
        Success result.Value
    else
        Error <| $"%s{str} is not a number"

type Calculate() =
    member this.Bind(value: string, cont) =
        let tryConvert = tryConvertString value

        match tryConvert with
        | Success value -> cont value
        | Error _ -> tryConvert

    member this.Return(value) = Success value

let calculate = Calculate()
