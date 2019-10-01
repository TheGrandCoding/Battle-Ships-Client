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

        private void button28_Click(object sender, EventArgs e)
        {

        }

        private void button30_Click(object sender, EventArgs e)
        {

        }

        private void button29_Click(object sender, EventArgs e)
        {

        }

        private void button33_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Con();
        }
        public void Con()
        {
            bool valid = IPAddress.TryParse(Properties.Resources.IPAdress, out IPAddress ipaddress);
            client.Connect(ipaddress, 777);
            MessageBox.Show("connected to" + Properties.Resources.IPAdress);
        }
    }
}
