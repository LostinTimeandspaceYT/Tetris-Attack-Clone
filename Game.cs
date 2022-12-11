/*
 * Name: Nathan Winslow
 * Class: Real Time Interfacing - Fall 2022
 * Instructor: Professor Gitlitz
 * Project: Tetris Attack Clone
 * Constrants/Requirements: Must use multithreading
 * 
 * Credits:
 *  https://www.codementor.io/@dewetvanthomas/tutorial-game-loop-for-c-128ovxgrig for starter code
 *  https://gist.github.com/markheath/8783999 - Audio Playback Engine
 *  https://www.markheath.net/post/looped-playback-in-net-with-naudio - looping audio playback
 *
 ********* TO DO: *********
 * 
 * 1. 
 * 2. 
 *
 ******* LONG TERM: *******
 * 
 * a. Create a Player class for Two Player Modes
 * b. Create a GameMode class
 * c. Create a start menu
 * d. Create a pause menu
 * 
 * 
 */


/* Using the known universe */
using System;
using System.Drawing;
using System.Windows.Input;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Media;
using NAudio;
using NAudio.Wave;

public enum COLORS
{
    EMPTY = 0,
    BLUE = 1,
    GREEN = 2,
    LBLUE = 3,
    PURPLE = 4,
    RED = 5,
    YELLOW = 6,
    GRAY = 7
}

namespace WindowsFormsApp1
{

    class Game
    {

        //Zone on the screen the block selector is permitted to move
        private const int LEFTBOUND = 345;
        private const int RIGHTBOUND = 601;
        private const int UPPERBOUND = 100;
        private const int LOWERBOUND = 768;

        //To build the board
        private const int ROWS = 12;
        private const int COLOUMNS = 6;
        private GameSprite[,] Player1Board;
        private GameSprite[] Player1NextRow;

        private static bool Player1BoardIsFull;
        private static bool Player1IsInDanger;
        public bool GameOver;

        //For score keeping
        private const int BASEPOINTS = 10;
        public int CurrentScore = 0;
        public int CurrentLevel = 1;

        //misc properties & flags
        private int PixelScaler = 4;
        public double AddingRowTime;

        //Endless mode sprites
        private GameSprite WordTime;
        private GameSprite SinglePlayerBackground;
        public GameSprite Player1_BlockSelector;

        //Sound Related
        private AudioPlaybackEngine AudioMixer;
        private CachedSound SwapSound;
        private WaveOut waveOut;
        private WaveFileReader reader;
        private bool replay;

        //Threads 
        Thread Dj;
        Thread BoardManager;
        Thread LevelCounter;
        Mutex mutex;


        public Size Resolution { get; set; }
        
        public void Load()
        {

            // Create the audio objects
            AudioMixer = new AudioPlaybackEngine();
            SwapSound = new CachedSound(@"..\slip.wav");

            // Create new sprites
            Player1_BlockSelector = new GameSprite();
            SinglePlayerBackground = new GameSprite();
            WordTime = new GameSprite();

            // Load sprite images
            Player1_BlockSelector.SpriteImage = Properties.Resources.block_selector;
            SinglePlayerBackground.SpriteImage = Properties.Resources._1p_background;
            WordTime.SpriteImage = Properties.Resources.time;

            // Set sprite height & width in pixels
            Player1_BlockSelector.Width = Player1_BlockSelector.SpriteImage.Width * PixelScaler;
            Player1_BlockSelector.Height = Player1_BlockSelector.SpriteImage.Height * PixelScaler;
            SinglePlayerBackground.Width = SinglePlayerBackground.SpriteImage.Width * PixelScaler;
            SinglePlayerBackground.Height = SinglePlayerBackground.SpriteImage.Height * PixelScaler;
            WordTime.Width = WordTime.SpriteImage.Width * PixelScaler;
            WordTime.Height = WordTime.SpriteImage.Height * PixelScaler;

            // Set sprite coodinates
            Player1_BlockSelector.X = 473;
            Player1_BlockSelector.Y = 601;
            SinglePlayerBackground.X = 0;
            SinglePlayerBackground.Y = 0;
            WordTime.X = LEFTBOUND - 264;
            WordTime.Y = UPPERBOUND - 15;

            //Generate the initial board and starting conditions
            Player1Board = CreateStartingBoard();
            AddingRowTime = 0;
            Player1IsInDanger = false;

            //Start Threads
            Dj = new Thread(new ThreadStart(MixSongs));
            BoardManager = new Thread(new ThreadStart(Player1CheckforCombos));
            LevelCounter = new Thread(new ThreadStart(CountLevels));
            Dj.Start();
            BoardManager.Start();
            LevelCounter.Start();
            mutex = new Mutex();

            //set flags
            replay = false;
            GameOver = false;
        }

        /// <summary>
        /// Generate a row of random blocks in player 1's board
        /// Note: if configuring for 2-player, 
        /// this method would need block X coordinate instansiation reimplemented
        /// </summary>
        /// <returns>A row of blocks</returns>
        public GameSprite[] GenerateRowOfBlocks()
        {    
            GameSprite[] blocks = new GameSprite[6];
            int spacing = 64;
            bool twoinarow = false;
            int prevblock = -1;
            for (int i = 0; i < 6; ++i)
            {
                //Random is too fast, so we need to slow it down
                Thread.Sleep(1);
                Random rand = new Random();
                blocks[i] = new GameSprite();
                //Make 7 the rarest block to spawn
                int block = rand.Next(1,7); 
                while (twoinarow)
                {
                    block = rand.Next(1,8);
                    if (block != prevblock)
                    {
                        twoinarow = false;
                    }
                }
                if (block == 1)
                {
                    blocks[i].SpriteImage = Properties.Resources.blue_block_1;
                    blocks[i].Color = (int)COLORS.BLUE;
                }
                if (block == 2)
                {
                    blocks[i].SpriteImage = Properties.Resources.green_block_1;
                    blocks[i].Color = (int)COLORS.GREEN;
                }
                if (block == 3)
                {
                    blocks[i].SpriteImage = Properties.Resources.lightblue_block_1;
                    blocks[i].Color = (int)COLORS.LBLUE;
                }
                if (block == 4)
                {
                    blocks[i].SpriteImage = Properties.Resources.pruple_block_1;
                    blocks[i].Color = (int)COLORS.PURPLE;
                }
                if (block == 5)
                {
                    blocks[i].SpriteImage = Properties.Resources.red_block_1;
                    blocks[i].Color = (int)COLORS.RED;
                }
                if (block == 6)
                {
                    blocks[i].SpriteImage = Properties.Resources.yellow_block_1;
                    blocks[i].Color = (int)COLORS.YELLOW;
                }
                if (block == 7)
                {
                    blocks[i].SpriteImage = Properties.Resources.gray_block_1;
                    blocks[i].Color = (int)COLORS.GRAY;
                    prevblock = block; //so we only spawn one per row.
                }

                blocks[i].Width = blocks[i].SpriteImage.Width * PixelScaler;
                blocks[i].Height = blocks[i].SpriteImage.Height * PixelScaler;
                blocks[i].X = 288 + (spacing * (i + 1));
                if (block == prevblock)
                {
                    twoinarow = true;
                }
                else
                {
                    twoinarow = false;
                }
                prevblock = block;
            }
            return blocks;
        }

        /// <summary>
        /// shifts the board up by one row and adds the newrow
        /// to the bottom of the board
        /// </summary>
        /// <param name="board">player 1 or player 2</param>
        public void AddRow(GameSprite[,] board, GameSprite[] newrow)
        {
            bool isEmpty = true;
            for (int a=0; a < COLOUMNS; a++)
            {
                if (board[ROWS -1, a] != null) isEmpty = false;
            }

            //shifts the rows up by one
            // top left of the board is 0,0
            // bot right of the board is 11,5
            if (!isEmpty)
            {

                // check the first three rows to see if there are tiles
                // if so, set the appropiate flags.
                for (int b=0; b< COLOUMNS; ++b)
                {
                    if (board[0, b] != null) Player1BoardIsFull = true;
                    
                    if (Player1BoardIsFull) return;
                }

                for (int i = 0; i <ROWS -1 ; ++i)
                {
                    for (int j = 0; j < COLOUMNS; ++j)
                    {
                        board[i, j] = board[i + 1, j];
                        if (board[i ,j] != null) board[i, j].Y -= 64;

                    }
                }

                //clear the bottom row
                for (int h = 0; h < COLOUMNS; h++)
                {
                    board[ROWS - 1, h] = null;
                }
            }

            //add new row to the bottom of the board. 
            for (int h=0; h < COLOUMNS; h++)
            {
                board[ROWS -1, h] = newrow[h];
                board[ROWS -1 , h].Y = 800; 
            }
        }

        public GameSprite[,] CreateStartingBoard()
        {
            GameSprite[,] StartingBoard = new GameSprite[ROWS, COLOUMNS];
            int rowcount = 6;
            for (int i = 0; i < rowcount; ++i)
            {
                Player1NextRow = new GameSprite[6];
                Player1NextRow = GenerateRowOfBlocks();
                AddRow(StartingBoard, Player1NextRow);
            }

            Thread.Sleep(1);
            Random rand = new Random();
            int randnumofrows = rand.Next(9, ROWS -1); 
            Thread.Sleep(1);
            int randcoloum = rand.Next(1, COLOUMNS -1);

            for (int h = 0; h < randnumofrows; ++h)
            {
                StartingBoard[h, randcoloum] = null;
            }
            
            //TODO: Add additional logic to ensure 3 blocks
            //      of the same color don't spawn in same coloumn

            return StartingBoard;
        }


        public void Unload()
        {

            // Unload graphics
            Player1Board = null;
            WordTime = null;
            SinglePlayerBackground = null;


            //Dispose of Audio
            AudioMixer.Dispose();
            if(waveOut != null)
            {
                waveOut.Dispose();
            }

            //join threads
            Dj.Join();
            BoardManager.Join();
            mutex.Dispose();
        }

        public void Update(TimeSpan gameTime)
        {

            AddingRowTime += gameTime.TotalMilliseconds / 1000;
            int TargetTime = 3 - (CurrentLevel * CurrentLevel);
            if (TargetTime == 0) TargetTime = 1;
            if (AddingRowTime >= TargetTime)
            {
                Player1NextRow = GenerateRowOfBlocks();
                AddRow(Player1Board, Player1NextRow);
                if (!Player1BoardIsFull) Player1_BlockSelector.Y -= 64; //to ensure the player is on the same row
                if (Player1BoardIsFull) GameOver = true; //flag to end the game
                AddingRowTime = 0;
            }
            MovePlayer1();
            SwapPlayer1Blocks();
            for (int b=0; b < COLOUMNS; ++b)
            {
                if (Player1Board[1, b] != null || Player1Board[2, b] != null || Player1Board[3, b] != null) Player1IsInDanger = true;

                if (Player1Board[0, b] == null && Player1Board[1, b] == null && Player1Board[2, b] == null && Player1Board[3, b] == null) Player1IsInDanger = false;
            }
        }

        public void Draw(Graphics gfx)
        {
            if (!GameOver)
            {
                SinglePlayerBackground.Draw(gfx);
                WordTime.Draw(gfx);

                for (int i=0; i < 12; ++i)
                {
                    for (int j=0; j < 6; ++j)
                    {
                        Player1Board[i,j]?.Draw(gfx);
                    }
                }
                Player1_BlockSelector.Draw(gfx);
            }
        }

        public void MovePlayer1()
        {
            int moveDistance = 64;

            // Move player sprite when WASD keys are pressed 
            if ((Keyboard.GetKeyStates(Key.D) & KeyStates.Down) > 0 && Player1_BlockSelector.X < RIGHTBOUND)
            {
                Player1_BlockSelector.X += moveDistance;
                Thread.Sleep(75);
            }
            else if ((Keyboard.GetKeyStates(Key.A) & KeyStates.Down) > 0 && Player1_BlockSelector.X > LEFTBOUND)
            {
                Player1_BlockSelector.X -= moveDistance;
                Thread.Sleep(75);
            }
            else if ((Keyboard.GetKeyStates(Key.W) & KeyStates.Down) > 0 && Player1_BlockSelector.Y > UPPERBOUND)
            {
                Player1_BlockSelector.Y -= moveDistance;
                Thread.Sleep(75);
            }
            else if ((Keyboard.GetKeyStates(Key.S) & KeyStates.Down) > 0 && Player1_BlockSelector.Y < LOWERBOUND)
            {
                Player1_BlockSelector.Y += moveDistance;
                Thread.Sleep(75);
            }
        }

        public void SwapPlayer1Blocks()
        {
            if ((Keyboard.GetKeyStates(Key.Space) & KeyStates.Down) > 0)
            {
                AudioMixer.PlaySound(SwapSound);
                for(int i = ROWS -1; i >=0; --i)
                {
                    for (int j=0; j < COLOUMNS -1; ++j)
                    {
                        if (Player1Board[i,j]!= null)
                        {
                            //Note: we don't want to change the position of the indicies, we just want to change their color
                            if ((Player1Board[i,j].X == (Player1_BlockSelector.X + 7) && Player1Board[i,j].Y == (Player1_BlockSelector.Y +7)) && Player1Board[i,j+1] != null)
                            {
                                //make a copy of the left block
                                GameSprite tmpblockL = new GameSprite();
                                tmpblockL.SpriteImage = Player1Board[i, j].SpriteImage;
                                tmpblockL.Color = Player1Board[i, j].Color;
                                //change the left block to the right block
                                if (Player1Board[i,j] != null && Player1Board[i,j+1] != null)
                                {
                                    Player1Board[i, j].SpriteImage = Player1Board[i,j+1].SpriteImage;
                                    Player1Board[i, j].Color = Player1Board[i, j + 1].Color;
                                }
                                //make the right block the copy of the left block
                                if (tmpblockL != null && Player1Board[i,j+1] != null)
                                {
                                    Player1Board[i, j + 1].SpriteImage = tmpblockL.SpriteImage;
                                    Player1Board[i, j+1].Color = tmpblockL.Color;
                                }
                                //Sleep the thread used for handling control inputs
                                //Set flag for board state manager thread to check the board
                                Thread.Sleep(150);
                                break;
                            }
                        }
                        //If the Left block is null and the right is not
                        else if (Player1Board[i,j] == null && (Player1Board[i,j+1]?.Y == Player1_BlockSelector.Y +7) && (Player1Board[i, j + 1]?.X == Player1_BlockSelector.X + 71))
                        {
                            GameSprite tmpblockR = new GameSprite();
                            tmpblockR.SpriteImage = Player1Board[i, j + 1].SpriteImage;
                            tmpblockR.Color = Player1Board[i, j + 1].Color;
                            tmpblockR.Y = Player1Board[i, j + 1].Y;
                            tmpblockR.X = Player1Board[i, j + 1].X - 64; //so it's in the correct position
                            tmpblockR.Height = Player1Board[i, j + 1].Height;
                            tmpblockR.Width = Player1Board[i, j + 1].Width;

                            Player1Board[i, j + 1].Color = (int)COLORS.EMPTY;
                            Player1Board[i, j + 1] = null;
                            Player1Board[i, j] = tmpblockR;
                            Thread.Sleep(150);
                            break;
                        }
                        //If the right block is null and the left is not
                        if (Player1Board[i, j+1] == null && (Player1Board[i, j]?.Y == Player1_BlockSelector.Y + 7) && (Player1Board[i, j]?.X == Player1_BlockSelector.X + 7))
                        {
                            GameSprite tmpblock = new GameSprite();
                            tmpblock.SpriteImage = Player1Board[i, j].SpriteImage;
                            tmpblock.Color = Player1Board[i, j].Color;
                            tmpblock.Y = Player1Board[i, j].Y;
                            tmpblock.X = Player1Board[i, j].X + 64 ; //so it's in the correct position
                            tmpblock.Height = Player1Board[i, j].Height;
                            tmpblock.Width = Player1Board[i, j].Width;

                            Player1Board[i, j].Color = (int)COLORS.EMPTY;
                            Player1Board[i, j] = null;
                            Player1Board[i, j+1] = tmpblock;
                            Thread.Sleep(150);
                            break;
                        }
                    }
                }
            } //endif
        } 

        private void CheckRows()
        {
            for (int a=0; a < ROWS; ++a)
            {
                for (int b=0; b < COLOUMNS; ++b)
                {
                    int ComboRowCount = 0;
                    if (Player1Board[a, b] != null)
                    {
                        int j;

                        //Check the Row for matching colors
                        for (j = b; j < COLOUMNS; ++j)
                        {
                            if (Player1Board[a, j]?.Color != Player1Board[a, b]?.Color) break;
                            if (Player1Board[a, j]?.Color == Player1Board[a, b].Color)
                            {
                                Player1Board[a, j].Matches = true;
                                ComboRowCount++;
                            }
                        }

                        //If no combos were found, reset the flag for proper score keeping
                        if (ComboRowCount < 3)
                        {
                            for (int w = 0; w < COLOUMNS; ++w)
                            {
                                if (Player1Board[a, w] != null) Player1Board[a, w].Matches = false;
                            }
                        }

                        //The Player got a horizontal combo
                        if (ComboRowCount >= 3)
                        {
                            for (int d = 0; d < COLOUMNS; ++d)
                            {
                                if (Player1Board[a, d] != null && Player1Board[a, d].Matches)
                                {
                                    Player1Board[a, d].Color = (int)COLORS.EMPTY;
                                    Player1Board[a, d] = null;
                                }
                            }

                            CurrentScore += (ComboRowCount) * BASEPOINTS;
                        }
                    }
                }
            }
        }

        private void CheckCols()
        {
            for (int a = 0; a < ROWS - 1; ++a)
            {
                for (int b = 0; b < COLOUMNS; ++b)
                {
                    int ComboColoumnCount = 0;
                    if (Player1Board[a, b] != null)
                    {
                        int i;
                        //Check the Coloumn for matching colors
                        for (i = a; i < ROWS; ++i)
                        {
                            if (Player1Board[i, b]?.Color != Player1Board[a, b].Color) break;
                            if (Player1Board[i, b]?.Color == Player1Board[a, b].Color)
                            {
                                Player1Board[i, b].Matches = true;
                                ComboColoumnCount++;
                            }
                        }
                        if (ComboColoumnCount < 3)
                        {
                            foreach (GameSprite block in Player1Board)
                            {
                                if (block != null) block.Matches = false;
                            }
                        }

                        //if the player got a combo
                        if (ComboColoumnCount >= 3)
                        {
                            for (int c = a; c < i; ++c)
                            {
                                for (int d = 0; d < COLOUMNS; ++d)
                                {
                                    if (Player1Board[c, d] != null && Player1Board[c, d].Matches)
                                    {
                                        //clear the matching tiles
                                        Player1Board[c, d].Color = (int)COLORS.EMPTY;
                                        Player1Board[c, d] = null;
                                    }
                                }
                            }
                            CurrentScore += ComboColoumnCount * BASEPOINTS;
                        }
                    }
                }
            }
        }

        //Threaded function
        private void Player1CheckforCombos()
        {
            while(!GameOver)
            {
                mutex.WaitOne();
                CheckCols();
                CheckRows();
                for (int a=0; a <ROWS-1; ++a)
                {
                    for (int b= 0; b < COLOUMNS; ++b)
                    {
                        // Move any floating blocks down the board
                        if (Player1Board[a,b] != null && Player1Board[a+1,b] == null)
                        {
                            //make the block below equal to the current block
                            Player1Board[a + 1, b] = new GameSprite();
                            Player1Board[a + 1,b].SpriteImage = Player1Board[a, b].SpriteImage;
                            Player1Board[a + 1, b].Color = Player1Board[a, b].Color;
                            Player1Board[a + 1, b].X = Player1Board[a, b].X;
                            Player1Board[a + 1, b].Y = Player1Board[a, b].Y + 64;
                            Player1Board[a + 1, b].Height = Player1Board[a, b].Height;
                            Player1Board[a + 1, b].Width = Player1Board[a, b].Width;

                            //null the original block
                            Player1Board[a, b].Color = (int)COLORS.EMPTY;
                            Player1Board[a, b] = null;
                        }
                    }
                }
                mutex.ReleaseMutex();
                Thread.Sleep(10);
            }
        }

        //Threaded Function
        public void MixSongs()
        {
            while (!GameOver)
            {
                if (waveOut == null)
                {
                    reader = new WaveFileReader(@"..\blaze-stage.wav");
                    LoopStream loop = new LoopStream(reader);
                    waveOut = new WaveOut();
                    waveOut.Init(loop);
                    waveOut.Play();
                }
                if (Player1IsInDanger && !replay)
                {
                    //Stop the Stage Music
                    waveOut.Stop();
                    waveOut.Dispose();
                    reader = null;
                    reader = new WaveFileReader(@"..\blaze-danger.wav");
                    LoopStream loop = new LoopStream(reader);
                    waveOut = new WaveOut();
                    waveOut.Init(loop);
                    waveOut.Play();
                    replay = true;
                }
                if (waveOut != null && !Player1IsInDanger && replay)
                {
                    waveOut.Stop();
                    waveOut.Dispose();
                    reader = null;
                    reader = new WaveFileReader(@"..\blaze-stage.wav");
                    LoopStream loop = new LoopStream(reader);
                    waveOut = new WaveOut();
                    waveOut.Init(loop);
                    waveOut.Play();
                    replay = false;
                }
                Thread.Sleep(10);
            }
        }

        //Threaded Function
        public void CountLevels()
        {
            while (!GameOver)
            {
                mutex.WaitOne();
                int ScoreToLevelTwo = 500;
                int ScoreToLevelThree = 1000;
                int ScoreToLevelFour = 2500;
                if (CurrentScore >= ScoreToLevelTwo && CurrentLevel < 2) CurrentLevel++;
                if (CurrentScore >= ScoreToLevelThree && CurrentLevel < 3) CurrentLevel++;
                if (CurrentScore >= ScoreToLevelFour && CurrentLevel < 4) CurrentLevel++;
                mutex.ReleaseMutex();
                Thread.Sleep(1000);
            }
        }
    }
}