module Game

open System
open Board

type GameResult = PlayerWins | EnemyWins | Draw

let private rng = Random()

let private enemyMove (board: Cell array) : int =
    let empties =
        board
        |> Array.mapi (fun i c -> i, c)
        |> Array.choose (fun (i, c) ->
            match c with Empty _ -> Some (i + 1) | _ -> None)
    empties.[rng.Next(empties.Length)]

let private getUserInput (board: Cell array) : int =
    let rec loop () =
        printf "Your move (1-9): "
        match Int32.TryParse(Console.ReadLine()) with
        | true, n when n >= 1 && n <= 9 ->
            match board.[n - 1] with
            | Empty _ -> n
            | _ ->
                printfn "Square %d is already taken. Please try again." n
                loop ()
        | _ ->
            printfn "Invalid input. Please enter a number from 1 to 9."
            loop ()
    loop ()

let run () : GameResult =
    let rec loop board =
        printfn ""
        render board

        // Player's turn (O)
        let sq = getUserInput board
        let board1 =
            match tryPlace board sq O with
            | Some b -> b
            | None -> board // validated above

        match winner board1 with
        | Some O ->
            printfn ""
            render board1
            printfn "You win!"
            PlayerWins
        | _ when isFull board1 ->
            printfn ""
            render board1
            printfn "It's a tie!"
            Draw
        | _ ->
            // Enemy's turn (X)
            let esq = enemyMove board1
            printfn "Enemy places X on square %d." esq
            let board2 =
                match tryPlace board1 esq X with
                | Some b -> b
                | None -> board1 // shouldn't happen

            match winner board2 with
            | Some X ->
                printfn ""
                render board2
                printfn "Enemy wins!"
                EnemyWins
            | _ when isFull board2 ->
                printfn ""
                render board2
                printfn "It's a tie!"
                Draw
            | _ -> loop board2

    loop (create ())