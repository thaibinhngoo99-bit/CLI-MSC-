# CLI Mandarin Square Capture (Ô ăn quan)

A command-line implementation of Mandarin Square Capture, built with **F# / .NET 10**.

Play against a simple AI opponent in this classic Vietnamese strategy game of distribution and capture.

---
## What is this game?

Mandarin Square Capture (Ô ăn quan) is a Vietnamese traditional two-player board game.

Each turn, you:
- Pick a square on your side.
- Distribute its stones across the board.
- Capture stones based on where your moves end.

Goal: Collect more stones than your opponent by the end of the game. 

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

### Game Board

The board consists of:
- 10 small tiles labeled 1 to 10 (5 per player)
- 2 large tiles A, B at both ends (Mandarin tiles)

```
|   | 10 | 9 | 8 | 7 | 6 |   |
| B +---+---+---+---+---+ A |
|   | 1 | 2 | 3 | 4 | 5 |   |
```

- You control tiles 1 to 5 (bottom row)
- The enemy controls tiles 6 to 10 (top row)
- Each tile displays the number of stones it contains

### Game starts

1. The board is printed, each tiles contains 5 stones.
2. Your score and enemy's initial scores are set to 0
3. You are prompted: 'Would you like to go first?'
4. Type Y or N and press **Enter**
   - If the input is not Y or N, you are asked to try again.
   - If the input is Y, you start the game.
   - If the input is N, the enemy starts the game.

### Taking a Turn

1. If all tiles from 1-5 are empty, you follow the refill rule (see Refill Rule for more details)
2. If there exists at least 1 non-empty tile from 1-5, you are prompted: `Select your tile (1-5):`
3. Type a number and press **Enter**.
   - If the input is not a number in 1–5, you are asked to try again.
   - If the selected tile has no stones, you are asked to try again.
4. You are then prompted: `Select the direction (L-R):`
5. Type L or R and press **Enter**.
   - If the input is not L or R, you are asked to try again.
6. Your move follows the move mechanics (see Move Mechanics for more details)

### Move Mechanics

After a player chooses a tile and direction: 
1. All stones in the chosen tile are picked up and distributed one-by-one into adjacent tiles in the chosen direction. 
2. Once the last stone is placed in a tile, consider its adjacent tile in the chosen direction:
- Case 1: If it has stones:
    - If it is a Mandarin tile, you finish your turn.
    - If it is not a Mandarin tile, you pick up the stones in that tile and continue distributing them in the same direction.
- Case 2: If it is empty, consider the adjacent tile to it in the same direction:
    - If it is also empty, you finish your turn.
    - If it has stones, you capture the stones in the tile and your score increases by the amount of stones it has. The captured tile updates the number of stones it has to 0. 
    - Captures can chain multiple times (rules of capturing follow step 2 of the move mechanics by considering the adjacent tile to the captured tile). Once the chain breaks, you finish your turn.

### Refill Rule

If all tiles on your side is empty and the game hasn't ended, you are prompted: `You must refill your tiles!`
- If your score > 0, 1 point of your score is spent per tile to refill (the order is from 1 to 5) until all 5 tiles are filled or your score reaches 0. You can then resume your turn.
- If your score = 0, you are prompted: `Sorry, you lost your turn!` and lost your turn.

### Enemy Turn

After your move, the enemy follows the same refill rule, selects a random valid tile and direction and performs the same move logic as yours automatically. The tile and direction chosen is displayed for you to follow

### Winning & Ending

The game ends when both Mandarin tiles are captured at least once

| Result | Condition |
|--------|-----------|
| **You win** | You have a higher score |
| **Enemy wins** | Enemy has a higher score |
| **Tie** | Your score ties with the enemy's |

After the game ends, you are asked whether to play again.

---

## Example Session

```
=== CLI MSC ===
You go first.
|   | 5 | 5 | 5 | 5 | 5 |   |
| 5 +---+---+---+---+---+ 5 |
|   | 5 | 5 | 5 | 5 | 5 |   |
Your score = 0, enemy score = 0.

Your turn:
Your selected tile (1-5): 1
Selected direction (L-R): R
Board updates:
|   | 6 | 6 | 6 | 6 | 0 |   |
| 6 +---+---+---+---+---+ 6 |
|   | 0 | 0 | 6 | 6 | 6 |   |
Your score = 6, enemy score = 0

Enemy's turn:
Enemy's selected tile (1-5): 8
Selected direction (L-R): L
Board updates:
|   | 6 | 6 | 0 | 7 | 1 |   |
| 6 +---+---+---+---+---+ 7 |
|   | 0 | 0 | 7 | 7 | 7 |   |
Your score = 6, enemy score = 0

Your turn:
...
```

---

## Project Structure

```
CLI-MSC-/
├── MSC.fsproj    # .NET 10 F# project file
├── run.bat             # Windows run script
├── run.sh              # Unix run script
├── README.md
├── requirements.md
└── MSC/
    ├── Board.fs        # Cell type, board state, rendering, win detection
    ├── Game.fs         # Game loop, player input validation, enemy AI
    └── Program.fs      # Entry point, play-again loop
```

### Key Types

```fsharp
// A tile only displays the number of stones it has, not its value. Values for each tiles are displayed at the beginning of the game.

// Possible outcomes of a completed game
type GameResult = PlayerWins | EnemyWins | Draw
```

### Module Overview

| Module | Responsibility |
|--------|---------------|
| `Board` | Immutable board operations: `create`, `tryPlace`, `winner`, `isFull`, `render` |
| `Game`  | Turn handling, input validation, random AI behaviour, end-state display |
| `Program` | Entry point; prints welcome message, replay loop |

---

## Author Notes

- Invalid inputs do not consume your turn.
- Enemy's direction input is considered the same as your input (enemy's left is also your left).
- The game is deterministic except for enemy randomness.
- Designed for clarity and learning purposes.

## Future improvements

- Smarter AI (strategy instead of random)
- Better UI visualization
- Configurable scoring rules
- Multiplayer mode
