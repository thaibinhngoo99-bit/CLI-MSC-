# CLI MSC

A command-line Mandarin Square Capture game built with **F# / .NET 10**.

You play against a random enemy **X**. Enter a square number (1–5) and a dỉrection (left, right) to make your move.

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)  
  Verify with: `dotnet --version` (should show `10.x.x`)

### Run

```bash
# Windows
run.bat

# Unix / macOS
chmod +x run.sh
./run.sh

# Or directly
dotnet run
```

### Build

```bash
dotnet build
```

### Publish Self-Contained Binary

```bash
# Windows x64
dotnet publish -c Release -r win-x64 --self-contained

# Linux x64
dotnet publish -c Release -r linux-x64 --self-contained
```

---

## How to Play

### Board Layout

10 unit squares of the 2*5 middle rectangle are numbered 1–10 counter-clockwise from bottom-left to top-left. 2 big rectangles on the sides are labeled A and B

```
|   | 10 | 9 | 8 | 7 | 6 |   |
| A +---+---+---+---+---+ B |
|   | 1 | 2 | 3 | 4 | 5 |   |
```

Each squares display the number of stones currently in it. Empty squares display 0.

### Game starts

1. The board is printed, each
2. You are prompted: 'Would you like to go first?'
3. Type Y or N and press **Enter**
   - If the input is not Y or N, you are asked to try again.
   - If the input is Y, you start taking your turn.
   - If the input is N, the enemy starts taking their turn.

### Distribution rule:
Given a tile number and a 

### Taking a Turn

1. If all tiles from 1-5 is empty, you are prompted: `You don't have any pebbles to use!`
   - If your score > 0, the system automatically deducts 1 point from your score and put 1 pebble into each tile from 1-5 respectively until your score is 0 or all 5 tiles have 1 pebble each. Your turn then continues as normal.
   - If your score = 0, you are prompted: `Sorry, you lost your turn!` and lost your turn. The enemy will start taking their turn
2. If there exists at least 1 non-empty tile from 1-5, you are prompted: `Select your tile (1-5):`
3. Type a number and press **Enter**.
   - If the input is not a number in 1–5, you are asked to try again.
   - If the selected tile has no pebbles, you are asked to try again.
   - The s
4. You are then prompted: `Select the direction (L-R):`
5. Type L or R and press **Enter**.
   - If the input is not L or R, you are asked to try again.
   - If the selected tile has no pebbles

### Enemy Turn

After your move, the enemy automatically picks a random tile numbered from 6-10 that contains at least 1 pebble and places `X`.
The chosen square number is printed so you can follow along.

### Winning & Ending

| Result | Condition |
|--------|-----------|
| **You win** | Three `O`s in a row, column, or diagonal |
| **Enemy wins** | Three `X`s in a row, column, or diagonal |
| **Tie** | All 9 squares filled with no winner |

After the game ends, you are asked whether to play again.

---

## Example Session

```
=== CLI Tic-Tac-Toe ===
You are O. Enemy is X. You go first.

 1 | 2 | 3
---+---+---
 4 | 5 | 6
---+---+---
 7 | 8 | 9

Your move (1-9): 5
Enemy places X on square 3.

 1 | 2 | X
---+---+---
 4 | O | 6
---+---+---
 7 | 8 | 9

Your move (1-9): 1
...
```

---

## Project Structure

```
project-example/
├── TicTacToe.fsproj    # .NET 10 F# project file
├── run.bat             # Windows run script
├── run.sh              # Unix run script
├── README.md
├── requirements.md
└── TicTacToe/
    ├── Board.fs        # Cell type, board state, rendering, win detection
    ├── Game.fs         # Game loop, player input validation, enemy AI
    └── Program.fs      # Entry point, play-again loop
```

### Key Types

```fsharp
// A cell is either an empty square (showing its number) or a placed mark
type Cell = Empty of int | O | X

// Possible outcomes of a completed game
type GameResult = PlayerWins | EnemyWins | Draw
```

### Module Overview

| Module | Responsibility |
|--------|---------------|
| `Board` | Immutable board operations: `create`, `tryPlace`, `winner`, `isFull`, `render` |
| `Game`  | Main game loop, input validation, random enemy move, end-state display |
| `Program` | Entry point; prints welcome message, drives play-again loop |

---

## Rules Summary

- You are always **O** and always go first.
- The enemy is always **X** and always moves randomly.
- Selecting an occupied or invalid square re-prompts — the turn does **not** advance.
- Three marks of the same kind in any row, column, or diagonal wins the game.
- A full board with no winner is a tie.
