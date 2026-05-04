namespace CS220

type Direction =
  | Left
  | Right

type Move =
  { Index: int        // 0–11 (internal board index)
    Dir: Direction }