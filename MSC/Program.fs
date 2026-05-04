namespace CS220

module Program =

  [<EntryPoint>]
  let main _ =
    printfn "=== CLI Mandarin Square Capture ==="

    let game = Game()
    game.Run()

    0