namespace MSC

open MSC

module Main =

  [<EntryPoint>]
  let main _ =
    let game = Game()
    game.Run()
    0