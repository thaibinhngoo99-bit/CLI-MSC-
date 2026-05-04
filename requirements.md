**Project Title:** CLI Mandarin Square Capture (MSC)

---

## Overview

This project is a command-line implementation of Mandarin Square Capture (Ô ăn quan), where the user plays against an AI opponent.

The user can choose to go first or second, and the game proceeds in alternating turns. The AI currently selects moves randomly.

This version uses a **simplified rule set**, where all tiles are treated uniformly and the game ends when both edge tiles are empty.

---

## Requirements

1. **Game Start**

   * The user chooses whether to go first or second.
   * The game displays:

     * A board with **12 tiles**, each containing 5 stones
     * Initial scores:

       ```
       Your score = 0, Enemy score = 0
       ```
     * A guide for selecting tiles and directions

---

2. **User Turn**

   * If all tiles on the user’s side (tiles 1–5) are empty:

     * The **refill rule** is applied
   * Otherwise:

     * The user selects:

       * A tile (1–5)
       * A direction (`L` or `R`)
   * Input validation:

     * Invalid tile or direction → user must retry
     * Selecting an empty tile → user must retry

---

3. **Move Execution**

   * If the input is valid:

     * The move follows the **distribution and capture mechanics**
     * Stones are distributed one-by-one in the chosen direction
     * Captures occur according to the game rules
   * After the move:

     * The board and scores are updated and displayed

---

4. **Enemy Turn**

   * After the user’s turn:

     * The enemy takes a turn automatically
   * The enemy:

     * Selects a **random non-empty tile**
     * Selects a **random direction**
     * Applies the same move mechanics as the user
   * If all enemy tiles are empty:

     * The enemy follows the refill rule

---

5. **Turn Alternation**

   * The user and enemy take turns alternately
   * After each turn:

     * The updated board and scores are displayed

---

6. **Game End Condition**

   * The game ends when:

     ```
     Both edge tiles are empty
     ```
   * Final scores are compared:

     * Higher score → winner
     * Equal scores → draw

---

7. **Example Interaction**

* The game prints the initial board with 5 stones per tile.
* The user selects:

  ```
  Tile: 5
  Direction: L
  ```
* The game:

  * Distributes stones from tile 5 to adjacent tiles on the left
  * Applies capture mechanics if applicable
* After the user’s turn:

  * The updated board and scores are printed
* The enemy:

  * Randomly selects a valid tile and direction
  * Performs its move
* The game continues until the end condition is met.

---

## Notes

* Invalid inputs do **not** consume the user’s turn
* The AI currently behaves randomly (no strategy)
* The game is deterministic except for enemy move selection
* Designed for clarity, correctness, and incremental development
