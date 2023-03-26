module FSharpHomeworks.Tests.Interpreter

open Expecto
open Interpreter

/// <summary>
/// Create test for some function.
/// </summary>
let createTest testFun term expected testName =
    testCase
        testName
        (fun () ->
            let actual = testFun term

            "Results must be the same" |> Expect.equal actual expected
        )

/// <summary>
/// Create test for Lambda.reduce.
/// </summary>
let createReduceTest = createTest Lambda.reduce

/// <summary>
/// Id abstraction reduce test.
/// </summary>
let abstractionTest =
    let term = Abs("name", Var("name"))

    "Id abstraction reduce" |> createReduceTest term term

/// <summary>
/// Var reduce test.
/// </summary>
let varTest =
    let term = Var "name"

    "Var reduce" |> createReduceTest term term

/// <summary>
/// Application of id abstraction to a variable test.
/// </summary>
let applicationOfIdAbstractionTest =
    let id = Abs("name", Var("name"))
    let var = Var("anotherName")

    let app = App(id, var)

    "Id application reduce" |> createReduceTest app var

/// <summary>
/// Reduce application of two variables test.
/// </summary>
let applicationTest =
    let leftVar = Var("someName")
    let rightVar = Var("anotherName")

    let app = App(leftVar, rightVar)

    "Var/Var application reduce" |> createReduceTest app app

/// <summary>
/// Reduce tests.
/// </summary>
let reduceTest =
    testList
        "Reduce tests"
        [ abstractionTest; varTest; applicationOfIdAbstractionTest; applicationTest ]

/// <summary>
/// Create substitution test.
/// </summary>
/// <param name="varName">Variable for substitute.</param>
/// <param name="termToSubstitute">Term for substitute.</param>
/// <param name="term">The term in which the substitution will occur.</param>
/// <param name="expected">Expected result.</param>
/// <param name="testName">Test name.</param>
let createSubstituteTest varName termToSubstitute term expected testName =
    testCase
        testName
        (fun () ->
            let actual = Lambda.substitute varName termToSubstitute term

            "Terms must be the same" |> Expect.equal actual expected
        )

/// <summary>
/// Substitute a variable instead of a variable.
/// </summary>
let substituteVariableToVariableTests =
    [ let firstVarName = "someName"
      let substituteVarName = "anotherName"
      let yetAnotherName = "yetAnotherName"

      let var = Var firstVarName
      let varToSubstitute = Var substituteVarName

      "Substitute variable to variable"
      |> createSubstituteTest firstVarName varToSubstitute var varToSubstitute

      "Try to substitute variable to variable with another name"
      |> createSubstituteTest yetAnotherName varToSubstitute var var ]
    |> testList "substituteVariableToVariableTests"

/// <summary>
/// Substitution into a free abstraction variable.
/// </summary>
let substituteToAbstractionWithFreeVariableTest =
    [ let freeVariable = Var "freeVariableName"
      let abstraction = Abs("someName", freeVariable)
      let varToSubstitute = Var("anotherName")

      let expected = Abs("someName", varToSubstitute)

      "Substitute variable to free variable in abstraction"
      |> createSubstituteTest "freeVariableName" varToSubstitute abstraction expected ]
    |> testList "substituteToAbstractionWithFreeVariableTest"

/// <summary>
/// Substitution by abstraction name test.
/// </summary>
let substituteToAbstractionWithSameArgumentName =
    let abstraction = Abs("abstractionName", Var("someName"))
    let varToSubstitute = Var("substituteVarName")

    "Try to substitute variable to abstraction"
    |> createSubstituteTest "abstractionName" varToSubstitute abstraction abstraction

/// <summary>
/// Substitution into abstraction with renaming.
/// </summary>
let substituteToAbstractionWithFreeVariables =
    [ let abstraction = Abs("_", Abs("_", Var "x"))
      let substituteTerm = Var("substituteName")

      let expected = Abs("_", Abs("_", substituteTerm))

      "Substitute variable with free name in left term"
      |> createSubstituteTest "x" substituteTerm abstraction expected

      let abstraction = Abs("y", App(Abs("_", Var("x")), Var("y")))
      let substituteTerm = Abs("_", Var "y")

      let expected = Abs("y$", App(Abs("_", Abs("_", Var "y")), Var("y$")))

      "Substitute with rename test"
      |> createSubstituteTest "x" substituteTerm abstraction expected ]
    |> testList "substituteToAbstractionWithFreeVariables"

/// <summary>
/// Substitution tests.
/// </summary>
let substituteTests =
    testList
        "Substitute tests"
        [ substituteVariableToVariableTests
          substituteToAbstractionWithFreeVariableTest
          substituteToAbstractionWithSameArgumentName
          substituteToAbstractionWithFreeVariables ]

let tests = testList "Lambda tests" [ substituteTests; reduceTest ]
