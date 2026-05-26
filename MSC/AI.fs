namespace MSC
open System
open MSC

module AI =

  let rand = Random()

  /// Generate all valid enemy moves
  let allMoves (board: Board) =
    [6..10]
    |> List.filter (fun i -> not (board.IsEmpty i))
    |> List.collect (fun i ->
        [
          { Index = i; Dir = Left;  Player = Computer }
          { Index = i; Dir = Right; Player = Computer }
        ]
    )

  /// EASY AI
  /// Random move
  let randomMove (board: Board) =
    let moves = allMoves board
    moves.[rand.Next(moves.Length)]
  /// Evaluate immediate score gain
  let evaluateMove (board: Board) (move: Move) =
    let tempBoard = board.Copy()
    Rules.applyMove tempBoard move

  /// HARD AI
  /// Choose move with highest immediate gain
  let bestMove (board: Board) =
    let moves = allMoves board
    moves
    |> List.maxBy (fun move ->
        evaluateMove board move
    )