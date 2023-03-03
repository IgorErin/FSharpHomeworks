namespace Introduction

module EvenNumbers =
    let isEven number = number % 2 = 0

    let countMapFold =
        List.map (fun item -> if isEven item then 1 else 0) >> List.fold (+) 0

    let countFilterFold = List.filter isEven >> List.length

    let countFilterMapFold =
        List.filter isEven >> List.map (fun _ -> 1) >> List.fold (+) 0
