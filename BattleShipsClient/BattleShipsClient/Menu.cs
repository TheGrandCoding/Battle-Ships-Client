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
        public Form1 f1 = new Form1();
        TcpClient client = new TcpClient();

        public Menu()
        {
            InitializeComponent();
        }
        private void Menu_Load(object sender, EventArgs e)
        {
            Program.MakeLog();
            Con();
            Refresh_Click(null, EventArgs.Empty);
        }
        public void Con()
        {
            bool valid = IPAddress.TryParse(Properties.Resources.IPAdress, out IPAddress ipaddress);
            client.Connect(ipaddress, 1234);
            f1.client = client;
            f1.menu = this;
            f1.Send("UN:" + Environment.UserName);
            Program.Log("Connected to " + Properties.Resources.IPAdress);
            Thread rd = new Thread(f1.recievedata);
            rd.Start();
        }
        private void StartNewGame_Click(object sender, EventArgs e)
        {
            if (NewGameName.Text == "" || NewGameName.Text.Contains("%") || NewGameName.Text.Contains("'"))
            {
                MessageBox.Show("Invalid Game Name");
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

        private void Button1_Click(object sender, EventArgs e)
        {

        }

        private void Menu_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }
    }
}
