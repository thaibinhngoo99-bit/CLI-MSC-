namespace MSC
open System
open MSC

type Board () =
  // Index layout (0-based):
  // Bottom row (Player): 0–4   → tiles 1–5
  // Right edge:         5      → tile A
  // Top row (Computer): 6–10   → tiles 6–10
  // Left edge:          11     → tile B

  let pits = Array.create 12 5   // ALL tiles start with 5 stones

  member __.Pits = pits

  /// Copy board (important for AI)
  member __.Copy () =
    let b = Board()
    Array.iteri (fun i v -> b.Pits.[i] <- v) pits
    b

  /// Check if a pit belongs to a player
  member __.IsOwnPit player index =
    match player with
    | Player -> index >= 0 && index <= 4
    | Computer -> index >= 6 && index <= 10

  /// Check if pit is empty
  member __.IsEmpty index =
    pits.[index] = 0

  /// Take all stones from a pit
  member __.TakeStones index =
    let stones = pits.[index]
    pits.[index] <- 0
    stones

  /// Add one stone to a pit
  member __.AddStone index =
    pits.[index] <- pits.[index] + 1

  /// Move to next index (circular)
   member __.NextIndex index direction =
    match direction with
    | Right ->
        match index with
        | 0  -> 1
        | 1  -> 2
        | 2  -> 3
        | 3  -> 4
        | 4  -> 5   // Player row → right edge
        | 5  -> 6   // Right edge → Computer row start
        | 6  -> 7
        | 7  -> 8
        | 8  -> 9
        | 9  -> 10
        | 10 -> 11  // Computer row end → left edge
        | 11 -> 0   // Left edge → Player row start
        | _  -> failwith "Invalid index"

    | Left ->
        match index with
        | 0  -> 11  // Player row start → left edge
        | 1  -> 0
        | 2  -> 1
        | 3  -> 2
        | 4  -> 3
        | 5  -> 4   // Right edge → Player row end
        | 6  -> 5   // Computer row start → right edge
        | 7  -> 6
        | 8  -> 7
        | 9  -> 8
        | 10 -> 9
        | 11 -> 10  // Left edge → Computer row end
        | _  -> failwith "Invalid index"
  
  member __.SetPits(values:int array) =
    Array.iteri (fun i v -> pits.[i] <- v) values

  member __.Serialize() =
    String.concat "," (pits |> Array.map string)
  /// Pretty print board (matches README)
  /// Cleaner board visualization
  /// Cleaner board visualization
  member __.Print () =

    printfn ""

    // Enemy side
    Console.ForegroundColor <- ConsoleColor.Red

    printfn "                ENEMY"
    printfn ""

    printfn "      ┌────┬────┬────┬────┬────┐"
    printfn "      │ %2d │ %2d │ %2d │ %2d │ %2d │"
      pits.[10] pits.[9] pits.[8] pits.[7] pits.[6]
    printfn "      └────┴────┴────┴────┴────┘"

    Console.ResetColor()

    // Side tiles
    Console.ForegroundColor <- ConsoleColor.Yellow

    printfn ""
    printfn "┌────┐                          ┌────┐"
    printfn "│ %2d │                          │ %2d │"
      pits.[11] pits.[5]
    printfn "└────┘                          └────┘"

    Console.ResetColor()

    // Player side
    Console.ForegroundColor <- ConsoleColor.Green

    printfn ""
    printfn "      ┌────┬────┬────┬────┬────┐"
    printfn "      │ %2d │ %2d │ %2d │ %2d │ %2d │"
      pits.[0] pits.[1] pits.[2] pits.[3] pits.[4]
    printfn "      └────┴────┴────┴────┴────┘"
    printfn ""
    printfn "                YOU"
    printfn ""
    Console.ResetColor()