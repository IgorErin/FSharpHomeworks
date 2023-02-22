namespace FSharpHomeworks.Tests

open FsCheck
open Expecto

/// <summary>
/// Generators for property based testing.
/// </summary>
module Generators =
    /// <summary>
    /// Type that generate test compatible lists.
    /// </summary>
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

/// <summary>
/// Module with test common functionality.
/// </summary>
module Utils =
    // TODO(type reference)
    /// <summary>
    /// Default test config.
    /// </summary>
    /// <remarks>
    /// Without nan and Infinity for <see cref="System.Double"/>.
    /// </remarks>
    let defaultConfig =
        { FsCheckConfig.defaultConfig with
            arbitrary = [ typeof<Generators.TestCompatibleList> ]
            maxTest = 10 }

    /// <summary>
    /// IsEqual for <see cref="System.Double"/>.
    /// </summary>
    let floatIsEqual x y =
        abs (x - y) < Accuracy.medium.absolute || x.Equals y
