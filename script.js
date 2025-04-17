document.addEventListener('DOMContentLoaded', () => {
    const socket = io();
    const board = document.getElementById('board');
    const cells = document.querySelectorAll('.cell');
    const status = document.getElementById('status');
    const playerScore = document.getElementById('player-score');
    const opponentScore = document.getElementById('opponent-score');
    const restartBtn = document.getElementById('restart');
    const connectionStatus = document.getElementById('connection-status');
    const modal = document.getElementById('modal');
    const modalTitle = document.getElementById('modal-title');
    const modalMessage = document.getElementById('modal-message');
    const playerNameInput = document.getElementById('player-name');
    const startGameBtn = document.getElementById('start-game');

    let playerSymbol = '';
    let currentPlayer = '';
    let gameActive = false;
    let playerName = '';
    let opponentName = '';
    let scores = { player: 0, opponent: 0 };

    // Show modal on start
    modal.style.display = 'flex';

    // Event listeners
    startGameBtn.addEventListener('click', () => {
        if (playerNameInput.value.trim() === '') {
            alert('Please enter your name');
            return;
        }
        playerName = playerNameInput.value.trim();
        modal.style.display = 'none';
        socket.emit('register', playerName);
    });

    restartBtn.addEventListener('click', () => {
        socket.emit('restart');
    });

    cells.forEach(cell => {
        cell.addEventListener('click', () => {
            if (gameActive && currentPlayer === playerSymbol) {
                const index = cell.getAttribute('data-index');
                socket.emit('move', index);
            }
        });
    });

    // Socket.io events
    socket.on('connect', () => {
        connectionStatus.textContent = 'Connected to server';
        connectionStatus.style.color = '#2ecc71';
    });

    socket.on('disconnect', () => {
        connectionStatus.textContent = 'Disconnected from server';
        connectionStatus.style.color = '#e74c3c';
        gameActive = false;
        status.textContent = 'Connection lost. Trying to reconnect...';
    });

    socket.on('reconnect', () => {
        connectionStatus.textContent = 'Reconnected to server';
        connectionStatus.style.color = '#2ecc71';
        if (playerName) {
            socket.emit('register', playerName);
        }
    });

    socket.on('waiting', () => {
        status.textContent = 'Waiting for opponent...';
        gameActive = false;
    });

    socket.on('gameStart', (data) => {
        playerSymbol = data.symbol;
        currentPlayer = data.currentPlayer;
        opponentName = data.opponentName;
        gameActive = true;
        
        updateStatus();
        resetBoard();
    });

    socket.on('move', (data) => {
        const { index, player, winningCells } = data;
        
        // Update board
        const cell = cells[index];
        cell.classList.add(player === 'X' ? 'x' : 'o');
        cell.textContent = player;
        
        // Check for winner or draw
        if (winningCells) {
            gameActive = false;
            winningCells.forEach(i => {
                cells[i].classList.add('winning-cell');
            });
            
            if (player === playerSymbol) {
                scores.player++;
                playerScore.textContent = scores.player;
                status.textContent = `You win!`;
            } else {
                scores.opponent++;
                opponentScore.textContent = scores.opponent;
                status.textContent = `${opponentName} wins!`;
            }
        } else if (data.isDraw) {
            gameActive = false;
            status.textContent = 'Game ended in a draw!';
        } else {
            currentPlayer = player === 'X' ? 'O' : 'X';
            updateStatus();
        }
    });

    socket.on('restart', () => {
        resetBoard();
        currentPlayer = 'X';
        gameActive = true;
        updateStatus();
    });

    socket.on('opponentDisconnected', () => {
        gameActive = false;
        status.textContent = 'Opponent disconnected. Waiting for new opponent...';
        resetBoard();
    });

    socket.on('error', (message) => {
        alert(message);
    });

    // Helper functions
    function updateStatus() {
        if (currentPlayer === playerSymbol) {
            status.textContent = 'Your turn';
        } else {
            status.textContent = `${opponentName}'s turn`;
        }
    }

    function resetBoard() {
        cells.forEach(cell => {
            cell.textContent = '';
            cell.classList.remove('x', 'o', 'winning-cell');
        });
    }
});
