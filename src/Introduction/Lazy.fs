module Introduction.Lazy

open System.Threading

type ILazy<'a> =
    abstract member Get: unit -> 'a

module Simple =
    type Lazy<'a>(action) =
        let mutable result = None

        interface ILazy<'a> with
            member this.Get() =
                match result with
                | None ->
                    let localResult = action ()

                    result <- Some localResult

                    localResult
                | Some result -> result

module Blocking =

    type Lazy<'a>(action) =
        let locker = obj ()

        let mutable isComputed = ref false

        member this.IsComputed = isComputed

        interface ILazy<'a> with
            member this.Get() =
                if this.IsComputed.Value then
                    getValue local
                else
                    lock
                        locker
                        (fun () ->
                            let result = (local :> ILazy<'a>).Get()

                            Volatile.Write(isComputed, true)

                            result
                        )
