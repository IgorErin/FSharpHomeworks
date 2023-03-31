module Introduction

type Position = int

type Message = string

type Id = int

/// <summary>
/// Type that represent parenthesis.
/// </summary>
type Parenthesis =
    | Open of Id
    | Closed of Id

/// <summary>
/// Type represent parser result.
/// </summary>
type ParsResult =
    | Success
    | Error of string

type Either<'a> =
    | Left of string
    | Right of 'a

module Parenthesis =
    /// <summary>
    /// Is left predicate.
    /// </summary>
    let isLeft =
        function
        | Left _ -> true
        | Right _ -> false

    /// <summary>
    /// Map for choose left.
    /// </summary>
    let chooseLeft =
        function
        | Left result -> Some result
        | Right _ -> None

    /// <summary>
    /// Map for choose right.
    /// </summary>
    let chooseRight =
        function
        | Left _ -> None
        | Right result -> Some result

    /// <summary>
    /// Try find ambiguous in parenthesis
    /// </summary>
    /// <param name="pairs">Pairs of opened and closed parenthesis correspondingly.</param>
    let tryFindAmbiguous pairs =
        let opened = List.map fst pairs
        let closed = List.map snd pairs

        opened @ closed |> List.countBy id |> List.tryFind (snd >> ((<>) 1))

    /// <summary>
    /// Create lookup table parsing.
    /// </summary>
    let createMap list =
        let toOpened = List.map <| fun (id, char) -> char, Open id
        let toClosed = List.map <| fun (id, char) -> char, Closed id

        List.mapi (fun index pair -> (index, fst pair), (index, snd pair)) list
        |> List.unzip
        |> fun (opened, closed) -> toOpened opened, toClosed closed
        ||> (@)

    /// <summary>
    /// Map char to parenthesis.
    /// </summary>
    let charToParenthesis map char =
        List.tryFind (fst >> ((=) char)) map
        |> function
            | Some(_, parenthesis) -> Right parenthesis
            | None -> Left $"Unknown character: {char}"

    /// <summary>
    /// Filter parenthesis.
    /// </summary>
    /// <param name="list"></param>
    let filter list =
        if List.exists isLeft list then
            List.choose chooseLeft list
            |> List.reduce (fun left right -> $"{left}\n{right}")
            |> Left
        else
            Right <| List.choose chooseRight list

    /// <summary>
    /// Check sequence for parenthesis consistently.
    /// </summary>
    let check parenthesis pairs =
        let idToChar id =
            snd <| List.item id pairs

        let rec parse =
            function
            | [], [] -> Success
            | Closed pairId :: remainParenthesis, expectedId :: remainExpectedId ->
                if expectedId = pairId then
                    parse (remainParenthesis, remainExpectedId)
                else
                    Error
                        $"Expected: %A{idToChar expectedId}, actual: %A{idToChar pairId}"
            | Open pairId :: remainParenthesis, expectedChars ->
                parse (remainParenthesis, pairId :: expectedChars)
            | [], _
            | _, [] -> Error "Incorrect number of brackets"

        parse (parenthesis, [])

    /// <summary>
    /// Parse string.
    /// </summary>
    let parse string pairs =
        match tryFindAmbiguous pairs with
        | Some(char, count) -> Error $"Ambiguity, the symbol {char} occurs {count} times"
        | None ->
            let chars = Seq.toList string
            let map = createMap pairs

            List.map (charToParenthesis map) chars
            |> filter
            |> function
                | Left errors -> Error errors
                | Right parenthesis -> check parenthesis pairs
