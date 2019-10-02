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
            for (int i=0; i<10; i++)
            {
                for (int j=0; j<10; j++)
                {
                    userShips.Controls.Add(new Button{ Text = "Hit", AutoSize = true }, i, j);
                    oppShips.Controls.Add(new Button { Text = "Hit", AutoSize = true }, i, j);
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
