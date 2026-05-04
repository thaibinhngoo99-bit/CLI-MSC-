namespace CS220

open System
open CS220

type Game () =
  let board = Board()
  let rand = Random()

  let mutable playerScore = 0
  let mutable enemyScore = 0

  /// Check if all pits on a side are empty
  let isSideEmpty player =
    match player with
    | Player ->
        [0..4] |> List.forall (fun i -> board.IsEmpty i)
    | Computer ->
        [6..10] |> List.forall (fun i -> board.IsEmpty i)

  /// Refill rule (simplified)
  let refill player =
    match player with
    | Player ->
        if playerScore > 0 then
          for i in 0..4 do
            if board.IsEmpty i && playerScore > 0 then
              board.AddStone i
              playerScore <- playerScore - 1
        else
          printfn "You have no stones to refill. Turn skipped."

    | Computer ->
        if enemyScore > 0 then
          for i in 6..10 do
            if board.IsEmpty i && enemyScore > 0 then
              board.AddStone i
              enemyScore <- enemyScore - 1

  /// Get valid moves for a player
  let validMoves player =
    match player with
    | Player -> [0..4] |> List.filter (fun i -> not (board.IsEmpty i))
    | Computer -> [6..10] |> List.filter (fun i -> not (board.IsEmpty i))

  /// Read user move
  let rec getUserMove () =
    printf "Select your tile (1-5): "
    let pitInput = Console.ReadLine()

    match Int32.TryParse pitInput with
    | true, pit when pit >= 1 && pit <= 5 ->
        let index = pit - 1
        if board.IsEmpty index then
          printfn "Tile is empty. Try again."
          getUserMove ()
        else
          printf "Direction (L/R): "
          match Console.ReadLine().ToUpper() with
          | "L" -> (index, Left)
          | "R" -> (index, Right)
          | _ ->
              printfn "Invalid direction."
              getUserMove ()
    | _ ->
        printfn "Invalid input."
        getUserMove ()

  /// Random AI move
  let getEnemyMove () =
    let moves = validMoves Computer
    let index = moves.[rand.Next(moves.Length)]
    let dir = if rand.Next(2) = 0 then Left else Right
    printfn "Enemy chooses tile %d, direction %A" (index - 5) dir
    (index, dir)

  /// Execute move (you will implement logic in Rules)
  let applyMove (index, dir) player =
    let stones = board.TakeStones index
    let mutable current = index

    // simple distribution (no capture yet)
    for _ in 1..stones do
      current <- board.NextIndex current dir
      board.AddStone current

    // TODO: capture logic (later)

  /// Check game end condition
  let isGameOver () =
    board.Pits.[5] = 0 && board.Pits.[11] = 0

  /// Print scores
  let printScores () =
    printfn "Your score = %d, Enemy score = %d" playerScore enemyScore

  member __.Run () =
    printf "Do you want to go first? (Y/N): "
    let playerFirst = Console.ReadLine().ToUpper() = "Y"

    let rec loop playerTurn =
      board.Print()
      printScores()

      if isGameOver () then
        printfn "Game Over!"
        if playerScore > enemyScore then printfn "You win!"
        elif playerScore < enemyScore then printfn "Enemy wins!"
        else printfn "Draw!"
      else
        match playerTurn with
        | Player ->
            if isSideEmpty Player then refill Player
            else
              let move = getUserMove ()
              applyMove move Player
            loop Computer

        | Computer ->
            if isSideEmpty Computer then refill Computer
            else
              let move = getEnemyMove ()
              applyMove move Computer
            loop Player

    if playerFirst then loop Player
    else loop Computer