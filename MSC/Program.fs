module CS220.Program

let start = function
  | RandomAIPlayerFirst ->
    MSC(O, X, true, RandomStrategy ()).Run()
    true
  | RandomAIComputerFirst ->
    MSC(O, X, false, RandomStrategy ()).Run()
    true
  | MinimaxAIPlayerFirst ->
    MSC(O, X, true, MinimaxStrategy ()).Run()
    true
  | MinimaxAIComputerFirst ->
    MSC(O, X, false, MinimaxStrategy ()).Run()
    true
  | Exit -> false

let rec gameLoop keepGoing =
  if keepGoing then GameOption.take () |> start |> gameLoop
  else 0

[<EntryPoint>]
let main _args =
  printfn "[CS220] Mandarin Square Capture"
  gameLoop true
