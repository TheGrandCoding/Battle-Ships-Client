using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace BattleShipsClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        TcpClient client = new TcpClient();
        Button[,] UButtons = new Button[10, 10];
        Button[,] OButtons = new Button[10, 10];

        private void Form1_Load(object sender, EventArgs e)
        {
            Program.MakeLog();
            Con();
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Button userBtn = new Button();
                    Button OppBtn = new Button();
                    userBtn.Click += UserBtn_Click;
                    OppBtn.Click += OppBtn_Click;
                    userBtn.Name =i.ToString() +","+ j.ToString();
                    OppBtn.Name = i.ToString() + "," + j.ToString();
                    UButtons[i,j] = userBtn;
                    OButtons[i, j] = OppBtn;
                    userShips.Controls.Add(userBtn, i, j);
                    oppShips.Controls.Add(OppBtn, i, j);
                }
            }
        }
        private void OppBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("opp");
        }
        private void UserBtn_Click(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                ChooseShips(btn);
            }
        }
        int FirstX, SecondX, FirstY, SecondY;
        bool FirstClicked = false;

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void lblA_Click(object sender, EventArgs e)
        {

        }

        private void lblUser10_Click(object sender, EventArgs e)
        {

        }

        private void lblUser5_Click(object sender, EventArgs e)
        {

        }

        private void lblUser1_Click(object sender, EventArgs e)
        {

        }

        public void Send(string message)
        {
            try
            {
                NetworkStream stream = client.GetStream();
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                stream.Write(data, 0, data.Length);
                Program.Log("[Sent]: " + message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Server has been closed" + ex.ToString());
                Environment.Exit(0);
            }
        }
        int lenthShips = 5;
        List<int> UserList
        private void ChooseShips(Button btn)
        {
            var Name = btn.Name.Split(',');
            if (FirstClicked == false)
            {
                FirstX = Convert.ToInt32(Name[0]);
                FirstY = Convert.ToInt32(Name[1]);
                FirstClicked = true;
                btn.BackColor = Color.Red;
            }
            else
            {
                FirstClicked = false;
                SecondX = Convert.ToInt32(Name[0]);
                SecondY = Convert.ToInt32(Name[1]);
                int SL = lenthShips - 1;
                if (FirstX == SecondX || FirstY == SecondY)
                {
                    if (FirstX + SL == SecondX)
                    {
                        for (int i = FirstX; i <= SL + FirstX; i++)
                        {
                            UButtons[i, FirstY].BackColor = Color.Red;
                        }
                    }
                    else if (FirstX - SL == SecondX)
                    {
                        for (int i =SecondX; i <= SL +SecondX; i++)
                        {
                            UButtons[i, FirstY].BackColor = Color.Red;
                        }
                    }
                    else if (FirstY + SL == SecondY)
                    {
                        for (int i = FirstY; i <= SL + FirstY; i++)
                        {
                            UButtons[FirstX, i].BackColor = Color.Red;
                        }
                    }
                    else if (FirstY - SL == SecondY)
                    {
                        for (int i = SecondY; i <= SL + SecondY; i++)
                        {
                            UButtons[FirstX, i].BackColor = Color.Red;
                        }
                    }
                }

            }
        }
        public void Con()
        {
            bool valid = IPAddress.TryParse(Properties.Resources.IPAdress, out IPAddress ipaddress);
            client.Connect(ipaddress, 666);
            MessageBox.Show("connected to" + Properties.Resources.IPAdress);
            Send("UN:"+Environment.UserName);
        }
    }
}
