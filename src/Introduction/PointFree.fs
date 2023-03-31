module Introduction.PointFree

let sourceFunction x l =
    List.map (fun y -> y * x) l

/// <summary>
/// Eta reduction.
/// </summary>
let fun1 x =
    List.map (fun y -> y * x)

/// <summary>
/// Explicit lambda reduction.
/// </summary>
let fun2 x = List.map ((*) x)

let fun3 = List.map << (*)
