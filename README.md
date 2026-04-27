# CLI MSC

A command-line Mandarin Square Capture game built with **F# / .NET 10**.

You play against a random enemy **X**. Enter a square number (1–5) and a dỉrection (Clockwise-Counter-clockwise) to make your move.

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

Empty squares display their number. Placed marks display `O` or `X`.

### Taking a Turn

1. The current board is printed.
2. You are prompted: `Your move (1-9):`
3. Type a number and press **Enter**.
   - If the input is not a number in 1–9, you are asked to try again.
   - If the selected square is already occupied, you are asked to try again.
4. `O` is placed on your chosen square.

### Enemy Turn

After your move, the enemy automatically picks a random empty square and places `X`.
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
