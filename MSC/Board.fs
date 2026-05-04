namespace CS220

type Player =
  | Player
  | Computer

type Board () =
  // 12 pits:
  // 1–5: player side
  // 6: right quan
  // 7–11: computer side
  // 12: left quan
  let pits = Array.zeroCreate 12

  do
    // Initialize small pits
    for i in 1..5 do pits.[i] <- 5
    for i in 7..11 do pits.[i] <- 5

    // Initialize quan pits
    pits.[6] <- 1
    pits.[12] <- 1

  member __.Pits = pits

  // Copy board (VERY IMPORTANT for AI)
  member __.Copy () =
    let b = Board()
    Array.iteri (fun i v -> b.Pits.[i] <- v) pits
    b

  // Check if a pit belongs to a player
  member __.IsOwnPit player index =
    match player with
    | Player -> index >= 0 && index <= 4
    | Computer -> index >= 6 && index <= 10

  // Check if pit is empty
  member __.IsEmpty index =
    pits.[index] = 0

  // Take all stones from a pit
  member __.TakeStones index =
    let stones = pits.[index]
    pits.[index] <- 0
    stones

  // Add one stone to a pit
  member __.AddStone index =
    pits.[index] <- pits.[index] + 1

  // Pretty print board (important for CLI)
  member __.Print () =
    printfn ""
    printfn "        [%2d] [%2d] [%2d] [%2d] [%2d]"
      pits.[10] pits.[9] pits.[8] pits.[7] pits.[6]

    printfn " [%2d]                       [%2d]"
      pits.[11] pits.[5]

    printfn "        [%2d] [%2d] [%2d] [%2d] [%2d]"
      pits.[0] pits.[1] pits.[2] pits.[3] pits.[4]

    printfn ""