﻿using System;
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
        private void Form1_Load(object sender, EventArgs e)
        {
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
                if (btn.BackColor == Color.Red)
                {
                    RemoveShips(btn);
                }
                else if(!(lenShips.Count == 0))
                {
                    ChooseShips(btn);
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
        int FirstX, SecondX, FirstY, SecondY , lengthShip , SL;
        Button FirstClicked = null;
        List<int> lenShips = new List<int>(){2,3,3,4,5};
        private void ChooseShips(Button btn)
        {
            var Name = btn.Name.Split(',');
            if (FirstClicked == null)
            {
                FirstX = Convert.ToInt32(Name[0]);
                FirstY = Convert.ToInt32(Name[1]);
                FirstClicked = btn;
                btn.BackColor = Color.Red;
            }
            else
            {
                SecondX = Convert.ToInt32(Name[0]);
                SecondY = Convert.ToInt32(Name[1]);
                if(FirstX == SecondX)
                {
                    lengthShip = Math.Abs(FirstY - SecondY) +1;
                }else if(FirstY == SecondY)
                {
                    lengthShip = Math.Abs(FirstX - SecondX)+1;
                }
                else
                {
                    FirstClicked.BackColor = SystemColors.Control;
                    MessageBox.Show("Invalid Ship");
                    FirstClicked = null;
                    return;
                }
                if (!lenShips.Contains(lengthShip))
                {
                    FirstClicked.BackColor = SystemColors.Control;
                    MessageBox.Show("Invalid Ship");
                    FirstClicked = null;
                    return;
                }
                SL = lengthShip -1;
                lenShips.Remove(lengthShip);
                FirstClicked = null;
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
        private void RemoveShips(Button btn)
        {

        }
    }
}
