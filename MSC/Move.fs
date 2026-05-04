namespace CS220

type Direction =
  | Left
  | Right

type Move =
  { Pit: int      // which pit (1–5 on your side)
    Dir: Direction }