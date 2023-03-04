namespace FSharpHomeworks.Tests

open FsCheck
open Expecto
open Introduction

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

    type ArithmeticTreeGenerator() =
        static let generateBinOp (valueGenerator: Gen<ArithmeticTree<'a>>) =
            gen {
                let! binOp =
                    Gen.oneof [ Gen.constant Add; Gen.constant Sub; Gen.constant Mul ]

                let! leftTree = valueGenerator
                let! rightTree = valueGenerator

                return BinOp(leftTree, binOp, rightTree)
            }

        static let generateLiteral (valueGenerator: Gen<'a>) =
            gen {
                let! value = valueGenerator

                return Literal value
            }

        static let rec generateTree (valueGenerator: Gen<'a>) =
            gen {
                let generateBinOp = generateBinOp <| generateTree valueGenerator
                let generateLiteral = generateLiteral valueGenerator

                return! Gen.oneof [ generateLiteral; generateBinOp ]
            }

        static member IntType() =
            generateTree <| Arb.generate<int> |> Arb.fromGen

        static member FloatType() =
            generateTree <| (Arb.Default.NormalFloat() |> Arb.toGen |> Gen.map float)
            |> Arb.fromGen

        static member SByteType() =
            generateTree <| Arb.generate<sbyte> |> Arb.fromGen

        static member ByteType() =
            generateTree <| Arb.generate<byte> |> Arb.fromGen

        static member Int16Type() =
            generateTree <| Arb.generate<int16> |> Arb.fromGen

        static member UInt16Type() =
            generateTree <| Arb.generate<uint16> |> Arb.fromGen

        static member Int32Type() =
            generateTree <| Arb.generate<int32> |> Arb.fromGen

        static member UInt32Type() =
            generateTree <| Arb.generate<uint32> |> Arb.fromGen

        static member BoolType() =
            generateTree <| Arb.generate<bool> |> Arb.fromGen

module Utils =
    let defaultConfig =
        { FsCheckConfig.defaultConfig with
            arbitrary = [ typeof<Generators.TestCompatibleList> ]
            maxTest = 10 }

    let floatIsEqual x y =
        abs (x - y) < Accuracy.medium.absolute || x.Equals y
