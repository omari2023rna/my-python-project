#include <stdio.h>
#include <stdbool.h>
#include <stdlib.h>
#include <ctype.h>

char board[3][3];
const char PLAYER = 'X';
const char COMPUTER = 'O';

void resetBoard() {
    for(int i = 0; i < 3; i++) {
        for(int j = 0; j < 3; j++) {
            board[i][j] = ' ';
        }
    }
}

void printBoard() {
    printf(" %c | %c | %c ", board[0][0], board[0][1], board[0][2]);
    printf("\n---|---|---\n");
    printf(" %c | %c | %c ", board[1][0], board[1][1], board[1][2]);
    printf("\n---|---|---\n");
    printf(" %c | %c | %c ", board[2][0], board[2][1], board[2][2]);
    printf("\n");
}

int checkFreeSpaces() {
    int freeSpaces = 9;
    for(int i = 0; i < 3; i++) {
        for(int j = 0; j < 3; j++) {
            if(board[i][j] != ' ') {
                freeSpaces--;
            }
        }
    }
    return freeSpaces;
}

void playerMove() {
    int x, y;
    do {
        printf("Enter row #(1-3): ");
        scanf("%d", &x);
        x--;
        printf("Enter column #(1-3): ");
        scanf("%d", &y);
        y--;

        if(board[x][y] != ' ') {
            printf("Invalid move!\n");
        } else {
            board[x][y] = PLAYER;
            break;
        }
    } while (board[x][y] != ' ');
}

void computerMove() {
    // Simple AI - first checks for winning move, then blocking move, then random
    // Check for winning move
    for(int i = 0; i < 3; i++) {
        for(int j = 0; j < 3; j++) {
            if(board[i][j] == ' ') {
                board[i][j] = COMPUTER;
                if(checkWinner() == COMPUTER) {
                    return;
                } else {
                    board[i][j] = ' ';
                }
            }
        }
    }

    // Check for blocking move
    for(int i = 0; i < 3; i++) {
        for(int j = 0; j < 3; j++) {
            if(board[i][j] == ' ') {
                board[i][j] = PLAYER;
                if(checkWinner() == PLAYER) {
                    board[i][j] = COMPUTER;
                    return;
                } else {
                    board[i][j] = ' ';
                }
            }
        }
    }

    // Random move
    int x, y;
    do {
        x = rand() % 3;
        y = rand() % 3;
    } while (board[x][y] != ' ');
    
    board[x][y] = COMPUTER;
}

char checkWinner() {
    // Check rows
    for(int i = 0; i < 3; i++) {
        if(board[i][0] == board[i][1] && board[i][1] == board[i][2]) {
            return board[i][0];
        }
    }
    // Check columns
    for(int i = 0; i < 3; i++) {
        if(board[0][i] == board[1][i] && board[1][i] == board[2][i]) {
            return board[0][i];
        }
    }
    // Check diagonals
    if(board[0][0] == board[1][1] && board[1][1] == board[2][2]) {
        return board[0][0];
    }
    if(board[0][2] == board[1][1] && board[1][1] == board[2][0]) {
        return board[0][2];
    }
    return ' ';
}

void printWinner(char winner) {
    if(winner == PLAYER) {
        printf("YOU WIN!\n");
    } else if(winner == COMPUTER) {
        printf("YOU LOSE!\n");
    } else {
        printf("IT'S A TIE!\n");
    }
}

int main() {
    char winner = ' ';
    char response;

    do {
        winner = ' ';
        response = ' ';
        resetBoard();

        while(winner == ' ' && checkFreeSpaces() != 0) {
            printBoard();

            playerMove();
            winner = checkWinner();
            if(winner != ' ' || checkFreeSpaces() == 0) {
                break;
            }

            computerMove();
            winner = checkWinner();
            if(winner != ' ' || checkFreeSpaces() == 0) {
                break;
            }
        }

        printBoard();
        printWinner(winner);

        printf("\nWould you like to play again? (Y/N): ");
        scanf(" %c", &response);
        response = toupper(response);
    } while (response == 'Y');

    printf("Thanks for playing!\n");

    return 0;
}
