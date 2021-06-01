using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
namespace Prog225FinalProject___Victor_Besson
{
    public partial class ArenaForm : Form
    {
        public static List<Panel> PlayerInfo = new List<Panel>();
        public ArenaForm()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            Cycle.CycleMessage += CycleMessage; //adds event to form based on cycle delegate
            foreach (CycleSelection cs in CycleSelection.SelectionList) //adds players based on cycle selected in cycleselection
            {
                switch (cs.current)
                {
                    case 0:
                        Cycle.Players.Add(new BlueCycle(this));
                        break;
                    case 1:
                        Cycle.Players.Add(new GreenCycle(this));
                        break;
                    case 2:
                        Cycle.Players.Add(new OrangeCycle(this));
                        break;
                    case 3:
                        Cycle.Players.Add(new PurpleCycle(this));
                        break;
                    case 4:
                        Cycle.Players.Add(new PinkCycle(this));
                        break;
                    case 5:
                        Cycle.Players.Add(new RedCycle(this));
                        break;
                    case 6:
                        Cycle.Players.Add(new WhiteCycle(this));
                        break;
                    case 7:
                        Cycle.Players.Add(new YellowCycle(this));
                        break;
                }
            }
            LoadPlayerInfo(); //Loads player info on top of arenaform
            CycleSelection.SelectionList.Clear(); //clears selectionlist to free up memory
        }
        public void CycleMessage(string message)
        {
            try
            {
                if (message.Contains(".wav")) //plays sound effect. Wish I knew how to play multiple sounds at once
                {
                    SoundPlayer simpleSound = new SoundPlayer(message);
                    simpleSound.Play();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void LoadPlayerInfo()
        {
            try
            {
                List<Color> Colors = new List<Color>(); //List of colors to be used to set the player labels color based on cycle selected
                Colors.Add(Color.Blue);
                Colors.Add(Color.Green);
                Colors.Add(Color.Orange);
                Colors.Add(Color.Purple);
                Colors.Add(Color.Pink);
                Colors.Add(Color.Red);
                Colors.Add(Color.White);
                Colors.Add(Color.Yellow);
                foreach (Cycle c in Cycle.Players)
                {
                    if (c.playerNum == 1)
                    {
                        Panel tmpPanel = new Panel(); //panel added to put play info into
                        Label lblPlayer = new Label(); //label displaying player number
                        tmpPanel.Width = pnInfo.Width / 4;
                        tmpPanel.Height = pnInfo.Height;
                        tmpPanel.Left = 0;
                        tmpPanel.Top = 0;
                        tmpPanel.BorderStyle = BorderStyle.FixedSingle;
                        lblPlayer.Text = "Player 1";
                        lblPlayer.Font = new Font("OCR A", 18, FontStyle.Bold);
                        lblPlayer.Width = TextRenderer.MeasureText(lblPlayer.Text, lblPlayer.Font, lblPlayer.MaximumSize).Width; //sets the width of the label based on the text, font, and maximum size
                        lblPlayer.AutoSize = true;
                        lblPlayer.Top = (tmpPanel.Height / 3) - (lblPlayer.Height / 2);
                        lblPlayer.Left = (tmpPanel.Width / 2) - (lblPlayer.Width / 2);
                        lblPlayer.ForeColor = Colors[CycleSelection.SelectionList[0].current];
                        tmpPanel.Controls.Add(lblPlayer);
                        pnInfo.Controls.Add(tmpPanel);
                        PlayerInfo.Add(tmpPanel);
                    }
                    if (c.playerNum == 2)
                    {
                        Panel tmpPanel = new Panel();
                        Label lblPlayer = new Label();
                        tmpPanel.Width = pnInfo.Width / 4;
                        tmpPanel.Height = pnInfo.Height;
                        tmpPanel.Left = pnInfo.Width / 4;
                        tmpPanel.Top = 0;
                        tmpPanel.BorderStyle = BorderStyle.FixedSingle;
                        lblPlayer.Text = "Player 2";
                        lblPlayer.Font = new Font("OCR A", 18, FontStyle.Bold);
                        lblPlayer.Width = TextRenderer.MeasureText(lblPlayer.Text, lblPlayer.Font, lblPlayer.MaximumSize).Width;
                        lblPlayer.AutoSize = true;
                        lblPlayer.Top = (tmpPanel.Height / 3) - (lblPlayer.Height / 2);
                        lblPlayer.Left = (tmpPanel.Width / 2) - (lblPlayer.Width / 2);
                        lblPlayer.ForeColor = Colors[CycleSelection.SelectionList[1].current];
                        tmpPanel.Controls.Add(lblPlayer);
                        pnInfo.Controls.Add(tmpPanel);
                        PlayerInfo.Add(tmpPanel);
                    }
                    if (c.playerNum == 3)
                    {
                        Panel tmpPanel = new Panel();
                        Label lblPlayer = new Label();
                        tmpPanel.Width = pnInfo.Width / 4;
                        tmpPanel.Height = pnInfo.Height;
                        tmpPanel.Left = pnInfo.Width / 2;
                        tmpPanel.Top = 0;
                        tmpPanel.BorderStyle = BorderStyle.FixedSingle;
                        lblPlayer.Text = "Player 3";
                        lblPlayer.Font = new Font("OCR A", 18, FontStyle.Bold);
                        lblPlayer.Width = TextRenderer.MeasureText(lblPlayer.Text, lblPlayer.Font, lblPlayer.MaximumSize).Width;
                        lblPlayer.AutoSize = true;
                        lblPlayer.Top = (tmpPanel.Height / 3) - (lblPlayer.Height / 2);
                        lblPlayer.Left = (tmpPanel.Width / 2) - (lblPlayer.Width / 2);
                        lblPlayer.ForeColor = Colors[CycleSelection.SelectionList[2].current];
                        tmpPanel.Controls.Add(lblPlayer);
                        pnInfo.Controls.Add(tmpPanel);
                        PlayerInfo.Add(tmpPanel);
                    }
                    if (c.playerNum == 4)
                    {
                        Panel tmpPanel = new Panel();
                        Label lblPlayer = new Label();
                        tmpPanel.Width = pnInfo.Width / 4;
                        tmpPanel.Height = pnInfo.Height;
                        tmpPanel.Left = pnInfo.Width * 3 / 4;
                        tmpPanel.Top = 0;
                        tmpPanel.BorderStyle = BorderStyle.FixedSingle;
                        lblPlayer.Text = "Player 4";
                        lblPlayer.Font = new Font("OCR A", 18, FontStyle.Bold);
                        lblPlayer.Width = TextRenderer.MeasureText(lblPlayer.Text, lblPlayer.Font, lblPlayer.MaximumSize).Width;
                        lblPlayer.AutoSize = true;
                        lblPlayer.Top = (tmpPanel.Height / 3) - (lblPlayer.Height / 2);
                        lblPlayer.Left = (tmpPanel.Width / 2) - (lblPlayer.Width / 2);
                        lblPlayer.ForeColor = Colors[CycleSelection.SelectionList[3].current];
                        tmpPanel.Controls.Add(lblPlayer);
                        pnInfo.Controls.Add(tmpPanel);
                        PlayerInfo.Add(tmpPanel);
                    }
                }//
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
