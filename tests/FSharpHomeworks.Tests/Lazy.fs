module FSharpHomeworks.Tests.Lazy

open System.Threading
open System.Threading.Tasks
open Expecto
open Introduction

let createGetTest name (iLazy: ILazy<_>) expected =
    test name {
        let actual = iLazy.Get()

        "Results must be the same" |> Expect.equal actual expected
    }

let getTests =
    [ let expected = obj ()

      createGetTest "naive" (Naive.Lazy(fun () -> expected)) expected
      createGetTest "lock" (Lock.Lazy(fun () -> expected)) expected
      createGetTest "lockFree" (LockFree.Lazy(fun () -> expected)) expected ]
    |> testList "get"

let replicateRunAndWait (event: ManualResetEvent) count (lz: ILazy<_>) =
    Seq.replicate count lz
    |> Seq.map (fun lz -> async { return lz.Get() })
    |> Async.Parallel
    |> Async.StartAsTask
    |> fun task ->
        event.Set() |> ignore
        task.Result

// Test that all computations return the same result
let createEventTest count (lazyConstructor: (unit -> _) -> ILazy<_>) name =
    test name {
        use event = new ManualResetEvent(false)

        let action =
            fun () ->
                event.WaitOne() |> ignore
                obj ()

        let lz = lazyConstructor action

        replicateRunAndWait event count lz
        |> Seq.pairwise
        |> Seq.map obj.ReferenceEquals
        |> Seq.reduce (&&)
        |> fun isEqual -> Expect.isTrue isEqual "The results should be equal."
    }

let lockTest =
    [ for iteration in 1..10000 do
          let createLockLazy = fun action -> Lock.Lazy(action) :> ILazy<_>

          yield createEventTest 10 createLockLazy $"lock %i{iteration}"

          let createLockFreeLazy = fun action -> Lock.Lazy(action) :> ILazy<_>

          yield createEventTest 10 createLockFreeLazy $"lockFree %i{iteration}" ]
    |> testList "same result"

let sideEffectTest =
    test "side effect" {
        let sourceCount = 0
        let count = ref sourceCount

        use event = new ManualResetEvent(false)

        let lz =
            Lock.Lazy(fun () ->
                event.WaitOne() |> ignore
                Interlocked.Increment(count) |> ignore
                ()
            )

        replicateRunAndWait event 100 lz |> ignore

        "Values must be the same" |> Expect.equal count.Value (sourceCount + 1)
    }

let allTests = testList "Lazy" [ getTests; lockTest; sideEffectTest ]
