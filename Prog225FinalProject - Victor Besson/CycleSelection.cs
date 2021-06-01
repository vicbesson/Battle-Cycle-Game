using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
namespace Prog225FinalProject___Victor_Besson
{
    class CycleSelection
    {
        private static int numPlayers;
        public static List<CycleSelection> SelectionList = new List<CycleSelection>();
        private static Image arrowimage;
        private static List<Image> cycleImages = new List<Image>();
        private static int counter;
        private int currentImage;
        private int playerNumber;
        private static Form PForm;
        private Panel selectionPanel = new Panel(); //Panel holding players
        private PictureBox myCycle = new PictureBox();
        private PictureBox rightArrow = new PictureBox();
        private PictureBox leftArrow = new PictureBox();
        private static Button readyButton = new Button();
        private static Panel readyPanel = new Panel();
        private Label lblName = new Label();
        static CycleSelection()
        {
            try
            {
                Bitmap tmpbitmap;
                if (!File.Exists("arrow.png"))
                    throw new Exception("Missing Files Please Redownload - Application Closed");
                arrowimage = Image.FromFile("arrow.png"); //Image to be used to pick cycle color
                numPlayers = Cycle.NumPlayers;
                counter = numPlayers - 1;
                for (int i = 0; i < 8; i++)
                {
                    if (!File.Exists($"Tron{i}.png"))
                        throw new Exception("Missing Files Please Redownload - Application Closed");
                    cycleImages.Add(Image.FromFile($"Tron{i}.png")); //Cycle Images
                }
                for (int i = 0; i < cycleImages.Count; i++)
                {
                    tmpbitmap = new Bitmap(cycleImages[i]);
                    tmpbitmap.RotateFlip(RotateFlipType.Rotate90FlipX);
                    cycleImages[i] = tmpbitmap;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Exit();
            }
        }
        public CycleSelection(Form pForm)
        {
            try
            {
                playerNumber = numPlayers - counter;
                counter--;
                selectionPanel.Height = (pForm.ClientSize.Height - 93) / 2; 
                selectionPanel.Width = pForm.ClientSize.Width / 2;
                selectionPanel.BorderStyle = BorderStyle.Fixed3D;
                switch (playerNumber)
                {
                    case 1:
                        selectionPanel.Left = 0;
                        selectionPanel.Top = 0;
                        myCycle.Image = new Bitmap(cycleImages[0]);
                        currentImage = 0;
                        readyPanel.Width = pForm.Width;
                        readyPanel.Height = 120;
                        readyPanel.Left = 0;
                        readyPanel.Top = pForm.Height - readyPanel.Height;
                        readyButton.Width = 164;
                        readyButton.Height = 64;
                        readyButton.Left = (readyPanel.Width / 2) - (readyButton.Width / 2);
                        readyButton.Top = (readyPanel.Height / 2) - 60;
                        readyButton.Tag = "ready"; //like static contructor spawning the ready button on the first contructed selection
                        readyButton.Click += Ready_click;
                        readyButton.Text = "Start";
                        readyButton.Font = new Font("OCR A Extended", 11, FontStyle.Bold);
                        readyButton.FlatStyle = FlatStyle.Flat;
                        readyButton.BackColor = Color.DarkTurquoise;
                        readyPanel.Controls.Add(readyButton);
                        pForm.Controls.Add(readyPanel);
                        break;
                    case 2:
                        selectionPanel.Left = pForm.ClientSize.Width / 2;
                        selectionPanel.Top = 0;
                        myCycle.Image = new Bitmap(cycleImages[1]);
                        currentImage = 1;
                        break;
                    case 3:
                        selectionPanel.Left = 0;
                        selectionPanel.Top = (pForm.ClientSize.Height - 93) / 2;
                        myCycle.Image = new Bitmap(cycleImages[2]);
                        currentImage = 2;
                        break;
                    case 4:
                        selectionPanel.Left = pForm.ClientSize.Width / 2;
                        selectionPanel.Top = (pForm.ClientSize.Height - 93) / 2;
                        myCycle.Image = new Bitmap(cycleImages[3]);
                        currentImage = 3;
                        break;
                }
                selectionPanel.BackColor = Color.Black;
                myCycle.SizeMode = PictureBoxSizeMode.AutoSize;
                rightArrow.SizeMode = PictureBoxSizeMode.AutoSize;
                leftArrow.SizeMode = PictureBoxSizeMode.AutoSize;
                lblName.AutoSize = true; //Label created for player number
                lblName.TextAlign = ContentAlignment.MiddleCenter;
                lblName.Top = selectionPanel.Height / 12;
                lblName.Left = (selectionPanel.Width / 2) - (lblName.Width * 11 / 20);
                lblName.Font = new Font("OCR A Extended", 18);
                lblName.Text = $"Player {playerNumber}";
                lblName.ForeColor = Color.White;
                rightArrow.Tag = "right"; //Tags used to orientate arrows into the correct position
                leftArrow.Tag = "left";
                rightArrow.MouseDown += PictureBox_MouseDown; //Events shrinking arrow on click
                leftArrow.MouseDown += PictureBox_MouseDown;
                leftArrow.MouseUp += PictureBox_MouseDown;
                rightArrow.MouseUp += PictureBox_MouseDown;
                rightArrow.Click += btn_click; //Events for click to change cycle color
                leftArrow.Click += btn_click;
                rightArrow.Image = new Bitmap(arrowimage, new Size(50, 65));
                Bitmap tmp = new Bitmap(rightArrow.Image); //Bitmap used to set left arrow image to the flipped version of the right arrow
                tmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
                leftArrow.Image = tmp;
                myCycle.Top = (selectionPanel.Height / 2) - (myCycle.Height / 2); 
                myCycle.Left = (selectionPanel.Width / 2) - (myCycle.Width / 2);
                rightArrow.Top = (selectionPanel.Height / 2) - (rightArrow.Height / 2);
                leftArrow.Top = (selectionPanel.Height / 2) - (leftArrow.Height / 2);
                rightArrow.Left = (myCycle.Left + myCycle.Width + ((selectionPanel.Width - (myCycle.Left + myCycle.Width)) / 3));
                leftArrow.Left = (myCycle.Left - ((selectionPanel.Width - (selectionPanel.Width - myCycle.Left)) / 3) - leftArrow.Width);
                selectionPanel.Controls.Add(lblName);
                selectionPanel.Controls.Add(rightArrow);
                selectionPanel.Controls.Add(leftArrow);
                selectionPanel.Controls.Add(myCycle);
                pForm.Controls.Add(selectionPanel);
                PForm = pForm;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public int current {get { return currentImage; } }
        private void PictureBox_MouseDown(object sender, EventArgs e)
        {
            try
            {
                Image tmp = new Bitmap(((PictureBox)sender).Image); //image used to change arrow image into smaller version
                if (tmp.Width == 50) //if big shrinks
                {
                    tmp = new Bitmap(tmp, tmp.Width * 3 / 4, tmp.Height * 3 / 4);
                    ((PictureBox)sender).Top += 10;
                }
                else
                {
                    if (((PictureBox)sender).Tag.ToString() == "right") //if Arrow is right and is small goes back to normal
                        tmp = new Bitmap(arrowimage, new Size(50, 65));
                    else
                    {
                        arrowimage.RotateFlip(RotateFlipType.RotateNoneFlipX); //if Arrow is left and is small goes back to normal
                        tmp = new Bitmap(arrowimage, new Size(50, 65));
                    }
                    ((PictureBox)sender).Top -= 10;
                }
            ((PictureBox)sender).Image = tmp;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btn_click(object sender, EventArgs e)
        {
            try
            {
                if (sender is PictureBox)
                {
                    if (((PictureBox)sender).Tag.ToString() == "right") //if right arrow clicked changes currentimage aka current picked cycle and shows the user the new cycle
                    {
                        currentImage += 1;
                        if (currentImage > cycleImages.Count - 1)
                            currentImage = 0;
                        myCycle.Image = cycleImages[currentImage];
                    }
                    if (((PictureBox)sender).Tag.ToString() == "left") //if left arrow clicked changes currentimage aka current picked cycle and shows the user the new cycle
                    {
                        currentImage -= 1;
                        if (currentImage < 0)
                            currentImage = cycleImages.Count - 1;
                        myCycle.Image = cycleImages[currentImage];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private static void Ready_click(object sender, EventArgs e) //Clears this form to save memory and opens arenaform
        {
            try
            {
                foreach (Control c in PForm.Controls) 
                    c.Dispose();
                ArenaForm tmpForm = new ArenaForm();
                PForm.Hide();
                tmpForm.ShowDialog();
                PForm.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
