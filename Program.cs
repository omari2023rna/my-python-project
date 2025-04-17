using System;
using System.Drawing;
using System.Windows.Forms;

namespace TicTacToe
{
    public partial class MainForm : Form
    {
        private Button[,] buttons = new Button[3, 3];
        private bool playerTurn = true; // true for X, false for O
        private int xWins = 0;
        private int oWins = 0;
        private int draws = 0;
        private bool gameOver = false;
        private bool vsComputer = false;

        public MainForm()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            // Create game board
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    buttons[i, j] = new Button();
                    buttons[i, j].Size = new Size(100, 100);
                    buttons[i, j].Location = new Point(j * 100, i * 100 + 50);
                    buttons[i, j].Font = new Font("Arial", 40, FontStyle.Bold);
                    buttons[i, j].Click += Button_Click;
                    this.Controls.Add(buttons[i, j]);
                }
            }

            // Initialize labels
            statusLabel.Text = "Player X's turn";
            scoreLabel.Text = $"X: {xWins} | O: {oWins} | Draws: {draws}";
        }

        private void Button_Click(object sender, EventArgs e)
        {
            if (gameOver) return;

            Button button = (Button)sender;
            if (button.Text != "") return;

            if (playerTurn)
            {
                button.Text = "X";
                button.ForeColor = Color.Red;
            }
            else
            {
                button.Text = "O";
                button.ForeColor = Color.Blue;
            }

            if (CheckForWinner())
            {
                gameOver = true;
                if (playerTurn)
                {
                    xWins++;
                    statusLabel.Text = "Player X wins!";
                }
                else
                {
                    oWins++;
                    statusLabel.Text = "Player O wins!";
                }
                HighlightWinningCells();
            }
            else if (IsBoardFull())
            {
                gameOver = true;
                draws++;
                statusLabel.Text = "It's a draw!";
            }
            else
            {
                playerTurn = !playerTurn;
                statusLabel.Text = playerTurn ? "Player X's turn" : "Player O's turn";

                if (vsComputer && !playerTurn && !gameOver)
                {
                    ComputerMove();
                }
            }

            scoreLabel.Text = $"X: {xWins} | O: {oWins} | Draws: {draws}";
        }

        private void ComputerMove()
        {
            // Simple AI - first checks for winning move, then blocking move, then random
            // Check for winning move
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (buttons[i, j].Text == "")
                    {
                        buttons[i, j].Text = "O";
                        if (CheckForWinner())
                        {
                            buttons[i, j].ForeColor = Color.Blue;
                            if (CheckForWinner())
                            {
                                gameOver = true;
                                oWins++;
                                statusLabel.Text = "Computer (O) wins!";
                                HighlightWinningCells();
                            }
                            else if (IsBoardFull())
                            {
                                gameOver = true;
                                draws++;
                                statusLabel.Text = "It's a draw!";
                            }
                            else
                            {
                                playerTurn = !playerTurn;
                                statusLabel.Text = "Player X's turn";
                            }
                            scoreLabel.Text = $"X: {xWins} | O: {oWins} | Draws: {draws}";
                            return;
                        }
                        else
                        {
                            buttons[i, j].Text = "";
                        }
                    }
                }
            }

            // Check for blocking move
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (buttons[i, j].Text == "")
                    {
                        buttons[i, j].Text = "X";
                        if (CheckForWinner())
                        {
                            buttons[i, j].Text = "O";
                            buttons[i, j].ForeColor = Color.Blue;
                            playerTurn = !playerTurn;
                            statusLabel.Text = "Player X's turn";
                            scoreLabel.Text = $"X: {xWins} | O: {oWins} | Draws: {draws}";
                            return;
                        }
                        else
                        {
                            buttons[i, j].Text = "";
                        }
                    }
                }
            }

            // Random move
            Random random = new Random();
            int x, y;
            do
            {
                x = random.Next(0, 3);
                y = random.Next(0, 3);
            } while (buttons[x, y].Text != "");

            buttons[x, y].Text = "O";
            buttons[x, y].ForeColor = Color.Blue;

            if (CheckForWinner())
            {
                gameOver = true;
                oWins++;
                statusLabel.Text = "Computer (O) wins!";
                HighlightWinningCells();
            }
            else if (IsBoardFull())
            {
                gameOver = true;
                draws++;
                statusLabel.Text = "It's a draw!";
            }
            else
            {
                playerTurn = !playerTurn;
                statusLabel.Text = "Player X's turn";
            }
            scoreLabel.Text = $"X: {xWins} | O: {oWins} | Draws: {draws}";
        }

        private bool CheckForWinner()
        {
            // Check rows
            for (int i = 0; i < 3; i++)
            {
                if (buttons[i, 0].Text != "" && buttons[i, 0].Text == buttons[i, 1].Text && buttons[i, 1].Text == buttons[i, 2].Text)
                {
                    return true;
                }
            }

            // Check columns
            for (int j = 0; j < 3; j++)
            {
                if (buttons[0, j].Text != "" && buttons[0, j].Text == buttons[1, j].Text && buttons[1, j].Text == buttons[2, j].Text)
                {
                    return true;
                }
            }

            // Check diagonals
            if (buttons[0, 0].Text != "" && buttons[0, 0].Text == buttons[1, 1].Text && buttons[1, 1].Text == buttons[2, 2].Text)
            {
                return true;
            }

            if (buttons[0, 2].Text != "" && buttons[0, 2].Text == buttons[1, 1].Text && buttons[1, 1].Text == buttons[2, 0].Text)
            {
                return true;
            }

            return false;
        }

        private bool IsBoardFull()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (buttons[i, j].Text == "")
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void HighlightWinningCells()
        {
            // Check rows
            for (int i = 0; i < 3; i++)
            {
                if (buttons[i, 0].Text != "" && buttons[i, 0].Text == buttons[i, 1].Text && buttons[i, 1].Text == buttons[i, 2].Text)
                {
                    buttons[i, 0].BackColor = Color.LightGreen;
                    buttons[i, 1].BackColor = Color.LightGreen;
                    buttons[i, 2].BackColor = Color.LightGreen;
                    return;
                }
            }

            // Check columns
            for (int j = 0; j < 3; j++)
            {
                if (buttons[0, j].Text != "" && buttons[0, j].Text == buttons[1, j].Text && buttons[1, j].Text == buttons[2, j].Text)
                {
                    buttons[0, j].BackColor = Color.LightGreen;
                    buttons[1, j].BackColor = Color.LightGreen;
                    buttons[2, j].BackColor = Color.LightGreen;
                    return;
                }
            }

            // Check diagonals
            if (buttons[0, 0].Text != "" && buttons[0, 0].Text == buttons[1, 1].Text && buttons[1, 1].Text == buttons[2, 2].Text)
            {
                buttons[0, 0].BackColor = Color.LightGreen;
                buttons[1, 1].BackColor = Color.LightGreen;
                buttons[2, 2].BackColor = Color.LightGreen;
                return;
            }

            if (buttons[0, 2].Text != "" && buttons[0, 2].Text == buttons[1, 1].Text && buttons[1, 1].Text == buttons[2, 0].Text)
            {
                buttons[0, 2].BackColor = Color.LightGreen;
                buttons[1, 1].BackColor = Color.LightGreen;
                buttons[2, 0].BackColor = Color.LightGreen;
                return;
            }
        }

        private void ResetGame()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    buttons[i, j].Text = "";
                    buttons[i, j].BackColor = SystemColors.Control;
                    buttons[i, j].Enabled = true;
                }
            }
            playerTurn = true;
            gameOver = false;
            statusLabel.Text = "Player X's turn";
        }

        private void restartButton_Click(object sender, EventArgs e)
        {
            ResetGame();
        }

        private void vsComputerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            vsComputer = true;
            ResetGame();
            playerVsPlayerToolStripMenuItem.Checked = false;
            vsComputerToolStripMenuItem.Checked = true;
        }

        private void playerVsPlayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            vsComputer = false;
            ResetGame();
            playerVsPlayerToolStripMenuItem.Checked = true;
            vsComputerToolStripMenuItem.Checked = false;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Tic Tac Toe Game\nVersion 1.0\nDeveloped by Your Name", "About");
        }
    }

    // Designer generated code (partial class)
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Label scoreLabel;
        private System.Windows.Forms.Button restartButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem gameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playerVsPlayerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vsComputerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.statusLabel = new System.Windows.Forms.Label();
            this.scoreLabel = new System.Windows.Forms.Label();
            this.restartButton = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.gameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playerVsPlayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vsComputerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLabel.Location = new System.Drawing.Point(12, 30);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(129, 20);
            this.statusLabel.TabIndex = 0;
            this.statusLabel.Text = "Player X\'s turn";
            // 
            // scoreLabel
            // 
            this.scoreLabel.AutoSize = true;
            this.scoreLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scoreLabel.Location = new System.Drawing.Point(200, 32);
            this.scoreLabel.Name = "scoreLabel";
            this.scoreLabel.Size = new System.Drawing.Size(110, 17);
            this.scoreLabel.TabIndex = 1;
            this.scoreLabel.Text = "X: 0 | O: 0 | D: 0";
            // 
            // restartButton
            // 
            this.restartButton.Location = new System.Drawing.Point(400, 30);
            this.restartButton.Name = "restartButton";
            this.restartButton.Size = new System.Drawing.Size(75, 23);
            this.restartButton.TabIndex = 2;
            this.restartButton.Text = "Restart";
            this.restartButton.UseVisualStyleBackColor = true;
            this.restartButton.Click += new System.EventHandler(this.restartButton_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gameToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(484, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // gameToolStripMenuItem
            // 
            this.gameToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playerVsPlayerToolStripMenuItem,
            this.vsComputerToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.gameToolStripMenuItem.Name = "gameToolStripMenuItem";
            this.gameToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.gameToolStripMenuItem.Text = "Game";
            // 
            // playerVsPlayerToolStripMenuItem
            // 
            this.playerVsPlayerToolStripMenuItem.Checked = true;
            this.playerVsPlayerToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.playerVsPlayerToolStripMenuItem.Name = "playerVsPlayerToolStripMenuItem";
            this.playerVsPlayerToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.playerVsPlayerToolStripMenuItem.Text = "Player vs Player";
            this.playerVsPlayerToolStripMenuItem.Click += new System.EventHandler(this.playerVsPlayerToolStripMenuItem_Click);
            // 
            // vsComputerToolStripMenuItem
            // 
            this.vsComputerToolStripMenuItem.Name = "vsComputerToolStripMenuItem";
            this.vsComputerToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.vsComputerToolStripMenuItem.Text = "vs Computer";
            this.vsComputerToolStripMenuItem.Click += new System.EventHandler(this.vsComputerToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 361);
            this.Controls.Add(this.restartButton);
            this.Controls.Add(this.scoreLabel);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximumSize = new System.Drawing.Size(500, 400);
            this.MinimumSize = new System.Drawing.Size(500, 400);
            this.Name = "MainForm";
            this.Text = "Tic Tac Toe";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
