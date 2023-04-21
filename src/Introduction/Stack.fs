namespace Introduction.Stack

open System.Collections.Generic

type FStack<'a>() =
    let stack = Stack<'a>()

    let locker = obj ()

    member this.Push(item) =
        lock locker (fun () -> stack.Push(item))

    member this.TryPop() =
        lock
            locker
            (fun () ->
                let result = ref Unchecked.defaultof<'a>

                if stack.TryPop(result) then Some result.Value else None
            )
