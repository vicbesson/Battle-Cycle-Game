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
    class PowerUp
    {
        private static List<Image> PowerUpImages = new List<Image>();
        private static Random rng = new Random();
        private PictureBox pbPower = new PictureBox();
        private Panel Pn;
        static PowerUp()
        {
            try
            {
                for (int i = 1; i < 5; i++) //Loads PowerUp images
                {
                    if (!File.Exists($"PowerUp{i}.png"))
                        throw new Exception("Missing Files Please Redownload - Application Closed");
                    PowerUpImages.Add(Image.FromFile($"PowerUp{i}.png"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Exit();
            }
        }
        public PowerUp(Panel pn)
        {
            try
            {
                pbPower.SizeMode = PictureBoxSizeMode.AutoSize; //Sets up powerup image
                pbPower.BackColor = Color.Transparent;
                Pn = pn;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public int X { get { return pbPower.Left; } } //properties to be used for powerups
        public int Y { get { return pbPower.Top; } }
        public int Width { get { return pbPower.Width; } }
        public int Height { get { return pbPower.Height; } }
        public PictureBox PUImage { get { return pbPower; } }
        public virtual void Hit()
        {
            try
            {
                Pn.Controls.Remove(pbPower); //removes power up from cycle play area panel in arenaform
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public class SlowTime : PowerUp //Sets powerup image to the corresponding powerup type and sets location and adds it to cycle play area panel in arenaform
        {
            public SlowTime(Panel pn) : base(pn)
            {
                try
                {
                    pbPower.Image = new Bitmap(PowerUpImages[0], new Size(40, 40));
                    pbPower.Left = rng.Next(0, pn.Width - pbPower.Width + 1);
                    pbPower.Top = rng.Next(0, pn.Height - pbPower.Height + 1);
                    pn.Controls.Add(pbPower);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        public class SpeedUp : PowerUp
        {
            public SpeedUp(Panel pn) : base(pn)
            {
                try
                {
                    pbPower.Image = new Bitmap(PowerUpImages[1], new Size(40, 40));
                    pbPower.Left = rng.Next(0, pn.Width - pbPower.Width + 1);
                    pbPower.Top = rng.Next(0, pn.Height - pbPower.Height + 1);
                    pn.Controls.Add(pbPower);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        public class Immunity : PowerUp
        {
            public Immunity(Panel pn) : base(pn)
            {
                try
                {
                    pbPower.Image = new Bitmap(PowerUpImages[2], new Size(40, 40));
                    pbPower.Left = rng.Next(0, pn.Width - pbPower.Width + 1);
                    pbPower.Top = rng.Next(0, pn.Height - pbPower.Height + 1);
                    pn.Controls.Add(pbPower);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        public class Thorns : PowerUp
        {
            public Thorns(Panel pn) : base(pn)
            {
                try
                {
                    pbPower.Image = new Bitmap(PowerUpImages[3], new Size(40, 40));
                    pbPower.Left = rng.Next(0, pn.Width - pbPower.Width + 1);
                    pbPower.Top = rng.Next(0, pn.Height - pbPower.Height + 1);
                    pn.Controls.Add(pbPower);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }//
    }
}
