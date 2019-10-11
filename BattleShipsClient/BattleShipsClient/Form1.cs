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
        public Menu menu;
        public TcpClient client;
        Button[,] UButtons = new Button[10, 10];
        Button[,] OButtons = new Button[10, 10];
        Size PlayingSize = new Size(1158, 796);
        string OppName;
        bool ConfirmedShips = false;
        List<List<Button>> Ships = new List<List<Button>>();
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Location = new Point(0, 0);
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Button userBtn = new Button();
                    Button OppBtn = new Button();
                    userBtn.Click += UserBtn_Click;
                    OppBtn.Click += OppBtn_Click;
                    userBtn.BackColor = SystemColors.Control;
                    OppBtn.BackColor = SystemColors.Control;
                    userBtn.Name = i.ToString() + "," + j.ToString();
                    OppBtn.Name = i.ToString() + "," + j.ToString();
                    UButtons[i, j] = userBtn;
                    OButtons[i, j] = OppBtn;
                    userShips.Controls.Add(userBtn, i, j);
                    oppShips.Controls.Add(OppBtn, i, j);
                }
            }
        }
        public void recievedata()
        {
            string data;
            while (true)
            {
                NetworkStream stream = client.GetStream();
                Byte[] bytes = new byte[256];
                int i;
                if ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    String responseData = String.Empty;
                    string DataBunched = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    string[] messages = DataBunched.Split('%').Where(x => string.IsNullOrWhiteSpace(x) == false && x != "%").ToArray();
                    foreach (var msg in messages)
                    {
                        data = msg.Substring(0, msg.IndexOf("`"));
                        Program.Log("[Rec] " + data);
                        if (data == "InvalidName")
                        {
                            MessageBox.Show("Name is already in use , please choose another one");
                        }
                        else if (data.StartsWith("JoinedGame:"))
                        {
                            var splitlist = data.Split(':');
                            menu.Invoke((MethodInvoker)delegate
                            {
                                menu.JoinPNL.Hide();
                                menu.Text = "Game = " + splitlist[1];
                            });
                        }
                        else if (data.StartsWith("Game:"))
                        {
                            var splitlist = data.Split(':');
                            menu.Invoke((MethodInvoker)delegate
                            {
                                if (!menu.CurrentGames.Items.Contains(splitlist[1]))
                                {
                                    menu.CurrentGames.Items.Add(splitlist[1]);
                                }
                            });
                        } else if (data.StartsWith("Opp:"))
                        {
                            var splitlist = data.Split(':');
                            OppName = splitlist[1];
                            menu.Invoke((MethodInvoker)delegate
                            {
                                menu.Hide();
                                this.Text = "vs " + OppName;
                                this.Show();
                            });
                        }
                    }
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
                if(ConfirmedShips == false)
                {
                    if (btn.BackColor == Color.Black)
                    {
                        RemoveShips(btn);
                    }
                    else if (!(lenShips.Count == 0))
                    {
                        ChoseShips(btn);
                    }
                }
            }
        }


        string messagesent;
        public void Send(string message)
        {
            try
            {
                NetworkStream stream = client.GetStream();
                messagesent = $"%{message}`";
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(messagesent);
                stream.Write(data, 0, data.Length);
                Program.Log("[Sent]: " + message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Server has been closed" + ex.ToString());
                Environment.Exit(0);
            }
        }

        private void ConfirmBTN_Click(object sender, EventArgs e)
        {
            if(lenShips.Count == 0)
            {
                ConfirmedShips = true;
                ConfirmBTN.Visible = false;
                //send Ships to server
            }
            else
            {
                MessageBox.Show("Place all Ships First");
            }
        }

       
        int FirstX, SecondX, FirstY, SecondY, lengthShip, SL;
        List<int> lenShips = new List<int>() { 2, 3, 3, 4, 5 };
        Button FirstClicked = null;
        List<Button> ThisShip = new List<Button>();
        int temp;
        private void ChoseShips(Button btn)
        {
            var Name = btn.Name.Split(',');
            if (FirstClicked == null)
            {
                FirstX = Convert.ToInt32(Name[0]);
                FirstY = Convert.ToInt32(Name[1]);
                FirstClicked = btn;
                btn.BackColor = Color.Black;
            }
            else
            {
                FirstClicked.BackColor = SystemColors.Control;
                SecondX = Convert.ToInt32(Name[0]);
                SecondY = Convert.ToInt32(Name[1]);
                if (FirstX == SecondX)
                {
                    lengthShip = Math.Abs(FirstY - SecondY) + 1;
                }
                else if (FirstY == SecondY)
                {
                    lengthShip = Math.Abs(FirstX - SecondX) + 1;
                }
                else
                {
                    MessageBox.Show("Invalid Ship");
                    FirstClicked = null;
                    return;
                }
                if (!lenShips.Contains(lengthShip))
                {
                    MessageBox.Show("Invalid Ship");
                    FirstClicked = null;
                    return;
                }
                ThisShip.Clear();
                if (SecondX < FirstX)
                {
                    temp = FirstX;
                    FirstX = SecondX;
                    SecondX = temp;
                }
                if (SecondY < FirstY)
                {
                    temp = FirstY;
                    FirstY = SecondY;
                    SecondY = temp;
                }
                for(int i = FirstX; i <SecondX+1;i++)
                {
                    for(int n = FirstY; n < SecondY+1; n++)
                    {
                        Button b = UButtons[i,n];
                        if(b.BackColor == Color.Black)
                        {
                            foreach(Button butt in ThisShip)
                            {
                                butt.BackColor = SystemColors.Control;
                                
                            }
                            FirstClicked = null;
                            MessageBox.Show("Invalid Ship");
                            return;
                        }
                        b.BackColor = Color.Black;
                        ThisShip.Add(b);
                    }
                }
                lenShips.Remove(lengthShip);
                Ships.Add(ThisShip);
                FirstClicked = null;
                if (lenShips.Count == 0)
                {
                    ConfirmBTN.Visible = true;
                }
            }
        }
        private void RemoveShips(Button btn)
        {
            int lnumber = 0;
            for(int i = 0;i<Ships.Count;i++)
            {
                foreach(Button bt in Ships[i])
                {
                    if(bt == btn)
                    {
                        lnumber = i;
                    }
                }
            }
            lenShips.Add(Ships[lnumber].Count);
            foreach (Button b in Ships[lnumber])
            {
                b.BackColor = SystemColors.Control;
            }
            Ships.RemoveAt(lnumber);
            ConfirmBTN.Visible = false;
            FirstClicked.BackColor = SystemColors.Control;
            FirstClicked = null;
        }
    }
}
