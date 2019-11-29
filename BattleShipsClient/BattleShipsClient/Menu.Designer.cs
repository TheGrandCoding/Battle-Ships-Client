namespace BattleShipsClient
{
    partial class Menu
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
            this.JoinPNL = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.Label7 = new System.Windows.Forms.Label();
            this.NewGameName = new System.Windows.Forms.TextBox();
            this.Refresh = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.CurrentGames = new System.Windows.Forms.ListBox();
            this.JoinGame = new System.Windows.Forms.Button();
            this.StartNewGame = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.JoinPNL.SuspendLayout();
            this.SuspendLayout();
            // 
            // JoinPNL
            // 
            this.JoinPNL.Controls.Add(this.button2);
            this.JoinPNL.Controls.Add(this.listBox1);
            this.JoinPNL.Controls.Add(this.Label7);
            this.JoinPNL.Controls.Add(this.NewGameName);
            this.JoinPNL.Controls.Add(this.Refresh);
            this.JoinPNL.Controls.Add(this.label1);
            this.JoinPNL.Controls.Add(this.CurrentGames);
            this.JoinPNL.Controls.Add(this.JoinGame);
            this.JoinPNL.Controls.Add(this.StartNewGame);
            this.JoinPNL.Location = new System.Drawing.Point(9, 10);
            this.JoinPNL.Margin = new System.Windows.Forms.Padding(2);
            this.JoinPNL.Name = "JoinPNL";
            this.JoinPNL.Size = new System.Drawing.Size(664, 392);
            this.JoinPNL.TabIndex = 9;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(3, 321);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 18;
            this.button2.Text = "Quit";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // listBox1
            // 
            this.listBox1.BackColor = System.Drawing.Color.Black;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(346, 0);
            this.listBox1.Margin = new System.Windows.Forms.Padding(2);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(8, 381);
            this.listBox1.TabIndex = 15;
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.Location = new System.Drawing.Point(49, 157);
            this.Label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(66, 13);
            this.Label7.TabIndex = 14;
            this.Label7.Text = "GameName:";
            // 
            // NewGameName
            // 
            this.NewGameName.Location = new System.Drawing.Point(118, 153);
            this.NewGameName.Margin = new System.Windows.Forms.Padding(2);
            this.NewGameName.Name = "NewGameName";
            this.NewGameName.Size = new System.Drawing.Size(210, 20);
            this.NewGameName.TabIndex = 13;
            // 
            // Refresh
            // 
            this.Refresh.Location = new System.Drawing.Point(402, 273);
            this.Refresh.Margin = new System.Windows.Forms.Padding(2);
            this.Refresh.Name = "Refresh";
            this.Refresh.Size = new System.Drawing.Size(56, 19);
            this.Refresh.TabIndex = 12;
            this.Refresh.Text = "Refresh";
            this.Refresh.UseVisualStyleBackColor = true;
            this.Refresh.Click += new System.EventHandler(this.Refresh_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(400, 52);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Select A Game:";
            // 
            // CurrentGames
            // 
            this.CurrentGames.FormattingEnabled = true;
            this.CurrentGames.Location = new System.Drawing.Point(402, 83);
            this.CurrentGames.Margin = new System.Windows.Forms.Padding(2);
            this.CurrentGames.Name = "CurrentGames";
            this.CurrentGames.Size = new System.Drawing.Size(213, 186);
            this.CurrentGames.TabIndex = 10;
            // 
            // JoinGame
            // 
            this.JoinGame.Location = new System.Drawing.Point(539, 273);
            this.JoinGame.Margin = new System.Windows.Forms.Padding(2);
            this.JoinGame.Name = "JoinGame";
            this.JoinGame.Size = new System.Drawing.Size(75, 81);
            this.JoinGame.TabIndex = 9;
            this.JoinGame.Text = "Join Game";
            this.JoinGame.UseVisualStyleBackColor = true;
            this.JoinGame.Click += new System.EventHandler(this.JoinGame_Click);
            // 
            // StartNewGame
            // 
            this.StartNewGame.Location = new System.Drawing.Point(176, 203);
            this.StartNewGame.Margin = new System.Windows.Forms.Padding(2);
            this.StartNewGame.Name = "StartNewGame";
            this.StartNewGame.Size = new System.Drawing.Size(75, 81);
            this.StartNewGame.TabIndex = 8;
            this.StartNewGame.Text = "Start New Game";
            this.StartNewGame.UseVisualStyleBackColor = true;
            this.StartNewGame.Click += new System.EventHandler(this.StartNewGame_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(180, 70);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(323, 31);
            this.label2.TabIndex = 16;
            this.label2.Text = "Waiting For Player to Join";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(279, 147);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 19);
            this.button1.TabIndex = 17;
            this.button1.Text = "Leave Game";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(637, 366);
            this.ControlBox = false;
            this.Controls.Add(this.JoinPNL);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Menu";
            this.Text = "Menu";
            this.Load += new System.EventHandler(this.Menu_Load);
            this.JoinPNL.ResumeLayout(false);
            this.JoinPNL.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label Label7;
        private System.Windows.Forms.TextBox NewGameName;
        private System.Windows.Forms.Button Refresh;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button JoinGame;
        private System.Windows.Forms.Button StartNewGame;
        public System.Windows.Forms.ListBox CurrentGames;
        public System.Windows.Forms.Panel JoinPNL;
        public System.Windows.Forms.Label label2;
        public System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}