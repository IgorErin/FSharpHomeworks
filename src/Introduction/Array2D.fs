namespace Introduction

// Array2D extensions
module Array2D =
    // get rows
    let splitByRow array =
        let length1 = Array2D.length1 array
        let length2 = Array2D.length2 array

        Array.init
            (length1 * length2)
            (fun index ->
                let rowIndex = index / length1
                let columnIndex = index / length2

                rowIndex, array.[rowIndex, columnIndex]
            )
        |> Array.groupBy fst
        // get values
        |> Array.map (snd >> Array.map snd)

    let mapByRow mapping array2D array1D =
        splitByRow array2D |> Array.map (fun row -> Array.map2 mapping row array1D)
