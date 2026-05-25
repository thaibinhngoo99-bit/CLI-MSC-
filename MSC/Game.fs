namespace MSC

open System
open MSC

type Game () =
  let board = Board()
  let rand = Random()

  let mutable playerScore = 0
  let mutable enemyScore  = 0
  let mutable leftSideCaptured  = false
  let mutable rightSideCaptured = false

  let isSideEmpty player =
    match player with
    | Player   -> [0..4]  |> List.forall (fun i -> board.IsEmpty i)
    | Computer -> [6..10] |> List.forall (fun i -> board.IsEmpty i)

  let refill player =
    let pits, score =
      match player with
      | Player   -> [0..4],  playerScore
      | Computer -> [6..10], enemyScore
    if score = 0 then
      printfn "%A has no stones to refill. Turn skipped." player
    else
      let mutable remaining = score
      for i in pits do
        if remaining > 0 then
          board.AddStone i
          remaining <- remaining - 1
      match player with
      | Player   -> playerScore <- remaining
      | Computer -> enemyScore  <- remaining

  let validMoves player =
    match player with
    | Player   -> [0..4]  |> List.filter (fun i -> not (board.IsEmpty i))
    | Computer -> [6..10] |> List.filter (fun i -> not (board.IsEmpty i))

  let rec getUserMove () =
    printf "Select your tile (1-5): "
    let pitInput = Console.ReadLine() |> Option.ofObj |> Option.defaultValue ""
    match Int32.TryParse(pitInput) with
    | true, pit when pit >= 1 && pit <= 5 ->
        let index = pit - 1
        if board.IsEmpty index then
          printfn "Tile is empty. Try again."
          getUserMove()
        else
          printf "Direction (L/R): "
          let dirInput = Console.ReadLine() |> Option.ofObj |> Option.defaultValue ""
          match dirInput.ToUpper() with
          | "L" -> (index, Left)
          | "R" -> (index, Right)
          | _   -> printfn "Invalid direction."; getUserMove()
    | _ ->
        printfn "Invalid input."
        getUserMove()

  let getEnemyMove () =
    let moves = validMoves Computer
    let index  = moves.[rand.Next(moves.Length)]
    let dir    = if rand.Next(2) = 0 then Left else Right
    printfn "Enemy chooses tile %d, direction %A" (11 - index) dir
    (index, dir)

  let applyMove (index, dir) player =
    let move   = { Index = index; Dir = dir; Player = player }
    let gained = Rules.applyMove board move
    if board.Pits.[5] = 0 && not rightSideCaptured then
      rightSideCaptured <- true
      printfn ">>> The RIGHT side tile has been captured for the first time!"
    if board.Pits.[11] = 0 && not leftSideCaptured then
      leftSideCaptured <- true
      printfn ">>> The LEFT side tile has been captured for the first time!"
    match player with
    | Player   -> playerScore <- playerScore + gained
    | Computer -> enemyScore  <- enemyScore  + gained

  let isGameOver () =
    leftSideCaptured && rightSideCaptured

  let printScores () =
    printfn "Your score = %d, Enemy score = %d" playerScore enemyScore

  member __.Run () =
    printf "Do you want to go first? (Y/N): "
    let input = Console.ReadLine() |> Option.ofObj |> Option.defaultValue ""
    let playerFirst = input.ToUpper() = "Y"

    let rec loop currentPlayer =
      board.Print()
      printScores()

      if isGameOver() then
        printfn "Game Over!"
        if   playerScore > enemyScore then printfn "You win!"
        elif playerScore < enemyScore then printfn "Enemy wins!"
        else printfn "Draw!"
      else
        let next =
          match currentPlayer with
          | Player   -> Computer
          | Computer -> Player

        match currentPlayer with
        | Player ->
            if isSideEmpty Player then refill Player
            else applyMove (getUserMove()) Player

        | Computer ->
            if isSideEmpty Computer then refill Computer
            else applyMove (getEnemyMove()) Computer

        loop next

    if playerFirst then loop Player else loop Computer