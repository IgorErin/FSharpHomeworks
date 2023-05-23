module Introduction

open System.Threading

type ILazy<'a> =
    abstract member Get: unit -> 'a

module Naive =
    type Lazy<'a>(action) =
        let mutable result = None

        interface ILazy<'a> with
            member this.Get() =
                Option.defaultWith
                    (fun () ->
                        let localResult = action ()

                        result <- Some localResult
                        localResult
                    )
                    result

module Lock =
    type Lazy<'a>(action) =

        let locker = obj ()

        [<VolatileField>]
        let mutable result = None

        interface ILazy<'a> with
            member this.Get() =
                // first check
                Option.defaultWith
                    (fun () ->
                        // lock
                        lock locker
                        <| (fun () ->
                            // second check
                            Option.defaultWith
                                (fun () ->
                                    // computation
                                    let localResult = action ()

                                    result <- Some localResult
                                    localResult
                                )
                                result
                        )
                    )
                    result

module LockFree =
    type Lazy<'a>(action) =
        [<VolatileField>]
        let mutable result = None

        interface ILazy<'a> with
            member this.Get() =
                Option.defaultWith
                    (fun () ->
                        let localResult = action ()

                        Interlocked.CompareExchange(&result, Some localResult, result)
                        |> ignore

                        result.Value
                    )
                    result

[<EntryPoint>]
let main args =

    let lz = LockFree.Lazy(fun _ -> 1) :> ILazy<_>

    lz.Get() |> printfn "%A"

    0
