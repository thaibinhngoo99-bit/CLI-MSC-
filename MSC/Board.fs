namespace CS220
open CS220

type Player =
  | Player
  | Computer

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
    | Left -> (index + 11) % 12   // move left
    | Right -> (index + 1) % 12   // move right

  /// Pretty print board (matches README)
  member __.Print () =
    printfn ""
    printfn "        [%2d] [%2d] [%2d] [%2d] [%2d]"
      pits.[10] pits.[9] pits.[8] pits.[7] pits.[6]

    printfn " [%2d]                       [%2d]"
      pits.[11] pits.[5]

    printfn "        [%2d] [%2d] [%2d] [%2d] [%2d]"
      pits.[0] pits.[1] pits.[2] pits.[3] pits.[4]

    printfn ""