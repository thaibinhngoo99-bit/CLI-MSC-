open System
open Game

[<EntryPoint>]
let main _ =
    printfn "=== CLI Tic-Tac-Toe ==="
    printfn "You are O. Enemy is X. You go first."

    let rec playLoop () =
        let _ = run ()
        printf "\nPlay again? (y/n): "
        match Console.ReadLine().Trim().ToLower() with
        | "y" -> playLoop ()
        | _ -> ()

    playLoop ()
    0