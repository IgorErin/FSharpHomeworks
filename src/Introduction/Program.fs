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

        let lockCheckAndCompute () =
            lock locker
            <| (fun () ->
                Option.defaultWith
                    (fun () ->
                        result <- Some <| action ()
                        result.Value
                    )
                    result
            )

        interface ILazy<'a> with
            member this.Get() =
                Option.defaultWith lockCheckAndCompute result

module LockFree =
    type Lazy<'a>(action) =
        let mutable result = None

        interface ILazy<'a> with
            member this.Get() =
                Option.defaultWith
                    (fun () ->
                        Interlocked.CompareExchange(&result, Some <| action (), None)
                        |> ignore

                        result.Value
                    )
                    result
