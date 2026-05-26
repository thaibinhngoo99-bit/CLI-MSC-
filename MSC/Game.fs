namespace MSC

open System
open MSC
open System.Threading

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

  let getEnemyMove difficulty =
    let move =
      match difficulty with
      | Easy -> AI.randomMove board
      | Hard -> AI.bestMove board
    printfn "Enemy chooses tile %d, direction %A"
      (11 - move.Index)
      move.Dir
    (move.Index, move.Dir)

  let applyMove (index, dir) player =
    let move   = { Index = index; Dir = dir; Player = player }
    let gained = Rules.applyMove board move
    if board.Pits.[5] = 0 && not rightSideCaptured then
      rightSideCaptured <- true
      Console.ForegroundColor <- ConsoleColor.Magenta
      printfn ""
      printfn "#############################################"
      printfn " RIGHT SIDE TILE CAPTURED FOR FIRST TIME!"
      printfn "#############################################"
      printfn ""
      Console.ResetColor()
    if board.Pits.[11] = 0 && not leftSideCaptured then
      leftSideCaptured <- true
      Console.ForegroundColor <- ConsoleColor.Magenta
      printfn ""
      printfn "#############################################"
      printfn " LEFT SIDE TILE CAPTURED FOR FIRST TIME!"
      printfn "#############################################"
      printfn ""
      Console.ResetColor()

    match player with
    | Player   -> playerScore <- playerScore + gained
    | Computer -> enemyScore  <- enemyScore  + gained

  let isGameOver () =
    leftSideCaptured && rightSideCaptured

  let printScores () =
    Console.ForegroundColor <- ConsoleColor.Cyan
    printfn "================================="
    printfn "   YOUR SCORE : %d" playerScore
    printfn "   ENEMY SCORE: %d" enemyScore
    printfn "================================="
    Console.ResetColor()
    printfn ""

  member __.Run () =
    Console.ForegroundColor <- ConsoleColor.Cyan
    printfn "============================================="
    printfn "        CLI MANDARIN SQUARE CAPTURE"
    printfn "============================================="
    printfn ""
    Console.ForegroundColor <- ConsoleColor.Yellow
    printfn "Capture more stones than the enemy!"
    printfn "The game ends once BOTH side tiles"
    printfn "have been captured at least once."
    printfn ""
    Console.ResetColor()

    printfn ""
    printfn "Difficulty:"
    printfn "1. Easy (For beginners, random moves)"
    printfn "2. Hard (For strategists, uses Minimax)"

    printf "Select difficulty (E/H): "

    let difficultyInput =
      Console.ReadLine()
      |> Option.ofObj
      |> Option.defaultValue ""

    let difficulty =
      match difficultyInput with
      | "E" -> Easy
      | "H" -> Hard

    printf "Do you want to go first? (Y/N): "
    let input =
      Console.ReadLine()
      |> Option.ofObj
      |> Option.defaultValue ""
    let playerFirst = input.ToUpper() = "Y"
    let rec loop currentPlayer =
      board.Print()
      printScores()
      Thread.Sleep(1500)

      if isGameOver() then
        Console.ForegroundColor <- ConsoleColor.Yellow
        printfn ""
        printfn "============================================="
        printfn "                 GAME OVER"
        printfn "============================================="
        printfn ""
        Console.ForegroundColor <- ConsoleColor.Green
        printfn "Final Score:"
        printfn "You   : %d" playerScore
        printfn "Enemy : %d" enemyScore
        printfn ""
        if playerScore > enemyScore then
          Console.ForegroundColor <- ConsoleColor.Cyan
          printfn ">>> YOU WIN! <<<"
        elif playerScore < enemyScore then
          Console.ForegroundColor <- ConsoleColor.Red
          printfn ">>> ENEMY WINS! <<<"
        else
          Console.ForegroundColor <- ConsoleColor.Magenta
          printfn ">>> DRAW! <<<"
        Console.ResetColor()
        printfn ""

        printf "Play again? (Y/N): "
        let again =
          Console.ReadLine()
          |> Option.ofObj
          |> Option.defaultValue ""
        if again.ToUpper() = "Y" then
          let newGame = Game()
          newGame.Run()

      else
        let next =
          match currentPlayer with
          | Player   -> Computer
          | Computer -> Player
        match currentPlayer with
        | Player ->
            Console.ForegroundColor <- ConsoleColor.Green
            printfn ""
            printfn "============== YOUR TURN =============="
            Console.ResetColor()
            Thread.Sleep(1000)
            if isSideEmpty Player then
              refill Player
            else
              let move = getUserMove()
              Console.ForegroundColor <- ConsoleColor.Cyan
              printfn ""
              printfn "Executing your move..."
              Console.ResetColor()
              Thread.Sleep(1500)
              applyMove move Player

        | Computer ->
            Console.ForegroundColor <- ConsoleColor.Red
            printfn ""
            printfn "============== ENEMY TURN =============="
            printfn "Enemy is thinking..."
            Console.ResetColor()
            Thread.Sleep(2000)
            if isSideEmpty Computer then
              refill Computer
            else
              applyMove (getEnemyMove difficulty) Computer
        loop next

    if playerFirst then
      loop Player
    else
      loop Computer