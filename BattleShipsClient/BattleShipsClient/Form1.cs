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

        private void AddMessage(string message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)(() => AddMessage(message)));
            }
            else
            {
                LBMessages.Items.Add(message);
                LBMessages.TopIndex = LBMessages.Items.Count - 1;
            }
        }
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
                    userBtn.Name = i.ToString() + j.ToString();
                    OppBtn.Name = i.ToString() + j.ToString();
                    OppBtn.Enabled = false;
                    UButtons[i, j] = userBtn;
                    OButtons[i, j] = OppBtn;
                    userShips.Controls.Add(userBtn, i, j);
                    oppShips.Controls.Add(OppBtn, i, j);
                }
            }
        }
        string messagesent;
        public void Send(string message)
        {
            try
            {
                NetworkStream stream = client.GetStream();
                messagesent = $"¬{message}`";
                Byte[] data = System.Text.Encoding.Unicode.GetBytes(messagesent);
                stream.Write(data, 0, data.Length);
                Program.Log("[Sent]: " + message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Server has been closed" + ex.ToString());
                Program.Log(ex.ToString());
                Environment.Exit(0);
            }
        }
        List<string> NameShip = new List<string>() {"Destroyer","Submarine","Cruiser","BattleShip","Carrier" };
        public void recievedata()
        {
            string data;
            try
            {
                while (true)
                {
                    NetworkStream stream = client.GetStream();
                    Byte[] bytes = new byte[256];
                    int i;
                    if ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        String responseData = String.Empty;
                        string DataBunched = System.Text.Encoding.Unicode.GetString(bytes, 0, i);
                        string[] messages = DataBunched.Split('¬').Where(x => string.IsNullOrWhiteSpace(x) == false && x != "¬").ToArray();
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
                                try
                                {
                                    menu.Invoke((MethodInvoker)delegate
                                    {
                                        menu.JoinPNL.Hide();
                                        menu.Text = "Game = " + splitlist[1];
                                    });
                                }
                                catch (Exception)
                                {
                                    this.Invoke((MethodInvoker)delegate
                                    {
                                        menu.JoinPNL.Hide();
                                        menu.Text = "Game = " + splitlist[1];
                                    });
                                }
                            }
                            else if (data.StartsWith("Games:"))
                            {
                                var splitlist = data.Split(':');
                                var Games = splitlist[1].Split(',');
                                foreach (var G in Games)
                                {         
                                    try
                                    {
                                        menu.Invoke((MethodInvoker)delegate
                                        {
                                            if (!menu.CurrentGames.Items.Contains(G))
                                            {
                                                menu.CurrentGames.Items.Add(G);
                                            }
                                        });
                                    }
                                    catch (Exception)
                                    {
                                        this.Invoke((MethodInvoker)delegate
                                        {
                                            if (!menu.CurrentGames.Items.Contains(G))
                                            {
                                                menu.CurrentGames.Items.Add(G);
                                            }
                                        });
                                    }
                                }
                            }
                            else if (data.StartsWith("Opp:"))
                            {
                                var splitlist = data.Split(':');
                                OppName = splitlist[1];
                                try
                                {
                                    menu.Invoke((MethodInvoker)delegate
                                    {
                                        menu.RefreshTimer.Stop();
                                        menu.Hide();
                                        this.Text = "You vs " + OppName;
                                        this.Show();
                                        AddMessage($"{OppName} joined the game");
                                        AddMessage("Please Choose Your Ships(Destroyer[2],Subramine[3],Cruiser[3],Battleship[4],Carrier[5])");
                                        HideShips.Visible = false;
                                    });
                                }
                                catch (Exception)
                                {
                                    this.Invoke((MethodInvoker)delegate
                                    {
                                        menu.RefreshTimer.Stop();
                                        menu.Hide();
                                        this.Text = "You vs " + OppName;
                                        this.Show();
                                        AddMessage($"{OppName} joined the game");
                                        AddMessage("Please Choose Your Ships(Destroyer[2],Subramine[3],Cruiser[3],Battleship[4],Carrier[5])");
                                        HideShips.Visible = false;
                                    });
                                }
                            }
                            else if (data == "Turn")
                            {
                                EnableOppShips(1);
                                this.Invoke((MethodInvoker)delegate
                                {
                                    AddMessage("Your Turn");
                                    TurnLBL.Visible = true;
                                    this.TurnLBL.BackColor = Color.Green;
                                });
                            }
                            else if (data == "OTurn")
                            {
                                this.Invoke((MethodInvoker)delegate
                                {
                                    AddMessage("Opponents Turn");
                                    TurnLBL.Visible = true;
                                    this.TurnLBL.BackColor = Color.Red;
                                });
                            }
                            else if (data.StartsWith("Hit:"))
                            {
                                var splitlist = data.Split(':');
                                string ShipName = NameShip[Convert.ToInt32(splitlist[1])];
                                this.Invoke((MethodInvoker)delegate
                                {
                                    OButtons[int.Parse(splitlist[2][0].ToString()), int.Parse(splitlist[2][1].ToString())].FlatStyle = FlatStyle.Flat;
                                    OButtons[int.Parse(splitlist[2][0].ToString()), int.Parse(splitlist[2][1].ToString())].FlatAppearance.BorderColor = Color.Red;
                                    OButtons[int.Parse(splitlist[2][0].ToString()), int.Parse(splitlist[2][1].ToString())].FlatAppearance.BorderSize = 1;
                                    AddMessage($"You Hit Your Opponents {ShipName}({ShipNameConvert(splitlist[2])})");
                                });
                            }
                            else if (data.StartsWith("OHit:"))
                            {
                                var splitlist = data.Split(':');
                                string ShipName = NameShip[Convert.ToInt32(splitlist[1])];
                                this.Invoke((MethodInvoker)delegate
                                {
                                    UButtons[int.Parse(splitlist[2][0].ToString()), int.Parse(splitlist[2][1].ToString())].FlatStyle = FlatStyle.Flat;
                                    UButtons[int.Parse(splitlist[2][0].ToString()), int.Parse(splitlist[2][1].ToString())].FlatAppearance.BorderColor = Color.Red;
                                    UButtons[int.Parse(splitlist[2][0].ToString()), int.Parse(splitlist[2][1].ToString())].FlatAppearance.BorderSize = 1;
                                    AddMessage($"Your {ShipName}  was hit({ShipNameConvert(splitlist[2])})");
                                });
                            }
                            else if (data.StartsWith("Miss:"))
                            {
                                var splitlist = data.Split(':');
                                this.Invoke((MethodInvoker)delegate
                                {
                                    OButtons[int.Parse(splitlist[1][0].ToString()), int.Parse(splitlist[1][1].ToString())].BackColor = Color.Black;
                                    AddMessage($"You missed({ShipNameConvert(splitlist[1])})");
                                });
                            }
                            else if (data.StartsWith("OMiss:"))
                            {
                                var splitlist = data.Split(':');
                                this.Invoke((MethodInvoker)delegate
                                {
                                    UButtons[int.Parse(splitlist[1][0].ToString()), int.Parse(splitlist[1][1].ToString())].BackColor = Color.Black;
                                    AddMessage($"Your Opponents Missed({ShipNameConvert(splitlist[1])})");
                                });
                            }
                            else if (data == "Invalid")
                            {
                                MessageBox.Show("Invalid Posistion Chosen \n Please choose again");
                                EnableOppShips(1);
                            }
                            else if (data.StartsWith("OSunk:"))
                            {
                                string ShipNames = "";
                                var SL = data.Split(':');
                                var splitlist = SL[2].Split(',');
                                string SN = NameShip[Convert.ToInt32(SL[1])];
                                this.Invoke((MethodInvoker)delegate
                                {
                                    foreach (var p in splitlist)
                                    {
                                        OButtons[int.Parse(p[0].ToString()), int.Parse(p[1].ToString())].BackColor = Color.Red;
                                        ShipNames += "," + ShipNameConvert(p);
                                    }
                                    ShipNames = ShipNames.Remove(0, 1);
                                    AddMessage($"You Sunk Your Opponents {SN}({ShipNames})");
                                });
                            }
                            else if (data.StartsWith("Sunk:"))
                            {
                                string ShipNames = "";
                                var SL = data.Split(':');
                                var splitlist = SL[2].Split(',');
                                string SN = NameShip[Convert.ToInt32(SL[1])];
                                this.Invoke((MethodInvoker)delegate
                                {
                                    foreach (var p in splitlist)
                                    {
                                        UButtons[int.Parse(p[0].ToString()), int.Parse(p[1].ToString())].BackColor = Color.Red;
                                        ShipNames += "," + ShipNameConvert(p);
                                    }
                                    ShipNames = ShipNames.Remove(0, 1);
                                    AddMessage($"Your {SN} Was Sunk({ShipNames})");
                                });
                            }
                            else if (data == "Win")
                            {
                                AddMessage("You Win");
                                this.Invoke((MethodInvoker)delegate
                                {
                                    HideShips.Visible = false;
                                });
                                MessageBox.Show("You win");
                                this.Invoke((MethodInvoker)delegate
                                {
                                    this.Close();
                                    menu = new Menu();
                                    menu.first = false;
                                    menu.client = client;
                                    menu.JoinPNL.Show();
                                    menu.ShowDialog();
                                });
                            }
                            else if (data == "Lose")
                            {
                                AddMessage("You Lose");
                                this.Invoke((MethodInvoker)delegate
                                {
                                    HideShips.Visible = false;
                                });
                                MessageBox.Show("You Lose");
                                this.Invoke((MethodInvoker)delegate
                                {
                                    this.Close();
                                    menu = new Menu();
                                    menu.client = client;
                                    menu.JoinPNL.Show();
                                    menu.first = false;
                                    menu.Show();
                                });
                            }
                            else if (data.StartsWith("Message:"))
                            {
                                var SL = data.Split(':');
                                AddMessage($"Recived from {OppName}:{SL[1]}");
                            }
                        }
                    }
                }
            }catch (Exception ex)
            {
                MessageBox.Show("Server has been closed:" + ex.ToString());
                Program.Log(ex.ToString());
            }
        }
        private void OppBtn_Click(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                Send("OShip:"+btn.Name);
                EnableOppShips(0);
            }
        }
        private void UserBtn_Click(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                if(ConfirmedShips == false)
                {
                    if (!(btn.BackColor == SystemColors.Control))
                    {
                        if(FirstClicked == null)
                        {
                            RemoveShips(btn);
                        }
                        else
                        {
                            MessageBox.Show("Invalid Ship");
                            FirstClicked.BackColor = SystemColors.Control;
                            FirstClicked = null;
                            return;
                        }
                    }
                    else
                    {
                        ChoseShips(btn);
                    }
                }
            }
        }
        string ShipString = "";
        private void ConfirmBTN_Click(object sender, EventArgs e)
        {
            if (AllowedShipLengths.Count ==0)
            {
                ConfirmedShips = true;
                ConfirmBTN.Visible = false;
                ShipString = "";
                foreach (List<Button> LB in Ships)
                {
                    ShipString = "Ships:"+ LB[0].Name;
                    for (int i = 1;i<LB.Count;i++)
                    {
                        ShipString += ","+LB[i].Name;
                    }
                    Send(ShipString);
                }
                Send("ShipsConfirmed");
                this.Invoke((MethodInvoker)delegate
                {
                    AddMessage("Ships Confirmed");
                    AddMessage("Waiting for Opponent");
                });
                foreach (Button b in UButtons)
                {
                    b.Enabled = false;
                }
            }
            else
            {
                MessageBox.Show("Place all Ships First");
            }
        }

       
        int FirstX, SecondX, FirstY, SecondY, lengthShip;
        List<int> AllowedShipLengths = new List<int>() { 2,3,3,4,5};
        Button FirstClicked = null;
        int Swap;
        private void ChoseShips(Button btn)
        {
            //MessageBox.Show(btn.Name);
            List<Button> ThisShip = new List<Button>();
            List<string> Name = new List<string>();
            foreach(char c in btn.Name)
            {
                Name.Add(c.ToString());
            }
            //MessageBox.Show(Name[0]);
            //MessageBox.Show(Name[1]);
            if (FirstClicked == null)
            {
                FirstX = Convert.ToInt32(Name[0]);
                FirstY = Convert.ToInt32(Name[1]);
                FirstClicked = btn;
                btn.BackColor = Color.Yellow;
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
                if (!(AllowedShipLengths.Contains(lengthShip)))
                {
                    MessageBox.Show("Invalid Ship");
                    FirstClicked = null;
                    return;
                }
                if (SecondX < FirstX)
                {
                    Swap = FirstX;
                    FirstX = SecondX;
                    SecondX = Swap;
                }
                if (SecondY < FirstY)
                {
                    Swap = FirstY;
                    FirstY = SecondY;
                    SecondY = Swap;
                }
                for(int i = FirstX; i <SecondX+1;i++)
                {
                    for(int n = FirstY; n < SecondY+1; n++)
                    {
                        Button b = UButtons[i,n];
                        if(!(b.BackColor == SystemColors.Control))
                        {
                            foreach(Button butt in ThisShip)
                            {
                                butt.BackColor = SystemColors.Control;
                                
                            }
                            FirstClicked = null;
                            MessageBox.Show("Invalid Ship");
                            return;
                        }
                        b.BackColor = ShipColour(lengthShip);
                        ThisShip.Add(b);
                    }
                }
                AllowedShipLengths.Remove(lengthShip);
                Ships.Add(ThisShip);
                FirstClicked = null;
                if (AllowedShipLengths.Count == 0)
                {
                    ConfirmBTN.Visible = true;
                }
                //PrintShips();
            }
        }
        private void PrintShips()
        {
            foreach(List<Button>  l in Ships)
            {
                foreach(Button b in l)
                {
                    MessageBox.Show(b.Name);
                }
            }
        }





        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 'h' || e.KeyChar == 'H')
            {
                if (HideShips.Visible == true)
                {
                    HideShips.Visible = false;
                }
                else
                {
                    HideShips.Visible = true;
                }
            }
        }

        private void BTNSend_Click(object sender, EventArgs e)
        {
            if (CBmsg.SelectedIndex ==-1)
            {
                MessageBox.Show("Invalid Message");
                return;
            }
            Send("Message:"+CBmsg.Items[CBmsg.SelectedIndex]);
            AddMessage("Sent:" + CBmsg.Items[CBmsg.SelectedIndex]);
            CBmsg.SelectedIndex = -1;
        }

        private void BTNhelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("HelpMessageHere");
        }

        private void BTNLeave_Click(object sender, EventArgs e)
        {
            try
            {
                Send("LeftG");
                this.Close();
                menu = new Menu();
                menu.first = false;
                menu.client = client;
                menu.JoinPNL.Show();
                menu.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void BTNQuit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
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
                        break;
                    }
                }
            }
            AllowedShipLengths.Add(Ships[lnumber].Count);
            foreach (Button b in Ships[lnumber])
            {
                b.BackColor = SystemColors.Control;
            }
            Ships.RemoveAt(lnumber);
            ConfirmBTN.Visible = false;
            try
            {
                FirstClicked.BackColor = SystemColors.Control;
                FirstClicked = null;
            }catch (Exception) { }
        }

        private Color ShipColour(int SL)
        {
            if(SL == 2)
            {
                return Color.Purple;
            }else if(SL == 4)
            {
                return Color.Green;
            }else if(SL == 5)
            {
                return Color.Orange;
            }else if(SL == 3)
            {
                AllowedShipLengths.Remove(3);
                if (AllowedShipLengths.Contains(3))
                {
                    AllowedShipLengths.Add(3);
                    return Color.LightBlue;
                }
                else
                {
                    AllowedShipLengths.Add(3);
                    return Color.DarkBlue;
                }
            }
            else
            {
                return SystemColors.Control;
            }
        }
        private void EnableOppShips(int Option)
        {
            foreach(Button b in OButtons)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    if (Option == 0)
                    {
                        b.Enabled = false;
                    }
                    else
                    {
                        b.Enabled = true;
                    }
                });
            }
        }

        public static string ShipNameConvert(string rawText) 
        {
            List<string> listLetters = new List<string>() {"A","B","C","D","E","F","G","H","I","J"};
            var yValue = int.Parse(rawText[1].ToString());
            var xValue = int.Parse(rawText[0].ToString());
            var xCoordinate = listLetters[xValue];
            var yCoordinate = (yValue+1).ToString();            
            return(xCoordinate+yCoordinate);
        }
    }
}
