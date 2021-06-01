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
    public class Projectile
    {
        public static List<Projectile> ProjectileList = new List<Projectile>(); //List of projectiles
        private static List<Projectile> DeletedProjectileList = new List<Projectile>();
        protected static Panel Pn; //Panel added by cycle class to arenaform
        private static Timer ProjectileTimer = new Timer(); //Timer used to move projectile
        private int PlayerNum; //playernumber saved to check which player the projectile belongs to
        protected PictureBox pbProjectile = new PictureBox();
        protected enum ProjectileState { Right, Left, Up, Down }; //Enumerator used to set projectile to specific direction
        protected ProjectileState myProjectile;
        static Projectile()
        {
            try
            {
                ProjectileTimer.Enabled = true;
                ProjectileTimer.Start();
                ProjectileTimer.Interval = 10;
                ProjectileTimer.Tick += timer_tick;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public Projectile(Panel pn, int x, int y, int width, int height, int Player, string direction)
        {
            try
            {
                pbProjectile.SizeMode = PictureBoxSizeMode.AutoSize; //Sets up projectile image
                pbProjectile.BackColor = Color.Transparent;
                PlayerNum = Player;
                Pn = pn;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public int X { get { return pbProjectile.Left; } } //properties for projectile
        public int Y { get { return pbProjectile.Top; } }
        public int Width { get { return pbProjectile.Width; } }
        public int Height { get { return pbProjectile.Height; } }
        public PictureBox pb { get { return pbProjectile; } }
        public int player { get { return PlayerNum; } }
        private static void timer_tick(object sender, EventArgs e)
        {
            try
            {
                foreach (Projectile p in ProjectileList) //Moves projectile based on direction
                {
                    switch (p.myProjectile)
                    {
                        case ProjectileState.Down:
                            p.pbProjectile.Top += 1;
                            break;
                        case ProjectileState.Up:
                            p.pbProjectile.Top -= 1;
                            break;
                        case ProjectileState.Left:
                            p.pbProjectile.Left -= 1;
                            break;
                        case ProjectileState.Right:
                            p.pbProjectile.Left += 1;
                            break;
                    }
                    p.CheckBounds();
                }
                if (DeletedProjectileList.Count > 0) //Delete projectiles that hit bounds
                {
                    for (int i = 0; i < DeletedProjectileList.Count; i++)
                        ProjectileList.Remove(DeletedProjectileList[i]);
                    DeletedProjectileList.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        protected Image SpawnProjectile(Image img, int x, int y, int width, int height, string direction)
        {
            try
            {
                Bitmap image = new Bitmap(img);
                switch (direction) //sets projectile orientation and position
                {
                    case "Left":
                        image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        image = new Bitmap(image, new Size(45, 20));
                        pbProjectile.Left = x - 10;
                        pbProjectile.Top = (y + (height / 2)) - (image.Height / 2);
                        myProjectile = ProjectileState.Left;
                        break;
                    case "Right":
                        image = new Bitmap(image, new Size(45, 20));
                        pbProjectile.Left = x + width + 10;
                        pbProjectile.Top = (y + (height / 2)) - (image.Height / 2);
                        myProjectile = ProjectileState.Right;
                        break;
                    case "Up":
                        image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        image = new Bitmap(image, new Size(20, 45));
                        pbProjectile.Left = (x + (width / 2)) - (image.Width / 2);
                        pbProjectile.Top = (y + (height / 2)) - (image.Height / 2) - 10;
                        myProjectile = ProjectileState.Up;
                        break;
                    case "Down":
                        image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        image = new Bitmap(image, new Size(20, 45));
                        pbProjectile.Left = (x + (width / 2)) - (image.Width / 2);
                        pbProjectile.Top = y + height + 10;
                        myProjectile = ProjectileState.Down;
                        break;
                }
                return image;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        private void CheckBounds() //checks if projectile hits bounds and removes it from the form and sets up for deletion
        {
            try
            {
                if (X > Pn.Width - Width || X < 0)
                {
                    Pn.Controls.Remove(pbProjectile);
                    pbProjectile.Dispose();
                }
                if (Y > Pn.Height - Height || Y < 0)
                {
                    Pn.Controls.Remove(pbProjectile);
                    DeletedProjectileList.Add(this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
    public class BlueProjectile : Projectile //Loads and creates projectiles
    {
        static Image ProjectileImage;
        static BlueProjectile()
        {
            try
            {
                if (!File.Exists("waterball.png"))
                    throw new Exception("Missing Files Please Redownload - Application Closed");
                ProjectileImage = Image.FromFile($"waterball.png");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Exit();
            }
        }
        public BlueProjectile(Panel pn, int x, int y, int width, int height, int Player, string direction) : base(pn, x, y, width, height, Player, direction)
        {
            try
            {
                pbProjectile.Image = SpawnProjectile(ProjectileImage, x, y, width, height, direction);
                pn.Controls.Add(pbProjectile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
    public class GreenProjectile : Projectile
    {
        static Image ProjectileImage;
        static GreenProjectile()
        {
            try
            {
                if (!File.Exists("sludge.png"))
                    throw new Exception("Missing Files Please Redownload - Application Closed");
                ProjectileImage = Image.FromFile($"sludge.png");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Exit();
            }
        }
        public GreenProjectile(Panel pn, int x, int y, int width, int height, int Player, string direction) : base(pn, x, y, width, height, Player, direction)
        {
            try
            {
                pbProjectile.Image = SpawnProjectile(ProjectileImage, x, y, width, height, direction);
                pn.Controls.Add(pbProjectile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
    public class OrangeProjectile : Projectile
    {
        static Image ProjectileImage;
        static OrangeProjectile()
        {
            try
            {
                if (!File.Exists("laser.png"))
                    throw new Exception("Missing Files Please Redownload - Application Closed");
                ProjectileImage = Image.FromFile($"laser.png");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Exit();
            }
        }
        public OrangeProjectile(Panel pn, int x, int y, int width, int height, int Player, string direction) : base(pn, x, y, width, height, Player, direction)
        {
            try
            {
                pbProjectile.Image = SpawnProjectile(ProjectileImage, x, y, width, height, direction);
                pn.Controls.Add(pbProjectile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
    public class PurpleProjectile : Projectile
    {
        static Image ProjectileImage;
        static PurpleProjectile()
        {
            try
            {
                if (!File.Exists("gas.png"))
                    throw new Exception("Missing Files Please Redownload - Application Closed");
                ProjectileImage = Image.FromFile($"gas.png");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Exit();
            }
        }
        public PurpleProjectile(Panel pn, int x, int y, int width, int height, int Player, string direction) : base(pn, x, y, width, height, Player, direction)
        {
            try
            {
                pbProjectile.Image = SpawnProjectile(ProjectileImage, x, y, width, height, direction);
                pn.Controls.Add(pbProjectile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
    public class PinkProjectile : Projectile
    {
        static Image ProjectileImage;
        static PinkProjectile()
        {
            try
            {
                if (!File.Exists("heart.png"))
                    throw new Exception("Missing Files Please Redownload - Application Closed");
                ProjectileImage = Image.FromFile($"heart.png");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Exit();
            }
        }
        public PinkProjectile(Panel pn, int x, int y, int width, int height, int Player, string direction) : base(pn, x, y, width, height, Player, direction)
        {
            try
            {
                pbProjectile.Image = SpawnProjectile(ProjectileImage, x, y, width, height, direction);
                pn.Controls.Add(pbProjectile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
    public class RedProjectile : Projectile
    {
        static Image ProjectileImage;
        static RedProjectile()
        {
            try
            {
                if (!File.Exists("fireball.png"))
                    throw new Exception("Missing Files Please Redownload - Application Closed");
                ProjectileImage = Image.FromFile($"fireball.png");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Exit();
            }
        }
        public RedProjectile(Panel pn, int x, int y, int width, int height, int Player, string direction) : base(pn, x, y, width, height, Player, direction)
        {
            try
            {
                pbProjectile.Image = SpawnProjectile(ProjectileImage, x, y, width, height, direction);
                pbProjectile.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
    public class WhiteProjectile : Projectile
    {
        static Image ProjectileImage;
        static WhiteProjectile()
        {
            try
            {
                if (!File.Exists("icebolt.png"))
                    throw new Exception("Missing Files Please Redownload - Application Closed");
                ProjectileImage = Image.FromFile($"icebolt.png");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Exit();
            }
        }
        public WhiteProjectile(Panel pn, int x, int y, int width, int height, int Player, string direction) : base(pn, x, y, width, height, Player, direction)
        {
            try
            {
                pbProjectile.Image = SpawnProjectile(ProjectileImage, x, y, width, height, direction);
                pn.Controls.Add(pbProjectile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
    public class YellowProjectile : Projectile
    {
        static Image ProjectileImage;
        static YellowProjectile()
        {
            try
            {
                if (!File.Exists("lightningbolt.png"))
                    throw new Exception("Missing Files Please Redownload - Application Closed");
                ProjectileImage = Image.FromFile($"lightningbolt.png");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Exit();
            }
        }
        public YellowProjectile(Panel pn, int x, int y, int width, int height, int Player, string direction) : base(pn, x, y, width, height, Player, direction)
        {
            try
            {
                pbProjectile.Image = SpawnProjectile(ProjectileImage, x, y, width, height, direction);
                pn.Controls.Add(pbProjectile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }//
    }
}
