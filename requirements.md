**Project Title:** CLI Mandarin Square Capture (Ô ăn quan)

---

# Overview

This project is a command-line implementation of a simplified variant of Mandarin Square Capture (Ô ăn quan), where the player competes against an AI opponent.

The player may choose to go first or second, and the game proceeds in alternating turns.

This implementation uses a custom rule set featuring:

* continued sowing mechanics,
* chained captures,
* special side-tile behavior,
* custom game-ending conditions,
* replay support,
* improved terminal visualization,
* and multiple AI difficulty modes.

The game is implemented in **F# using .NET 10**.

---

# Features

* Command-line gameplay
* Improved terminal board visualization
* Highlighted scoreboards and announcements
* Execution delay prompts for readability
* Replay support
* Multiple AI difficulty modes:
  * Easy → random AI
  * Hard → heuristic/minimax-style AI (depth 1)
* Continued sowing mechanics
* Capture-chain system
* Refill rule implementation

---

# Requirements

## Game Start

1. A welcome message is displayed
2. You are prompted:

   ```
   Difficulty:
   1. Easy (For beginners, random moves)
   2. Hard (For strategists, uses Minimax)
   Select difficulty (E/H):
   ```
3. Input:

   * `E` → Easy difficulty
   * `H` → Hard difficulty
   * Invalid input prompts error message and default to easy difficulty
4. You are prompted:

   ```
   Would you like to go first? (Y/N)
   ```
5. Input:

   * `Y` → you go first
   * `N` → enemy goes first
   * Invalid input prompts retry message
6. The board is displayed 
7. Scores are initialized:

   ```
   YOUR SCORE : 0
   ENEMY SCORE: 0
   ```
---

## Turn Flow

Each turn consists of:
1. Choosing a tile 
2. Choosing a direction
3. Executing sowing logic
4. Possibly entering a capture chain
5. Updating scores and board states

## Player Turn

1. Choosing a tile: 

   ```
   Select a non-empty existing tile on your side (1–5):
   ```
   * From left to right: 1, 2, 3, 4, 5
   * Invalid input prompts retry message
   * If all existing tiles on your side is empty, follow the Refill Rule (see below)

2. Choosing a direction:

   ```
   Select direction (L/R):
   ```

   * `L` = left
   * `R` = right
   * Invalid input prompts retry message

---

## Move Mechanics

After selecting a tile and direction, a move consists of 2 phases:

### Phase 1: Sowing

* Pick up stones from the chosen tile
* Distribute them one-by-one in the selected direction
* After placing the final stone:

   * Case 1: Next tile contains stones:

      * If the next tile is a regular tile: pick up all stones from that tile and continue sowing in the same direction
      * If the next tile is a side tile (A or B): the turn immediately ends (side tiles cannot be picked up during sowing continuation)

   * Case 2: Next tile doesn't contain stones:

      * The game enters its capture phase

### Phase 2: Capture chain

* Capture chains follow this pattern: empty -> stones -> empty -> ...

   * If the next tile is empty and the tile after it contains stones, then:

      * All stones from that tile are captured and added to the current player's score,
      * The chain continues forward.
   
   * The capture chain ends when either one of these events occur:

      * empty -> empty
      * stones -> stones

---

## Refill Rule

If all tiles on a player's side are empty at the beginning of the turn:

* If their score > 0:

  * Spend 1 point per tile to refill in the counter-clockwise direction
  * Stop when all tiles are filled or score reaches 0
  * Their turn is skipped
* If their score = 0:

  * Their turn is skipped

---

## AI System

The game supports multiple AI difficulty modes.

### Easy Mode
* Random valid tile
* Random direction

### Hard Mode

* Uses a heuristic/minimax-style AI:
   * Evaluates all possible moves
   * Simulates captures
   * Chooses the move that gains the highest immediate score (Currently depth-1 evaluation.)

The chosen move is displayed before execution.

---

## Other UI features

### Execution Delay

To improve readability: 

* Player moves display:
  ```
   Executing move...
  ```
* Enemy turns display:
  ```
   Enemy is thinking...
  ```

The game pauses briefly before executing moves so players can observe the board state.

### Side Tile Capture Announcement

The game tracks the first capture of both side tiles.

When captured for the first time:

  ```
   >>> The LEFT side tile has been captured for the first time!
  ```
or
  ```
   >>> The LEFT side tile has been captured for the first time!
  ```
These announcements are highlighted in the terminal.

---

## Game End & Winning Conditions

The game ends when:

```
Both side tiles (A and B) have been captured at least once
```

| Result         | Condition                |
| -------------- | ------------------------ |
| **You win**    | Your score > enemy score |
| **Enemy wins** | Enemy score > your score |
| **Draw**       | Scores are equal         |

Final results are highlighted in the terminal.

---

## Replay Support

After a game finishes, the player is prompted:

```
Play again? (Y/N)
```

* `Y` → restart game
* `N` → exit program
---

## Example Session

```
=============================================
        CLI MANDARIN SQUARE CAPTURE
=============================================

Capture more stones than the enemy!
The game ends once BOTH side tiles
have been captured at least once.


Difficulty:
1. Easy (For beginners, random moves)
2. Hard (For strategists, uses Minimax)
Select difficulty (E/H): E (Easy mode)
Do you want to go first? (Y/N): Y (You go first)

                ENEMY

      ┌────┬────┬────┬────┬────┐
      │  5 │  5 │  5 │  5 │  5 │
      └────┴────┴────┴────┴────┘

┌────┐                          ┌────┐
│  5 │                          │  5 │
└────┘                          └────┘

      ┌────┬────┬────┬────┬────┐
      │  5 │  5 │  5 │  5 │  5 │
      └────┴────┴────┴────┴────┘

                YOU

=================================
   YOUR SCORE : 0
   ENEMY SCORE: 0
=================================


============== YOUR TURN ==============
Select your tile (1-5): 1
Direction (L/R): R
Executing your move...

                ENEMY

      ┌────┬────┬────┬────┬────┐
      │  6 │  6 │  6 │  6 │  0 │
      └────┴────┴────┴────┴────┘

┌────┐                          ┌────┐
│  6 │                          │  6 │
└────┘                          └────┘

      ┌────┬────┬────┬────┬────┐
      │  0 │  0 │  6 │  6 │  6 │
      └────┴────┴────┴────┴────┘

                YOU

=================================
   YOUR SCORE : 6
   ENEMY SCORE: 0
=================================


============== ENEMY TURN ==============
Enemy is thinking...
Enemy chooses tile 4, direction Left

#############################################
 LEFT SIDE TILE CAPTURED FOR FIRST TIME!
#############################################


                ENEMY

      ┌────┬────┬────┬────┬────┐
      │  6 │  6 │  6 │  0 │  1 │
      └────┴────┴────┴────┴────┘

┌────┐                          ┌────┐
│  0 │                          │  7 │
└────┘                          └────┘

      ┌────┬────┬────┬────┬────┐
      │  0 │  1 │  7 │  7 │  7 │
      └────┴────┴────┴────┴────┘

                YOU

=================================
   YOUR SCORE : 6
   ENEMY SCORE: 6
=================================


============== YOUR TURN ==============
...
```


# Author notes

* The number displayed on a tile of the board is the number of stones in that tile, not the tile's index
* Invalid inputs do not consume a turn
* Side tiles cannot be picked up during sow continuation
* Player and AI follow identical movement rules
* Hard AI currently uses depth-1 evaluation
* The project prioritizes readability and modularity
