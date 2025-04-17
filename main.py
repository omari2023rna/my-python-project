def print_board(board):
    for i in range(3):
        print(f" {board[i*3]} | {board[i*3+1]} | {board[i*3+2]} ")
        if i < 2:
            print("-----------")

def check_winner(board):
    # Check rows
    for i in range(0, 9, 3):
        if board[i] == board[i+1] == board[i+2] != " ":
            return board[i]
    
    # Check columns
    for i in range(3):
        if board[i] == board[i+3] == board[i+6] != " ":
            return board[i]
    
    # Check diagonals
    if board[0] == board[4] == board[8] != " ":
        return board[0]
    if board[2] == board[4] == board[6] != " ":
        return board[2]
    
    # Check for tie
    if " " not in board:
        return "Tie"
    
    return None

def main():
    board = [" "] * 9
    current_player = "X"
    
    print("Welcome to Tic-Tac-Toe!")
    print("Enter a number (1-9) to make your move:")
    print_board(["1", "2", "3", "4", "5", "6", "7", "8", "9"])
    print("\nLet's begin!\n")
    
    while True:
        print_board(board)
        move = input(f"Player {current_player}'s turn (1-9): ")
        
        try:
            move = int(move) - 1
            if move < 0 or move > 8:
                print("Please enter a number between 1 and 9.")
                continue
            if board[move] != " ":
                print("That position is already taken!")
                continue
        except ValueError:
            print("Please enter a valid number!")
            continue
        
        board[move] = current_player
        winner = check_winner(board)
        
        if winner:
            print_board(board)
            if winner == "Tie":
                print("It's a tie!")
            else:
                print(f"Player {winner} wins!")
            break
        
        current_player = "O" if current_player == "X" else "X"

if __name__ == "__main__":
    main()
