# CLI Mandarin Square Capture (Ô ăn quan)

A command-line implementation of a simplified variant of Mandarin Square Capture, built with **F# / .NET 10**.

Play against an AI opponent in this traditional Vietnamese strategy game of sowing and capture chains.

---

## What is this game?

Mandarin Square Capture (Ô ăn quan) is a traditional Vietnamese two-player board game.

Each turn, a player:

* Picks a tile on their side
* Distributes its stones across the board
* Continues sowing or captures stones depending on the landing position

**Goal:** Collect more stones than your opponent before both side tiles have been captured.

> ⚠️ Note: This implementation uses a custom simplified rule set designed for learning and experimentation.

---

## Getting Started

### Prerequisites

* [.NET 10 SDK](https://dotnet.microsoft.com/download)

   Verify with:

   ```bash
   dotnet --version

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
* 2 edge tiles (A and B)

```
|   | 1  | 2  | 3  | 4  | 5  |   |
| B +----+----+----+----+----+ A |
|   | 1  | 2  | 3  | 4  | 5  |   |
```

* You only control tiles **1–5** on the bottom row
* The enemy only controls tiles **1–5** on the top row
* Both player's directions are as follows: left = clockwise, right = counterclockwise
* All tiles start with **5 stones**.
* Tiles A and B are special side tiles used for capture and game-ending conditions

---

## How to Play

### Game Start

1. The board is displayed 
2. Scores are initialized:

   * Your score = 0
   * Enemy score = 0
3. You are prompted:

   ```
   Would you like to go first? (Y/N)
   ```
4. Input:

   * `Y` → you go first
   * `N` → enemy goes first
   * Invalid input prompts retry message

---

### Your Turn

1. Choose tile: 

   ```
   Select a non-empty existing tile on your side (1–5):
   ```
   * From left to right: 1, 2, 3, 4, 5
   * Invalid input prompts retry message
   * If all existing tiles on your side is empty, follow the Refill Rule (see below)

2. Choose direction:

   ```
   Select direction (L/R):
   ```

   * `L` = left
   * `R` = right
   * Invalid input prompts retry message

---

### Move Mechanics

After selecting a tile and direction, a move consists of 2 phases:

#### Phase 1: Sowing

* Pick up stones from the chosen tile
* Distribute them one-by-one in the selected direction
* After placing the final stone:

   * Case 1: Next tile contains stones:

      * If the next tile is a regular tile: pick up all stones from that tile and continue sowing in the same direction
      * If the next tile is a side tile (A or B): the turn immediately ends

   * Case 2: Next tile doesn't contain stones:

      * The game enters its capture phase

#### Phase 2: Capture chain

* Capture chains follow this pattern: empty -> stones -> empty -> ...

   * If the next tile is empty and the tile after it contains stones, then:

      * All stones from that tile are captured and added to the current player's score,
      * The chain continues forward.
   
   * The capture chain ends when either one of these events occur:

      * empty -> empty
      * stones -> stones

---

### Refill Rule

If all tiles on a player's side are empty at the beginning of the turn:

* If their score > 0:

  * Spend 1 point per tile to refill from left to right
  * Stop when all tiles are filled or score reaches 0
* If their score = 0:

  * Their turn is skipped

---

### Enemy Turn

The enemy follows the same rules as the player.
Currently, the AI:

* Randomly selects a valid tile,
* Randomly chooses a direction.

The chosen move is displayed before execution.

---

### Game End & Winning

The game ends when:

```
Both side tiles (A and B) have been captured at least once
Whenever a side tile is captured for the first time, the game displays:
* >>> The LEFT side tile has been captured for the first time!
or
* >>> The RIGHT side tile has been captured for the first time!

```

| Result         | Condition                |
| -------------- | ------------------------ |
| **You win**    | Your score > enemy score |
| **Enemy wins** | Enemy score > your score |
| **Draw**       | Scores are equal         |

---

## Example Session

```
Do you want to go first? (Y/N): Y (You go first)
=== CLI MSC ===

|   | 5 | 5 | 5 | 5 | 5 |   |
| 5 +---+---+---+---+---+ 5 |
|   | 5 | 5 | 5 | 5 | 5 |   |

Your score = 0, enemy score = 0

(Your turn)
Select your tile (1-5): 1
Direction (L/R): R

(Board updates)

|   | 6 | 6 | 6 | 6 | 0 |   |
| 6 +---+---+---+---+---+ 6 |
|   | 0 | 0 | 6 | 6 | 6 |   |

Your score = 6, enemy score = 0
(Enemy's turn)
Enemy selects tile 2, direction Right

(Board updates)

|   | 8 | 1 | 7 | 7 | 1 |   |
| 7 +---+---+---+---+---+ 7 |
|   | 1 | 1 | 7 | 7 | 0 |   |

Your score = 6, enemy score = 0
(Your turn)
```

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
    ├── Rules.fs     # Move logic
    ├── Game.fs      # Game loop, game rules and input handling
    └── Program.fs   # Entry point
```

---

## Module Overview

| Module    | Responsibility                                               |
| --------- | -------------------------------------------------------------|
| `Board`   | Mutable board state and operations                           |
| `Move`    | Move representation (tile + direction)                       |
| `Rules`   | Sowing logic and capture-chain mechanics                     |
| `Game`    | Turn handling, input validation, scoring & AI behavior       |
| `Program` | Application entry point                                      |

---

## Author Notes

* Invalid inputs do not consume a turn
* Side tiles cannot be picked up during sow continuation
* Enemy uses the same movement rules as the player
* AI behavior is currently random
* Designed primarily for learning and experimentation

---

## Future Improvements

* Smarter AI (Minimax or heuristic-based)
* Replay system
* Improved terminal board visualization
* Save/load functionality
* Configurable rule variants
* Multiplayer mode
