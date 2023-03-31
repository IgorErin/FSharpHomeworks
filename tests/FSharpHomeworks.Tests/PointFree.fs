module FSharpHomeworks.Tests.PointFree

open Expecto
open Introduction

let checkResult firstFun secondFun (value: int, list: int list) =
    let firstResult = firstFun value list
    let secondResult = secondFun value list

    "Result must be the same" |> Expect.equal firstResult secondResult

let test =
    checkResult PointFree.sourceFunction PointFree.fun3
    |> testProperty "test with source function"
