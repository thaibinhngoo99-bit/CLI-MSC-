module Board

type Cell = Empty of int | O | X

let create () : Cell array =
    Array.init 9 (fun i -> Empty (i + 1))

let cellStr = function
    | Empty n -> "0"
    | O -> "O"
    | X -> "X"

let render (board: Cell array) =
    let s i = cellStr board.[i]
    printfn " %s | %s | %s" (s 0) (s 1) (s 2)
    printfn "---+---+---"
    printfn " %s | %s | %s" (s 3) (s 4) (s 5)
    printfn "---+---+---"
    printfn " %s | %s | %s" (s 6) (s 7) (s 8)

let private winLines = [|
    [|0;1;2|]; [|3;4;5|]; [|6;7;8|]
    [|0;3;6|]; [|1;4;7|]; [|2;5;8|]
    [|0;4;8|]; [|2;4;6|]
|]

let winner (board: Cell array) =
    winLines
    |> Array.tryPick (fun line ->
        match board.[line.[0]], board.[line.[1]], board.[line.[2]] with
        | O, O, O -> Some O
        | X, X, X -> Some X
        | _ -> None)

let isFull (board: Cell array) =
    board |> Array.forall (function Empty _ -> false | _ -> true)

/// Returns Some newBoard if the square was empty, None if occupied.
let tryPlace (board: Cell array) (sq: int) (cell: Cell) : Cell array option =
    if sq < 1 || sq > 9 then None
    else
        match board.[sq - 1] with
        | Empty _ ->
            let b = Array.copy board
            b.[sq - 1] <- cell
            Some b
        | _ -> None