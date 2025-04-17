import pygame
import sys
import numpy as np
import time

# Initialize pygame
pygame.init()

# Constants
WIDTH, HEIGHT = 600, 700
LINE_WIDTH = 15
BOARD_ROWS, BOARD_COLS = 3, 3
SQUARE_SIZE = WIDTH // BOARD_COLS
CIRCLE_RADIUS = SQUARE_SIZE // 3
CIRCLE_WIDTH = 15
CROSS_WIDTH = 25
SPACE = SQUARE_SIZE // 4
BG_COLOR = (28, 170, 156)
LINE_COLOR = (23, 145, 135)
CIRCLE_COLOR = (239, 231, 200)
CROSS_COLOR = (66, 66, 66)
FONT_COLOR = (239, 231, 200)

# Screen
screen = pygame.display.set_mode((WIDTH, HEIGHT))
pygame.display.set_caption('Tic Tac Toe AI')
screen.fill(BG_COLOR)

# Board
board = np.zeros((BOARD_ROWS, BOARD_COLS))

# Font
font = pygame.font.SysFont('arial', 60)
small_font = pygame.font.SysFont('arial', 30)

def draw_lines():
    # Horizontal lines
    pygame.draw.line(screen, LINE_COLOR, (0, SQUARE_SIZE), (WIDTH, SQUARE_SIZE), LINE_WIDTH)
    pygame.draw.line(screen, LINE_COLOR, (0, 2 * SQUARE_SIZE), (WIDTH, 2 * SQUARE_SIZE), LINE_WIDTH)
    # Vertical lines
    pygame.draw.line(screen, LINE_COLOR, (SQUARE_SIZE, 0), (SQUARE_SIZE, HEIGHT - 100), LINE_WIDTH)
    pygame.draw.line(screen, LINE_COLOR, (2 * SQUARE_SIZE, 0), (2 * SQUARE_SIZE, HEIGHT - 100), LINE_WIDTH)

def draw_figures():
    for row in range(BOARD_ROWS):
        for col in range(BOARD_COLS):
            if board[row][col] == 1:
                pygame.draw.circle(screen, CIRCLE_COLOR, 
                                  (int(col * SQUARE_SIZE + SQUARE_SIZE // 2), 
                                   int(row * SQUARE_SIZE + SQUARE_SIZE // 2)), 
                                  CIRCLE_RADIUS, CIRCLE_WIDTH)
            elif board[row][col] == 2:
                pygame.draw.line(screen, CROSS_COLOR, 
                                (col * SQUARE_SIZE + SPACE, row * SQUARE_SIZE + SQUARE_SIZE - SPACE), 
                                (col * SQUARE_SIZE + SQUARE_SIZE - SPACE, row * SQUARE_SIZE + SPACE), 
                                CROSS_WIDTH)
                pygame.draw.line(screen, CROSS_COLOR, 
                                (col * SQUARE_SIZE + SPACE, row * SQUARE_SIZE + SPACE), 
                                (col * SQUARE_SIZE + SQUARE_SIZE - SPACE, row * SQUARE_SIZE + SQUARE_SIZE - SPACE), 
                                CROSS_WIDTH)

def mark_square(row, col, player):
    board[row][col] = player

def available_square(row, col):
    return board[row][col] == 0

def is_board_full():
    for row in range(BOARD_ROWS):
        for col in range(BOARD_COLS):
            if board[row][col] == 0:
                return False
    return True

def check_win(player):
    # Vertical win check
    for col in range(BOARD_COLS):
        if board[0][col] == player and board[1][col] == player and board[2][col] == player:
            draw_vertical_winning_line(col, player)
            return True
    # Horizontal win check
    for row in range(BOARD_ROWS):
        if board[row][0] == player and board[row][1] == player and board[row][2] == player:
            draw_horizontal_winning_line(row, player)
            return True
    # Ascending diagonal win check
    if board[2][0] == player and board[1][1] == player and board[0][2] == player:
        draw_asc_diagonal(player)
        return True
    # Descending diagonal win check
    if board[0][0] == player and board[1][1] == player and board[2][2] == player:
        draw_desc_diagonal(player)
        return True
    return False

def draw_vertical_winning_line(col, player):
    posX = col * SQUARE_SIZE + SQUARE_SIZE // 2
    color = CIRCLE_COLOR if player == 1 else CROSS_COLOR
    pygame.draw.line(screen, color, (posX, 15), (posX, HEIGHT - 115), 15)

def draw_horizontal_winning_line(row, player):
    posY = row * SQUARE_SIZE + SQUARE_SIZE // 2
    color = CIRCLE_COLOR if player == 1 else CROSS_COLOR
    pygame.draw.line(screen, color, (15, posY), (WIDTH - 15, posY), 15)

def draw_asc_diagonal(player):
    color = CIRCLE_COLOR if player == 1 else CROSS_COLOR
    pygame.draw.line(screen, color, (15, HEIGHT - 115), (WIDTH - 15, 15), 15)

def draw_desc_diagonal(player):
    color = CIRCLE_COLOR if player == 1 else CROSS_COLOR
    pygame.draw.line(screen, color, (15, 15), (WIDTH - 15, HEIGHT - 115), 15)

def restart():
    screen.fill(BG_COLOR)
    draw_lines()
    for row in range(BOARD_ROWS):
        for col in range(BOARD_COLS):
            board[row][col] = 0

def ai_move():
    best_score = -float('inf')
    move = None
    for row in range(BOARD_ROWS):
        for col in range(BOARD_COLS):
            if board[row][col] == 0:
                board[row][col] = 2
                score = minimax(board, 0, False)
                board[row][col] = 0
                if score > best_score:
                    best_score = score
                    move = (row, col)
    if move:
        mark_square(move[0], move[1], 2)
        return True
    return False

def minimax(board, depth, is_maximizing):
    if check_win(2):
        return 1
    elif check_win(1):
        return -1
    elif is_board_full():
        return 0

    if is_maximizing:
        best_score = -float('inf')
        for row in range(BOARD_ROWS):
            for col in range(BOARD_COLS):
                if board[row][col] == 0:
                    board[row][col] = 2
                    score = minimax(board, depth + 1, False)
                    board[row][col] = 0
                    best_score = max(score, best_score)
        return best_score
    else:
        best_score = float('inf')
        for row in range(BOARD_ROWS):
            for col in range(BOARD_COLS):
                if board[row][col] == 0:
                    board[row][col] = 1
                    score = minimax(board, depth + 1, True)
                    board[row][col] = 0
                    best_score = min(score, best_score)
        return best_score

def draw_status(player, game_over):
    pygame.draw.rect(screen, BG_COLOR, (0, HEIGHT - 100, WIDTH, 100))
    if game_over:
        if player == 1:
            text = "Player O wins!"
        elif player == 2:
            text = "Player X wins!"
        else:
            text = "Game is a tie!"
        label = font.render(text, True, FONT_COLOR)
        screen.blit(label, (WIDTH // 2 - label.get_width() // 2, HEIGHT - 80))
    else:
        text = "Player O's turn" if player == 1 else "Player X's turn"
        label = small_font.render(text, True, FONT_COLOR)
        screen.blit(label, (WIDTH // 2 - label.get_width() // 2, HEIGHT - 80))
    
    restart_text = small_font.render("Press R to restart", True, FONT_COLOR)
    screen.blit(restart_text, (WIDTH // 2 - restart_text.get_width() // 2, HEIGHT - 40))

draw_lines()
player = 1
game_over = False

# Main game loop
while True:
    for event in pygame.event.get():
        if event.type == pygame.QUIT:
            sys.exit()
        
        if event.type == pygame.KEYDOWN:
            if event.key == pygame.K_r:
                restart()
                player = 1
                game_over = False
        
        if not game_over:
            if player == 1 and event.type == pygame.MOUSEBUTTONDOWN:
                mouseX = event.pos[0]
                mouseY = event.pos[1]
                
                clicked_row = int(mouseY // SQUARE_SIZE)
                clicked_col = int(mouseX // SQUARE_SIZE)
                
                if clicked_row < 3 and available_square(clicked_row, clicked_col):
                    mark_square(clicked_row, clicked_col, player)
                    if check_win(player):
                        game_over = True
                    player = 2
                    
            elif player == 2 and not game_over:
                if ai_move():
                    if check_win(2):
                        game_over = True
                    player = 1
    
    draw_figures()
    draw_status(player, game_over)
    pygame.display.update()
