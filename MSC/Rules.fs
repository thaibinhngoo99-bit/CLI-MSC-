namespace MSC
open MSC

module Rules =

  let applyMove (board: Board) (move: Move) =

    let mutable score = 0
    let mutable current = move.Index
    let mutable stones = board.TakeStones current

    // ==========================================
    // PHASE 1: INITIAL SOW + CONTINUED SOWING
    // ==========================================
    let mutable sowing = true

    while sowing do

      // sow stones
      while stones > 0 do
        current <- board.NextIndex current move.Dir
        board.AddStone current
        stones <- stones - 1

      let next = board.NextIndex current move.Dir

      // ------------------------------------------
      // NEW RULE:
      // if next pit is a side tile with stones,
      // immediately stop turn
      // ------------------------------------------
      let isSideTile =
        next = 5 || next = 11

      if isSideTile && not (board.IsEmpty next) then
        sowing <- false

      // landing -> stones
      elif not (board.IsEmpty next) then
        stones <- board.TakeStones next
        current <- next

      // landing -> empty
      else
        sowing <- false

    // ==========================================
    // PHASE 2: CAPTURE CHAIN
    // ==========================================
    let mutable capturing = true

    while capturing do

      let emptyPit = board.NextIndex current move.Dir

      // must begin with empty
      if board.IsEmpty emptyPit then

        let stonePit = board.NextIndex emptyPit move.Dir

        // empty -> stones
        if not (board.IsEmpty stonePit) then

          let captured = board.TakeStones stonePit
          score <- score + captured

          // continue from captured pit
          current <- stonePit

        else
          // empty -> empty
          capturing <- false

      else
        // stones -> stones
        capturing <- false

    score