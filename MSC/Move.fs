namespace MSC

type Direction =
  | Left
  | Right

type Player =
  | Player
  | Computer

type Move = {
  Index  : int
  Dir    : Direction
  Player : Player
}