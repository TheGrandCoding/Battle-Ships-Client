using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace BattleShipsClient
{
    public partial class Menu : Form
    {
        public System.Windows.Forms.Timer RefreshTimer;
        public Form1 f1 = new Form1();
        public TcpClient client = new TcpClient();
        public bool first = true;

        public Menu()
        {
            InitializeComponent();
        }
        private void Menu_Load(object sender, EventArgs e)
        {
            if (first == true)
            {
                Program.MakeLog();
                Con();
            }
            else
            {
                f1 = new Form1();
                f1.menu = this;
                f1.client = client;
                Thread rd = new Thread(f1.recievedata);
                rd.Start();
                //f1.Send("UN:" + Environment.UserName);
                this.Text = "Menu";
                JoinPNL.Show();
            }
            Refresh_Click(null, EventArgs.Empty);
            RefreshTimer = new System.Windows.Forms.Timer();
            RefreshTimer.Tick += new EventHandler(Refresh_Click);
            RefreshTimer.Interval = 5000;
            RefreshTimer.Start();
        }
        public void Con()
        {
            if(IPAddress.TryParse(Properties.Resources.IPAdress, out IPAddress ipaddress))
            {
                client.Connect(ipaddress, 9876);
                f1.client = client;
                f1.menu = this;
                f1.Send("UN:" + Environment.UserName);
                Program.Log("Connected to " + Properties.Resources.IPAdress);
                Thread rd = new Thread(f1.recievedata);
                rd.Start();
            }
        }
        private void StartNewGame_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NewGameName.Text) || NewGameName.Text.Contains("`") || NewGameName.Text.Contains("¬")|| NewGameName.Text.Contains(","))
            {
                MessageBox.Show("Invalid Game Name(No ` ¬ ,)");
                return;
            }
            f1.Send("NewGame:" + NewGameName.Text);
        }

        private void Refresh_Click(object sender, EventArgs e)
        {
            CurrentGames.Items.Clear();
            f1.Send("CurrentGames");
        }

        private void JoinGame_Click(object sender, EventArgs e)
        {
            if (CurrentGames.SelectedIndex > -1)
            {
                //MessageBox.Show(CurrentGames.SelectedItem.ToString());
                f1.Send("JoinGame:" + CurrentGames.SelectedItem.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            f1.Send("LeftG");
            JoinPNL.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
