#include <iostream>
#include <vector>
#include <limits>
#include <algorithm>
#include <windows.h> // For color on Windows

using namespace std;

// Constants for colors (Windows)
const int COLOR_DEFAULT = 7;
const int COLOR_PLAYER_X = 12; // Light Red
const int COLOR_PLAYER_O = 11; // Light Cyan
const int COLOR_WIN = 10;      // Light Green
const int COLOR_TITLE = 14;    // Yellow

class TicTacToe {
private:
    vector<vector<char>> board;
    char currentPlayer;
    bool gameOver;
    int xWins;
    int oWins;
    int draws;
    
    void setColor(int color) {
        SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE), color);
    }
    
    void resetColor() {
        setColor(COLOR_DEFAULT);
    }
    
    void drawBoard() {
        system("cls");
        
        setColor(COLOR_TITLE);
        cout << "  TIC TAC TOE  " << endl;
        cout << " X: " << xWins << " | O: " << oWins << " | Draws: " << draws << endl << endl;
        resetColor();
        
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                setColor(board[i][j] == 'X' ? COLOR_PLAYER_X : 
                       board[i][j] == 'O' ? COLOR_PLAYER_O : COLOR_DEFAULT);
                cout << " " << (board[i][j] == 0 ? '1' + i * 3 + j : board[i][j]) << " ";
                resetColor();
                if (j < 2) cout << "|";
            }
            cout << endl;
            if (i < 2) cout << "-----------" << endl;
        }
        cout << endl;
    }
    
    bool checkWin(char player) {
        // Check rows and columns
        for (int i = 0; i < 3; i++) {
            if (board[i][0] == player && board[i][1] == player && board[i][2] == player) return true;
            if (board[0][i] == player && board[1][i] == player && board[2][i] == player) return true;
        }
        // Check diagonals
        if (board[0][0] == player && board[1][1] == player && board[2][2] == player) return true;
        if (board[0][2] == player && board[1][1] == player && board[2][0] == player) return true;
        return false;
    }
    
    bool isBoardFull() {
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                if (board[i][j] == 0) return false;
            }
        }
        return true;
    }
    
    void makeMove(int position) {
        int row = (position - 1) / 3;
        int col = (position - 1) % 3;
        
        if (position < 1 || position > 9 || board[row][col] != 0) {
            cout << "Invalid move! Try again." << endl;
            Sleep(1000);
            return;
        }
        
        board[row][col] = currentPlayer;
        
        if (checkWin(currentPlayer)) {
            drawBoard();
            setColor(COLOR_WIN);
            cout << "Player " << currentPlayer << " wins!" << endl;
            resetColor();
            if (currentPlayer == 'X') xWins++;
            else oWins++;
            gameOver = true;
        } else if (isBoardFull()) {
            drawBoard();
            setColor(COLOR_TITLE);
            cout << "It's a draw!" << endl;
            resetColor();
            draws++;
            gameOver = true;
        } else {
            currentPlayer = (currentPlayer == 'X') ? 'O' : 'X';
        }
    }
    
    int minimax(bool isMaximizing) {
        if (checkWin('X')) return 1;
        if (checkWin('O')) return -1;
        if (isBoardFull()) return 0;
        
        if (isMaximizing) {
            int bestScore = -numeric_limits<int>::max();
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    if (board[i][j] == 0) {
                        board[i][j] = 'X';
                        int score = minimax(false);
                        board[i][j] = 0;
                        bestScore = max(score, bestScore);
                    }
                }
            }
            return bestScore;
        } else {
            int bestScore = numeric_limits<int>::max();
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    if (board[i][j] == 0) {
                        board[i][j] = 'O';
                        int score = minimax(true);
                        board[i][j] = 0;
                        bestScore = min(score, bestScore);
                    }
                }
            }
            return bestScore;
        }
    }
    
    void aiMove() {
        int bestScore = -numeric_limits<int>::max();
        int bestMove = -1;
        
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                if (board[i][j] == 0) {
                    board[i][j] = 'X';
                    int score = minimax(false);
                    board[i][j] = 0;
                    if (score > bestScore) {
                        bestScore = score;
                        bestMove = i * 3 + j + 1;
                    }
                }
            }
        }
        
        makeMove(bestMove);
    }
    
public:
    TicTacToe() : board(3, vector<char>(3, 0)), currentPlayer('X'), gameOver(false), xWins(0), oWins(0), draws(0) {}
    
    void startGame() {
        int mode;
        cout << "Select game mode:" << endl;
        cout << "1. Player vs Player" << endl;
        cout << "2. Player vs AI" << endl;
        cout << "Enter choice (1-2): ";
        cin >> mode;
        
        while (mode < 1 || mode > 2) {
            cout << "Invalid choice. Enter 1 or 2: ";
            cin >> mode;
        }
        
        bool playerVsAI = (mode == 2);
        
        while (true) {
            // Reset game
            board = vector<vector<char>>(3, vector<char>(3, 0));
            currentPlayer = 'X';
            gameOver = false;
            
            while (!gameOver) {
                drawBoard();
                
                if (currentPlayer == 'O' || !playerVsAI) {
                    int move;
                    cout << "Player " << currentPlayer << ", enter your move (1-9): ";
                    cin >> move;
                    
                    while (cin.fail()) {
                        cin.clear();
                        cin.ignore(numeric_limits<streamsize>::max(), '\n');
                        cout << "Invalid input. Enter a number (1-9): ";
                        cin >> move;
                    }
                    
                    makeMove(move);
                } else {
                    cout << "AI is thinking..." << endl;
                    Sleep(1000);
                    aiMove();
                }
            }
            
            drawBoard();
            cout << "Game over!" << endl;
            
            char playAgain;
            cout << "Play again? (y/n): ";
            cin >> playAgain;
            
            if (tolower(playAgain) != 'y') {
                break;
            }
        }
    }
};

int main() {
    TicTacToe game;
    game.startGame();
    return 0;
}
