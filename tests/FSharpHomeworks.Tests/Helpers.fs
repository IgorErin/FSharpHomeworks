namespace FSharpHomeworks.Tests

open FsCheck
open Expecto

module Generators =
    type TestCompatibleList() =
        static let pairOfVectorsOfEqualSize (valuesGenerator: Gen<'a>) =
            gen {
                let! length = Gen.sized <| fun size -> Gen.choose (1, size)

                let! array = Gen.arrayOfLength length valuesGenerator

                return array
            }

        static member IntType() =
            pairOfVectorsOfEqualSize <| Arb.generate<int> |> Arb.fromGen

        static member FloatType() =
            pairOfVectorsOfEqualSize
            <| (Arb.Default.NormalFloat() |> Arb.toGen |> Gen.map float)
            |> Arb.fromGen

        static member SByteType() =
            pairOfVectorsOfEqualSize <| Arb.generate<sbyte> |> Arb.fromGen

        static member ByteType() =
            pairOfVectorsOfEqualSize <| Arb.generate<byte> |> Arb.fromGen

        static member Int16Type() =
            pairOfVectorsOfEqualSize <| Arb.generate<int16> |> Arb.fromGen

        static member UInt16Type() =
            pairOfVectorsOfEqualSize <| Arb.generate<uint16> |> Arb.fromGen

        static member Int32Type() =
            pairOfVectorsOfEqualSize <| Arb.generate<int32> |> Arb.fromGen

        static member UInt32Type() =
            pairOfVectorsOfEqualSize <| Arb.generate<uint32> |> Arb.fromGen

        static member BoolType() =
            pairOfVectorsOfEqualSize <| Arb.generate<bool> |> Arb.fromGen

module Utils =
    let defaultConfig =
        { FsCheckConfig.defaultConfig with
            arbitrary = [ typeof<Generators.TestCompatibleList> ] }

    let floatIsEqual x y =
        abs (x - y) < Accuracy.medium.absolute || x.Equals y
