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


        private void Form1_Load(object sender, EventArgs e)
        {
            //Con();
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
                    userShips.Controls.Add(userBtn, i, j);
                    oppShips.Controls.Add(OppBtn, i, j);
                }
            }
        }
        private void OppBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("opp");
        }
        int FirstX , SecoundX , FirstY , SecoundY;
        bool FirstClicked = false;
        int lenthShips = 5;
        private void UserBtn_Click(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                var Name = btn.Name.Split(',');
                if(FirstClicked == false)
                {
                    FirstX = Convert.ToInt32(Name[0]);
                    FirstY =Convert.ToInt32(Name[1]);
                    FirstClicked = true;
                }
                else
                {
                    FirstClicked = false;
                    SecoundX = Convert.ToInt32(Name[0]);
                    SecoundY = Convert.ToInt32(Name[1]);
                    int SL = lenthShips - 1;
                    if(FirstX == SecoundX || FirstY == SecoundY)
                    {
                        if(FirstX +SL == SecoundX || FirstX - SL == SecoundX || FirstY + SL == SecoundY || FirstY - SL == SecoundY)
                        {
                            MessageBox.Show("True");
                        }
                        else
                        {
                            UserBtn_Click(btn, EventArgs.Empty);
                        }
                    }
                    else
                    {
                        UserBtn_Click(btn, EventArgs.Empty);
                    }
                    
                }
            }
        }
        public void Con()
        {
            bool valid = IPAddress.TryParse(Properties.Resources.IPAdress, out IPAddress ipaddress);
            client.Connect(ipaddress, 777);
            MessageBox.Show("connected to" + Properties.Resources.IPAdress);
        }
    }
}
