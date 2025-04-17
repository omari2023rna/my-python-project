import java.util.Scanner;

public class TicTacToe {
    private static char[] board = {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '};
    private static char currentPlayer = 'X';
    private static boolean gameActive = true;

    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        System.out.println("Welcome to Tic-Tac-Toe!");
        System.out.println("Enter a number (1-9) to make your move:");
        printSampleBoard();
        System.out.println("\nLet's begin!\n");
        
        while (gameActive) {
            printBoard();
            System.out.print("Player " + currentPlayer + "'s turn (1-9): ");
            String input = scanner.nextLine();
            
            try {
                int move = Integer.parseInt(input) - 1;
                
                if (move < 0 || move > 8) {
                    System.out.println("Please enter a number between 1 and 9.");
                    continue;
                }
                
                if (board[move] != ' ') {
                    System.out.println("That position is already taken!");
                    continue;
                }
                
                board[move] = currentPlayer;
                checkGameStatus();
                
                if (gameActive) {
                    currentPlayer = (currentPlayer == 'X') ? 'O' : 'X';
                }
            } catch (NumberFormatException e) {
                System.out.println("Please enter a valid number!");
            }
        }
        scanner.close();
    }

    private static void printSampleBoard() {
        System.out.println(" 1 | 2 | 3 ");
        System.out.println("-----------");
        System.out.println(" 4 | 5 | 6 ");
        System.out.println("-----------");
        System.out.println(" 7 | 8 | 9 ");
    }

    private static void printBoard() {
        System.out.println(" " + board[0] + " | " + board[1] + " | " + board[2] + " ");
        System.out.println("-----------");
        System.out.println(" " + board[3] + " | " + board[4] + " | " + board[5] + " ");
        System.out.println("-----------");
        System.out.println(" " + board[6] + " | " + board[7] + " | " + board[8] + " ");
    }

    private static void checkGameStatus() {
        // Check rows
        for (int i = 0; i < 9; i += 3) {
            if (board[i] != ' ' && board[i] == board[i+1] && board[i] == board[i+2]) {
                printBoard();
                System.out.println("Player " + currentPlayer + " wins!");
                gameActive = false;
                return;
            }
        }
        
        // Check columns
        for (int i = 0; i < 3; i++) {
            if (board[i] != ' ' && board[i] == board[i+3] && board[i] == board[i+6]) {
                printBoard();
                System.out.println("Player " + currentPlayer + " wins!");
                gameActive = false;
                return;
            }
        }
        
        // Check diagonals
        if (board[0] != ' ' && board[0] == board[4] && board[0] == board[8]) {
            printBoard();
            System.out.println("Player " + currentPlayer + " wins!");
            gameActive = false;
            return;
        }
        
        if (board[2] != ' ' && board[2] == board[4] && board[2] == board[6]) {
            printBoard();
            System.out.println("Player " + currentPlayer + " wins!");
            gameActive = false;
            return;
        }
        
        // Check for tie
        boolean isTie = true;
        for (char c : board) {
            if (c == ' ') {
                isTie = false;
                break;
            }
        }
        
        if (isTie) {
            printBoard();
            System.out.println("It's a tie!");
            gameActive = false;
        }
    }
}
