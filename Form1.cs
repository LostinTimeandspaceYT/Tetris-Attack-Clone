using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        System.Windows.Forms.Timer graphicsTimer;
        Stopwatch Stopwatch;
        GameLoop gameLoop = null;
        Game myGame;

        public Form1()
        {
            InitializeComponent();
            TimerText.Hide();
            CurrentLevel.Hide();
            CurrentScore.Hide();
            LevelLabel.Hide();
            ScoreLabel.Hide();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (gameLoop == null)
            {
                Rectangle resolution = Screen.PrimaryScreen.Bounds;

                // Initialize Game
                myGame = new Game();
                myGame.Resolution = new Size(resolution.Width, resolution.Height);

                // Initialize & Start GameLoop
                gameLoop = new GameLoop();
                gameLoop.Load(myGame);
                gameLoop.Start();

                // Start Graphics Timer
                graphicsTimer.Start();
                gameLoop.Draw(e.Graphics);
            }
            else
            {
                // Draw game graphics on Form1
                gameLoop.Draw(e.Graphics);
            }
        }

        private void GraphicsTimer_Tick(object sender, EventArgs e)
        {
            TimerText.Text = Stopwatch.Elapsed.ToString(@"hh\:mm\:ss");
            CurrentScore.Text = myGame.CurrentScore.ToString();
            CurrentLevel.Text = myGame.CurrentLevel.ToString();
            
            // Refresh Form1 graphics
            Invalidate();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            Rectangle resolution = Screen.PrimaryScreen.Bounds;

            // Initialize Game
            myGame = new Game();
            myGame.Resolution = new Size(resolution.Width, resolution.Height);

            // Initialize & Start GameLoop
            gameLoop = new GameLoop();
            gameLoop.Load(myGame);
            gameLoop.Start();

            // Initialize graphicsTimer
            graphicsTimer = new System.Windows.Forms.Timer();
            graphicsTimer.Interval = 1000 / 120;
            graphicsTimer.Tick += GraphicsTimer_Tick;
            graphicsTimer.Start();

            // Initialize Paint Event
            Paint += Form1_Paint;

            //Start the stopwatch
            Stopwatch = new Stopwatch();
            Stopwatch.Start();

            ScoreLabel.Show();
            TimerText.Show();
            CurrentLevel.Show();
            CurrentScore.Show();
            LevelLabel.Show();
            ScoreLabel.Show();

            label1.Dispose();
            StartButton.Dispose();
            ShowControls.Dispose();
        }

        private void ShowControls_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Movement:\n\nW: move up\nS: move down\nA: move left\nD: move right\n\nSpace: Swap selected tiles","Basic Controls", MessageBoxButtons.OK);
        }
    }
}
