# CLI Mandarin Square Capture (Ô ăn quan)

A command-line implementation of Mandarin Square Capture, built with **F# / .NET 10**.

Play against an AI opponent in this classic Vietnamese strategy game of distribution and capture.

---

## What is this game?

Mandarin Square Capture (Ô ăn quan) is a traditional Vietnamese two-player board game.

Each turn, a player:

* Picks a tile on their side
* Distributes its stones across the board
* Captures stones based on where the move ends

**Goal:** Collect more stones than your opponent by the end of the game.

> ⚠️ Note: This implementation uses a **simplified rule set** for clarity and ease of development.

---

## Getting Started

### Prerequisites

* [.NET 10 SDK](https://dotnet.microsoft.com/download)
  Verify with:

  ```bash
  dotnet --version
  ```

  (should show `10.x.x`)

---

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

---

### Build

```bash
dotnet build
```

---

### Publish Self-Contained Binary

```bash
# Windows x64
dotnet publish -c Release -r win-x64 --self-contained

# Linux x64
dotnet publish -c Release -r linux-x64 --self-contained
```

---

## Game Overview

### Board Layout

The board consists of **12 tiles**:

* 10 regular tiles (5 per player)
* 2 edge tiles (left and right ends)

All tiles start with **5 stones**.

```
|   | 10 | 9 | 8 | 7 | 6 |   |
| B +----+----+----+----+----+ A |
|   | 1  | 2  | 3  | 4  | 5  |   |
```

* You control tiles **1–5** (bottom row)
* The enemy controls tiles **6–10** (top row)
* Tiles **A** and **B** are edge tiles (treated like normal tiles in this version)

---

## How to Play

### Game Start

1. The board is displayed (each tile starts with 5 stones)
2. Scores are initialized:

   * Your score = 0
   * Enemy score = 0
3. You are prompted:

   ```
   Would you like to go first? (Y/N)
   ```
4. Enter:

   * `Y` → you go first
   * `N` → enemy goes first
   * Invalid input → retry

---

### Your Turn

1. If all your tiles (1–5) are empty:

   * Apply the **Refill Rule** (see below)

2. Otherwise:

   ```
   Select your tile (1–5):
   ```

   * Must be a valid, non-empty tile, otherwise retry

3. Choose direction:

   ```
   Select direction (L/R):
   ```

   * `L` = left
   * `R` = right

---

### Move Mechanics

After selecting a tile and direction:

1. Pick up all stones from the selected tile
2. Distribute stones one-by-one into adjacent tiles in the chosen direction

After placing the last stone:

#### Case 1: Next tile has stones

* Pick up stones from that tile
* Continue distributing in the same direction

#### Case 2: Next tile is empty

* Look at the following tile:

  * If also empty → turn ends
  * If it has stones → **capture all stones**

    * Add them to your score
    * Set that tile to 0
    * Continue checking for chained captures

---

### Refill Rule

If all your tiles (1–5) are empty:

* If your score > 0:

  * Spend 1 point per tile to refill from left to right
  * Stop when all tiles are filled or score reaches 0
* If your score = 0:

  * You lose your turn

---

### Enemy Turn

After your move:

* Enemy follows the same rules
* Chooses a valid tile and direction automatically (currently random)
* The chosen move is displayed

---

### Game End & Winning

The game ends when:

```
Both edge tiles (A and B) become empty
```

| Result         | Condition                |
| -------------- | ------------------------ |
| **You win**    | Your score > enemy score |
| **Enemy wins** | Enemy score > your score |
| **Draw**       | Scores are equal         |

After the game ends, you can choose to play again.

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
Select your tile (1-5): 1
Select direction (L/R): R

Board updates:
...

Enemy's turn:
Enemy selects tile 8, direction L

Board updates:
...
```

---

## Project Structure

```
CLI-MSC/
├── MSC.fsproj
├── run.bat
├── run.sh
├── README.md
└── MSC/
    ├── Board.fs     # Board state and operations
    ├── Move.fs      # Move and direction types
    ├── Rules.fs     # Game rules and move logic
    ├── Game.fs      # Game loop and input handling
    └── Program.fs   # Entry point
```

---

## Module Overview

| Module    | Responsibility                                     |
| --------- | -------------------------------------------------- |
| `Board`   | Mutable board state and operations                 |
| `Move`    | Move representation (tile + direction)             |
| `Rules`   | Move execution, capture logic, game end conditions |
| `Game`    | Turn handling, input validation, AI behavior       |
| `Program` | Entry point and main loop                          |

---

## Author Notes

* Invalid inputs do **not** consume your turn
* Enemy uses the same coordinate system as the player
* The game is deterministic except for AI randomness
* Designed for clarity and learning purposes

---

## Future Improvements

* Smarter AI (Minimax or heuristic-based)
* Improved board visualization
* Configurable rules and scoring
* Multiplayer mode
