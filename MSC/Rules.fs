namespace CS220

module Rules =

  /// Apply a move and return score gained
  let applyMove (board: Board) (move: Move) =
    let mutable current = move.Index
    let mutable stones = board.TakeStones current
    let mutable score = 0

    // STEP 1: distribute stones
    while stones > 0 do
      current <- board.NextIndex current move.Dir
      board.AddStone current
      stones <- stones - 1

    // STEP 2: chaining + capture
    let mutable continueTurn = true

    while continueTurn do
      let next = board.NextIndex current move.Dir

      if board.IsEmpty next then
        let afterNext = board.NextIndex next move.Dir

        if board.IsEmpty afterNext then
          // Case: empty → empty → stop
          continueTurn <- false
        else
          // Case: empty → stones → capture
          let captured = board.TakeStones afterNext
          score <- score + captured
          current <- afterNext
      else
        // Case: next has stones → continue distributing
        stones <- board.TakeStones next
        current <- next

        while stones > 0 do
          current <- board.NextIndex current move.Dir
          board.AddStone current
          stones <- stones - 1

    score


  /// Game ends when both edge tiles are empty
  let isGameOver (board: Board) =
    board.Pits.[5] = 0 && board.Pits.[11] = 0