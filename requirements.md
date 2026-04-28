Project Title: CLI -MSC-

Overview: This project is a command-line Mandarin Square Capture (Ô ăn quan) game where the user plays against a random enemy. The user can choose to go first/ last.

Requirements:

1. The user will choose to go first or last.
2. The user will see a MSC board with 5 stones per tile, a scoreboard of 0 to 0 and a guide to select tiles is printed.
2. The user will make a move by either following the refill rule or selecting a tile from 1 to 5 and a left/right direction. 
3. If the user selects a non-empty tile and a valid direction, the user's turn will follow the move mechanics. Otherwise, the user will be asked to retry the input.
4. After the user's turn is completed and the scores & board are updated, it becomes the enemy's turn. The enemy will always either choose a random non-empty tile and a random direction, or follow the refill rule. The same move mechanics is applied to the enemy.
5. The user and the enemy will take turns making moves.
6. When both Mandarin tiles are captured at least once, the game ends and the scores are finalized. Whoever has the higher score wins. Otherwise, the game is tied.
7. Example Interaction: The game prints a MSC board with 5 stones per tile. The user enters 5 and L. The game starts distributing stones from tile 5 one-by-one to adjacent tiles to the left of it and the user's turn follows the move mechanics. Once the user's turn ends, the game prints the updated scores and board and the enemy moves next. The enemy randomly chooses a non-empty tile and a direction. Once the enemy's turn ends, the game prints the updated scores and board and asks the user for the next move...
