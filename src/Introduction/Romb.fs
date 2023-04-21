namespace Introduction

type Romb =
    | Star
    | WhiteSpace

module Romb =
    let createLine starCount lineLength =
        let whiteSpaceCount = (lineLength - starCount) / 2

        let rec add item count list =
            if count > 0 then
                add item (count - 1) <| item :: list
            else
                list

        add WhiteSpace whiteSpaceCount []
        |> add Star starCount
        |> add WhiteSpace whiteSpaceCount

    let create n =
        let lineLenght = 2 * n - 1

        let rec createLineUp starCount acc =
            if starCount <= lineLenght then
                let line = createLine starCount lineLenght
                createLineUp (starCount + 2) <| line :: acc
            else
                acc

        let rec createLineDown starCount acc =
            if starCount > 0 then
                let line = createLine starCount lineLenght
                createLineDown (starCount - 2) <| line :: acc
            else
                acc // duplication

        createLineUp 1 [] |> createLineDown (lineLenght - 2)

    let toString n =
        create n
        |> List.map (
            List.fold
                (fun state ->
                    function
                    | WhiteSpace -> $"%s{state} "
                    | Star -> $"%s{state}*"
                )
                ""
        )
        |> String.concat "\n"
