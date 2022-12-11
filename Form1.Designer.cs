
namespace WindowsFormsApp1
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.StartButton = new System.Windows.Forms.Button();
            this.ShowControls = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.TimerText = new System.Windows.Forms.Label();
            this.ScoreLabel = new System.Windows.Forms.Label();
            this.CurrentScore = new System.Windows.Forms.Label();
            this.LevelLabel = new System.Windows.Forms.Label();
            this.CurrentLevel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // StartButton
            // 
            this.StartButton.BackColor = System.Drawing.Color.Transparent;
            this.StartButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartButton.Location = new System.Drawing.Point(13, 834);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(166, 50);
            this.StartButton.TabIndex = 0;
            this.StartButton.Text = "Start Game";
            this.StartButton.UseVisualStyleBackColor = false;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // ShowControls
            // 
            this.ShowControls.BackColor = System.Drawing.Color.Transparent;
            this.ShowControls.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShowControls.Location = new System.Drawing.Point(13, 765);
            this.ShowControls.Name = "ShowControls";
            this.ShowControls.Size = new System.Drawing.Size(166, 50);
            this.ShowControls.TabIndex = 1;
            this.ShowControls.Text = "Controls";
            this.ShowControls.UseVisualStyleBackColor = false;
            this.ShowControls.Click += new System.EventHandler(this.ShowControls_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label1.Location = new System.Drawing.Point(355, 99);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(375, 37);
            this.label1.TabIndex = 2;
            this.label1.Text = "WinForms Tetris Attack";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TimerText
            // 
            this.TimerText.AutoSize = true;
            this.TimerText.BackColor = System.Drawing.Color.Transparent;
            this.TimerText.Font = new System.Drawing.Font("Consolas", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TimerText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.TimerText.Location = new System.Drawing.Point(74, 123);
            this.TimerText.Name = "TimerText";
            this.TimerText.Size = new System.Drawing.Size(232, 56);
            this.TimerText.TabIndex = 4;
            this.TimerText.Text = "00:00:00";
            // 
            // ScoreLabel
            // 
            this.ScoreLabel.AutoSize = true;
            this.ScoreLabel.BackColor = System.Drawing.Color.Transparent;
            this.ScoreLabel.Font = new System.Drawing.Font("Consolas", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ScoreLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ScoreLabel.Location = new System.Drawing.Point(782, 123);
            this.ScoreLabel.Name = "ScoreLabel";
            this.ScoreLabel.Size = new System.Drawing.Size(180, 56);
            this.ScoreLabel.TabIndex = 5;
            this.ScoreLabel.Text = "Score:";
            // 
            // CurrentScore
            // 
            this.CurrentScore.AutoSize = true;
            this.CurrentScore.BackColor = System.Drawing.Color.Transparent;
            this.CurrentScore.Font = new System.Drawing.Font("Consolas", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CurrentScore.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.CurrentScore.Location = new System.Drawing.Point(774, 193);
            this.CurrentScore.Name = "CurrentScore";
            this.CurrentScore.Size = new System.Drawing.Size(175, 34);
            this.CurrentScore.TabIndex = 6;
            this.CurrentScore.Text = "0000000000";
            // 
            // LevelLabel
            // 
            this.LevelLabel.AutoSize = true;
            this.LevelLabel.BackColor = System.Drawing.Color.Transparent;
            this.LevelLabel.Font = new System.Drawing.Font("Consolas", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LevelLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.LevelLabel.Location = new System.Drawing.Point(782, 332);
            this.LevelLabel.Name = "LevelLabel";
            this.LevelLabel.Size = new System.Drawing.Size(206, 56);
            this.LevelLabel.TabIndex = 7;
            this.LevelLabel.Text = "Level: ";
            // 
            // CurrentLevel
            // 
            this.CurrentLevel.AutoSize = true;
            this.CurrentLevel.BackColor = System.Drawing.Color.Transparent;
            this.CurrentLevel.Font = new System.Drawing.Font("Consolas", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CurrentLevel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.CurrentLevel.Location = new System.Drawing.Point(832, 408);
            this.CurrentLevel.Name = "CurrentLevel";
            this.CurrentLevel.Size = new System.Drawing.Size(50, 56);
            this.CurrentLevel.TabIndex = 8;
            this.CurrentLevel.Text = "1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GrayText;
            this.BackgroundImage = global::WindowsFormsApp1.Properties.Resources._1p_background_1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1024, 896);
            this.Controls.Add(this.CurrentLevel);
            this.Controls.Add(this.LevelLabel);
            this.Controls.Add(this.CurrentScore);
            this.Controls.Add(this.ScoreLabel);
            this.Controls.Add(this.TimerText);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ShowControls);
            this.Controls.Add(this.StartButton);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tetris Attack Clone";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Button ShowControls;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label TimerText;
        private System.Windows.Forms.Label ScoreLabel;
        private System.Windows.Forms.Label CurrentScore;
        private System.Windows.Forms.Label LevelLabel;
        private System.Windows.Forms.Label CurrentLevel;
    }
}

