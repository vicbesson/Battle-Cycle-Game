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
    public class Cycle
    {
        public delegate void CycleMessageHandler(string message);
        public static event CycleMessageHandler CycleMessage; //Used for sound
        private static List<Image> destructionImages = new List<Image>();
        protected static List<Image> cycleImages = new List<Image>();
        public static List<Cycle> Players = new List<Cycle>();
        protected static List<Image> EffectImages = new List<Image>();
        private static Timer GameTimer = new Timer();
        private static bool GameStart = true;
        private static bool GameOver = false;
        public static Panel PlayArea = new Panel();
        public static int NumPlayers;
        public static int counter; 
        private static Label lblStart = new Label();
        private static Form PForm;
        private static int PowerUpSpawnTime = 166; //Delay for spawning PowerUp
        private static List<PowerUp> PowerUpList = new List<PowerUp>();
        private int PowerUpLength = 166; //Length PowerUp lasts
        private Timer ExplosionTimer = new Timer();
        private PictureBox pbExplosion = new PictureBox();
        private bool AbleToFire = true;
        private int ProjectileDelay = 33; //Delay for firing: at ~one second
        private int speed = 5; //Speed of cycle
        private int ExplosionFrame = 0; //at what frame the explosion is at
        protected Image cycleImage;
        private int playerNumber;
        protected PictureBox pbCycle = new PictureBox();
        private PictureBox EffectImage = new PictureBox();
        private enum CycleState { Hit, Exploding, Right, Left, Up, Down }; //States for cycle
        private CycleState myCycle;
        private enum PowerState { Slowed, SpeedUp, Normal, Immune, Thorns } //States for effect on cycle
        private PowerState myPower;
        private int X { get { return pbCycle.Left; } set { pbCycle.Left = value; } } //Properties to get and set player image
        private int Y { get { return pbCycle.Top; } set { pbCycle.Top = value; } }
        private int Width { get { return pbCycle.Width; } set { pbCycle.Width = value; } }
        private int Height { get { return pbCycle.Height; } set { pbCycle.Height = value; } }//
        public int playerNum { get { return playerNumber; } }
        static Cycle() //Loads Images for effects and Explosion and starts game timer
        {
            try
            {
                for (int i = 0; i < 15; i++)
                {
                    if (!File.Exists($"Explosion{i}.png"))
                        throw new Exception("Missing Files Please Redownload - Application Closed");
                    destructionImages.Add(new Bitmap($"Explosion{i}.png"));
                }
                for(int i = 0; i < 8; i++)
                {
                    if(!File.Exists($"Tron{i}.png"))
                        throw new Exception("Missing Files Please Redownload - Application Closed");
                    cycleImages.Add(Image.FromFile($"Tron{i}.png"));
                }
                if (!File.Exists($"NoEffect.png") || !File.Exists($"Slowed.png") || !File.Exists($"Immunity.png") || !File.Exists($"Speed.png") || !File.Exists($"Thorns.png"))
                    throw new Exception("Missing Files Please Redownload - Application Closed");
                EffectImages.Add(Image.FromFile("NoEffect.png"));
                EffectImages.Add(Image.FromFile("Slowed.png"));
                EffectImages.Add(Image.FromFile("Immunity.png"));
                EffectImages.Add(Image.FromFile("Speed.png"));
                EffectImages.Add(Image.FromFile("Thorns.png"));
                GameTimer.Interval = 1000;
                GameTimer.Start();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Exit();
            }
        }
        public Cycle(Form pForm)
        {
            try
            {
                playerNumber = NumPlayers - counter;//Set Up Player Numbers
                if(NumPlayers > 1)
                counter--;
                pForm.KeyDown += Cycle_KeyDown;
                PlayArea.Width = pForm.ClientSize.Width; //Setup Panel used for play area
                PlayArea.Height = pForm.ClientSize.Height - 109;
                PlayArea.Left = 0;
                PlayArea.Top = 109;//
                switch (playerNumber) //Sets Spawn position based on player number
                {
                    case 1:
                        myCycle = CycleState.Right;
                        pbCycle.Left = (PlayArea.Width / 8) * 2;
                        pbCycle.Top = (PlayArea.Height / 8) * 2;
                        break;
                    case 2:
                        myCycle = CycleState.Left;
                        pbCycle.Left = (PlayArea.Width / 8) * 6 - 32;
                        pbCycle.Top = (PlayArea.Height / 8) * 2;
                        break;
                    case 3:
                        myCycle = CycleState.Right;
                        pbCycle.Left = (PlayArea.Width / 8) * 2;
                        pbCycle.Top = (PlayArea.Height / 8) * 6;
                        break;
                    case 4:
                        myCycle = CycleState.Left;
                        pbCycle.Left = (PlayArea.Width / 8) * 6 - 32;
                        pbCycle.Top = (PlayArea.Height / 8) * 6;
                        break;
                }//
                if (counter == 0) //Sorta like a static constructor only happens once
                {
                    PForm = pForm;
                    GameTimer.Tick += timer_tick;
                    pForm.Controls.Add(PlayArea);
                    lblStart.AutoSize = true; //sets up start label for countdown
                    lblStart.TextAlign = ContentAlignment.MiddleCenter;
                    lblStart.Text = "3";
                    lblStart.Font = new Font("OCR A", 24, FontStyle.Bold);
                    lblStart.Width = TextRenderer.MeasureText(lblStart.Text, lblStart.Font, lblStart.MaximumSize).Width;
                    lblStart.Height = TextRenderer.MeasureText(lblStart.Text, lblStart.Font, lblStart.MaximumSize).Height;
                    lblStart.AutoSize = true;
                    lblStart.ForeColor = Color.White;
                    lblStart.Left = (PlayArea.Width / 2) - (lblStart.Width / 2);
                    lblStart.Top = PlayArea.Height / 2;//
                    PlayArea.Controls.Add(lblStart);
                    PlayArea.BackColor = Color.DimGray;
                }
                pbCycle.SizeMode = PictureBoxSizeMode.AutoSize;//Setup for player image
                pbCycle.BackColor = Color.Transparent;//
                myPower = PowerState.Normal;
                EffectImage.SizeMode = PictureBoxSizeMode.AutoSize; //Setup for Effect Image
                EffectImage.BackColor = Color.Transparent;
                pbCycle.Controls.Add(EffectImage);//
                pbExplosion.SizeMode = PictureBoxSizeMode.AutoSize;
                pbExplosion.BackColor = Color.Transparent;
                ExplosionTimer.Interval = 100; //Setup for Explosion Timer
                ExplosionTimer.Tick += Explosion_tick;//
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private static void timer_tick(object sender, EventArgs e) //Main Game Timer
        {
            try
            {
                if (GameStart) //Starts count down
                    GameBegin();
                else if (GameOver) //If players are equal to one or less then GAMEOVER
                {
                    GameTimer.Enabled = false;
                    lblStart.Text = "Game Over";
                    lblStart.Left = (PlayArea.Width / 2) - (lblStart.Width / 2);
                    lblStart.Show();
                    Button btnTryAgain = new Button();
                    btnTryAgain.Text = "Try Again";
                    btnTryAgain.Width = 150;
                    btnTryAgain.Height = 40;
                    btnTryAgain.Font = new Font("OCR A", 15, FontStyle.Bold);
                    btnTryAgain.FlatStyle = FlatStyle.Flat;
                    btnTryAgain.BackColor = Color.Aqua;
                    btnTryAgain.TextAlign = ContentAlignment.MiddleCenter;
                    btnTryAgain.Left = (PlayArea.Width / 2) - (btnTryAgain.Width / 2);
                    btnTryAgain.Top = lblStart.Top + lblStart.Height + 5;
                    btnTryAgain.Click += TryAgain_click;
                    PlayArea.Controls.Add(btnTryAgain);
                    foreach (Projectile p in Projectile.ProjectileList) //Removes all controls
                        PlayArea.Controls.Remove(p.pb);
                    Projectile.ProjectileList.Clear();
                    foreach (PowerUp p in PowerUpList)
                        PlayArea.Controls.Remove(p.PUImage);
                    PowerUpList.Clear();
                    foreach (Cycle c in Players)
                        if (c.pbCycle != null)
                            PlayArea.Controls.Remove(c.pbCycle);
                    Players.Clear();//
                }
                else
                {
                    foreach (Cycle c in Players)
                    {
                        if (c.pbCycle != null)
                        {
                            switch (c.myCycle) //Moves Cycles
                            {
                                case CycleState.Right:
                                    c.X += c.speed;
                                    break;
                                case CycleState.Left:
                                    c.X -= c.speed;
                                    break;
                                case CycleState.Up:
                                    c.Y -= c.speed;
                                    break;
                                case CycleState.Down:
                                    c.Y += c.speed;
                                    break;//
                                case CycleState.Hit: //Starts Explosion and deletes player image
                                    c.myCycle = CycleState.Exploding;
                                    c.pbExplosion.Top = c.pbCycle.Top;
                                    c.pbExplosion.Left = c.pbCycle.Left;
                                    c.pbCycle = null;
                                    PlayArea.Controls.Add(c.pbExplosion);
                                    c.ExplosionTimer.Start();
                                    continue;//
                            }
                            switch (c.myPower) //Checks if slowed or sped up and eventually leads to the removal of the powerup
                            {
                                case PowerState.Slowed:
                                    c.PowerUpLength--;
                                    if (c.PowerUpLength == 0)
                                    {
                                        c.myPower = PowerState.Normal;
                                        c.speed = 5;
                                        c.PowerUpLength = 166;
                                        c.SpawnPosition(c.cycleImage, c.EffectImage.Image);
                                    }
                                    break;
                                case PowerState.SpeedUp:
                                    c.PowerUpLength--;
                                    if (c.PowerUpLength == 0)
                                    {
                                        c.myPower = PowerState.Normal;
                                        c.speed = 5;
                                        c.PowerUpLength = 166;
                                        c.SpawnPosition(c.cycleImage, c.EffectImage.Image);
                                    }
                                    break;
                                case PowerState.Thorns:
                                    c.PowerUpLength--;
                                    if (c.PowerUpLength == 0)
                                    {
                                        c.myPower = PowerState.Normal;
                                        c.PowerUpLength = 166;
                                        c.SpawnPosition(c.cycleImage, c.EffectImage.Image);
                                    }
                                    break;
                            }//
                            if (c.AbleToFire == false) //Sets Up Projectile Firerate 
                            {
                                c.ProjectileDelay--;
                                if (c.ProjectileDelay == 0)
                                {
                                    c.AbleToFire = true;
                                    c.ProjectileDelay = 33;
                                }
                            }//
                            c.CheckBounds(); //Checks if out of bounds
                        }
                    }
                    CheckPowerUp();  //Check if picking up PowerUp
                    CheckForCrash(); //Check for player on player collison
                    PowerUpSpawnTime--;
                    if (PowerUpSpawnTime == 0)
                    {
                        Random rng = new Random();
                        int SpawnPU = rng.Next(0, 4);
                        switch (SpawnPU)
                        {
                            case 0:
                                PowerUpList.Add(new PowerUp.SpeedUp(PlayArea));
                                break;
                            case 1:
                                PowerUpList.Add(new PowerUp.SlowTime(PlayArea));
                                break;
                            case 2:
                                PowerUpList.Add(new PowerUp.Immunity(PlayArea));
                                break;
                            case 3:
                                PowerUpList.Add(new PowerUp.Thorns(PlayArea));
                                break;
                        }
                        PowerUpSpawnTime = 166;//
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private static void TryAgain_click(object sender, EventArgs e) //Restarts the app
        {
            try
            {
                Application.Restart();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Explosion_tick(object sender, EventArgs e) //Tick for Explosions that eventually delete the player and its controls on the panel
        {
            try
            {
                Bitmap temp = new Bitmap(destructionImages[ExplosionFrame]);
                pbExplosion.Image = new Bitmap(temp, new Size(50, 50));
                if (ExplosionFrame == 14)
                {
                    ExplosionTimer.Enabled = false;
                    PlayArea.Controls.Remove(pbExplosion);
                    pbExplosion.Image = null;
                    Players.Remove(this);
                    if (Players.Count <= 1)
                    {
                        if (Players.Count == 1)
                        {
                            if (ArenaForm.PlayerInfo[Players[0].playerNumber - 1].Controls.Count < 2)
                            {
                                Label lbltmp = new Label(); //Says Last Player Alive if there is one to winner
                                lbltmp.Text = "Winner";
                                lbltmp.Font = new Font("OCR A", 18, FontStyle.Bold);
                                lbltmp.Width = TextRenderer.MeasureText(lbltmp.Text, lbltmp.Font, lbltmp.MaximumSize).Width;
                                lbltmp.ForeColor = Color.Blue;
                                lbltmp.AutoSize = true;
                                lbltmp.Top = (ArenaForm.PlayerInfo[Players[0].playerNum - 1].Height * 2 / 3) - (lbltmp.Height / 2);
                                lbltmp.Left = (ArenaForm.PlayerInfo[Players[0].playerNum - 1].Width / 2) - (lbltmp.Width / 2);
                                ArenaForm.PlayerInfo[Players[0].playerNum - 1].Controls.Add(lbltmp);
                            }
                        }
                        GameOver = true;

                    }
                }
                ExplosionFrame++;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private static void GameBegin() //Happens at beginning of game and shows the start label counting down to go
        {
            try
            {
                if (lblStart.Text == "Go")
                {
                    GameStart = false;
                    lblStart.Text = "";
                    lblStart.Visible = false;
                    GameTimer.Interval = 30;
                    return;
                }
                int temp = Convert.ToInt32(lblStart.Text) - 1;
                if (temp == 0)
                {
                    lblStart.Text = "Go";
                }
                else
                    lblStart.Text = temp.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Cycle_KeyDown(object sender, KeyEventArgs e) //Changes Cycle Direction and Spawns Projectile Based On Player Number
        {
            try
            {
                if (GameStart == false && (myCycle == CycleState.Left || myCycle == CycleState.Right || myCycle == CycleState.Up || myCycle == CycleState.Down) && GameOver == false)
                {
                    switch (playerNumber)
                    {
                        case 1:
                            if (e.KeyCode == Keys.W || e.KeyCode == Keys.A || e.KeyCode == Keys.S || e.KeyCode == Keys.D)
                            {
                                if (e.KeyCode == Keys.W && myCycle != CycleState.Down && myCycle != CycleState.Up)
                                {
                                    myCycle = CycleState.Up;
                                }
                                else if (e.KeyCode == Keys.A && myCycle != CycleState.Right && myCycle != CycleState.Left)
                                {
                                    myCycle = CycleState.Left;
                                }
                                else if (e.KeyCode == Keys.S && myCycle != CycleState.Up && myCycle != CycleState.Down)
                                {
                                    myCycle = CycleState.Down;
                                }
                                else if (e.KeyCode == Keys.D && myCycle != CycleState.Left && myCycle != CycleState.Right)
                                {
                                    myCycle = CycleState.Right;
                                }
                                pbCycle.Image = SpawnPosition(cycleImage, EffectImage.Image);
                            }
                            if (e.KeyCode == Keys.E && AbleToFire)
                            {
                                SpawnProjectile();
                            }
                            break;
                        case 2:
                            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Left || e.KeyCode == Keys.Down || e.KeyCode == Keys.Right)
                            {
                                if (e.KeyCode == Keys.Up && myCycle != CycleState.Down && myCycle != CycleState.Up)
                                {
                                    myCycle = CycleState.Up;
                                }
                                else if (e.KeyCode == Keys.Left && myCycle != CycleState.Right && myCycle != CycleState.Left)
                                {
                                    myCycle = CycleState.Left;
                                }
                                else if (e.KeyCode == Keys.Down && myCycle != CycleState.Up && myCycle != CycleState.Down)
                                {
                                    myCycle = CycleState.Down;
                                }
                                else if (e.KeyCode == Keys.Right && myCycle != CycleState.Left && myCycle != CycleState.Right)
                                {
                                    myCycle = CycleState.Right;
                                }
                                pbCycle.Image = SpawnPosition(cycleImage, EffectImage.Image);
                            }
                            if (e.KeyCode == Keys.NumPad0 && AbleToFire)
                            {
                                SpawnProjectile();
                            }
                            break;
                        case 3:
                            if (e.KeyCode == Keys.Y || e.KeyCode == Keys.G || e.KeyCode == Keys.H || e.KeyCode == Keys.J)
                            {
                                if (e.KeyCode == Keys.Y && myCycle != CycleState.Down && myCycle != CycleState.Up)
                                {
                                    myCycle = CycleState.Up;
                                }
                                else if (e.KeyCode == Keys.G && myCycle != CycleState.Right && myCycle != CycleState.Left)
                                {
                                    myCycle = CycleState.Left;
                                }
                                else if (e.KeyCode == Keys.H && myCycle != CycleState.Up && myCycle != CycleState.Down)
                                {
                                    myCycle = CycleState.Down;
                                }
                                else if (e.KeyCode == Keys.J && myCycle != CycleState.Left && myCycle != CycleState.Right)
                                {
                                    myCycle = CycleState.Right;
                                }
                                pbCycle.Image = SpawnPosition(cycleImage, EffectImage.Image);
                            }
                            if (e.KeyCode == Keys.U && AbleToFire)
                            {
                                SpawnProjectile();
                            }
                            break;
                        case 4:
                            if (e.KeyCode == Keys.P || e.KeyCode == Keys.L || e.KeyCode == Keys.OemSemicolon || e.KeyCode == Keys.OemQuotes)
                            {
                                if (e.KeyCode == Keys.P && myCycle != CycleState.Down && myCycle != CycleState.Up)
                                {
                                    myCycle = CycleState.Up;
                                }
                                else if (e.KeyCode == Keys.L && myCycle != CycleState.Right && myCycle != CycleState.Left)
                                {
                                    myCycle = CycleState.Left;
                                }
                                else if (e.KeyCode == Keys.OemSemicolon && myCycle != CycleState.Up && myCycle != CycleState.Down)
                                {
                                    myCycle = CycleState.Down;
                                }
                                else if (e.KeyCode == Keys.OemQuotes && myCycle != CycleState.Left && myCycle != CycleState.Right)
                                {
                                    myCycle = CycleState.Right;
                                }
                                pbCycle.Image = SpawnPosition(cycleImage, EffectImage.Image);
                            }
                            if (e.KeyCode == Keys.OemOpenBrackets && AbleToFire)
                            {
                                SpawnProjectile();
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void SpawnProjectile() //Spawns in Projectile Based on Player Color
        {
            try
            {
                if (this is BlueCycle)
                {
                    Projectile.ProjectileList.Add(new BlueProjectile(PlayArea, X, Y, Width, Height, playerNumber, myCycle.ToString()));
                }
                else if (this is GreenCycle)
                {
                    Projectile.ProjectileList.Add(new GreenProjectile(PlayArea, X, Y, Width, Height, playerNumber, myCycle.ToString()));
                }
                else if (this is OrangeCycle)
                {
                    Projectile.ProjectileList.Add(new OrangeProjectile(PlayArea, X, Y, Width, Height, playerNumber, myCycle.ToString()));
                }
                else if (this is PurpleCycle)
                {
                    Projectile.ProjectileList.Add(new PurpleProjectile(PlayArea, X, Y, Width, Height, playerNumber, myCycle.ToString()));
                }
                else if (this is PinkCycle)
                {
                    Projectile.ProjectileList.Add(new PinkProjectile(PlayArea, X, Y, Width, Height, playerNumber, myCycle.ToString()));
                }
                else if (this is RedCycle)
                {
                    Projectile.ProjectileList.Add(new RedProjectile(PlayArea, X, Y, Width, Height, playerNumber, myCycle.ToString()));
                }
                else if (this is WhiteCycle)
                {
                    Projectile.ProjectileList.Add(new WhiteProjectile(PlayArea, X, Y, Width, Height, playerNumber, myCycle.ToString()));

                }
                else if (this is YellowCycle)
                {
                    Projectile.ProjectileList.Add(new YellowProjectile(PlayArea, X, Y, Width, Height, playerNumber, myCycle.ToString()));
                }
                AbleToFire = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        protected Image SpawnPosition(Image img, Image img2) //Changes Direction of Player Image and the corresponing effect image
        {
            try
            {
                Bitmap image = new Bitmap(img);
                Bitmap image2 = new Bitmap(img2);
                switch (myPower)
                {
                    case PowerState.Normal:
                        image2 = new Bitmap(EffectImages[0]);
                        break;
                    case PowerState.Slowed:
                        image2 = new Bitmap(EffectImages[1]);
                        break;
                    case PowerState.Immune:
                        image2 = new Bitmap(EffectImages[2]);
                        break;
                    case PowerState.SpeedUp:
                        image2 = new Bitmap(EffectImages[3]);
                        break;
                    case PowerState.Thorns:
                        image2 = new Bitmap(EffectImages[4]);
                        break;
                }
                switch (myCycle)
                {
                    case CycleState.Left:
                        image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        image2.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        image2 = new Bitmap(image2, new Size(64, 20));
                        image = new Bitmap(image, new Size(64, 20));
                        break;
                    case CycleState.Right:
                        image = new Bitmap(image, new Size(64, 20));
                        image2 = new Bitmap(image2, new Size(64, 20));
                        break;
                    case CycleState.Up:
                        image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        image2.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        image2 = new Bitmap(image2, new Size(20, 64));
                        image = new Bitmap(image, new Size(20, 64));
                        break;
                    case CycleState.Down:
                        image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        image2.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        image2 = new Bitmap(image2, new Size(20, 64));
                        image = new Bitmap(image, new Size(20, 64));
                        break;
                }
                EffectImage.Image = new Bitmap(image2);
                return image;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        private static void CheckForCrash() //Check if Players Collide or if Players Collide With Enemy Projectile
        {
            try
            {
                if (Players.Count >= 2)
                {
                    for (int i = 0; i < Players.Count; i++)
                        for (int j = i; j < Players.Count; j++)
                            if (i != j && Players[i].pbCycle != null && Players[j].pbCycle != null)
                                if (CrashTest(Players[i], Players[j]))
                                {
                                    Players[i].Collision();
                                    Players[j].Collision();
                                }
                }
                if (Projectile.ProjectileList.Count > 0)
                {
                    for (int i = 0; i < Players.Count; i++)
                        for (int j = 0; j < Projectile.ProjectileList.Count; j++)
                            if (Players[i].pbCycle != null && Projectile.ProjectileList[j].player != Players[i].playerNumber)
                                if (HitTest(Players[i], Projectile.ProjectileList[j]))
                                {
                                    if (Players[i].myPower == PowerState.SpeedUp) //if sped up still dies
                                    {
                                        Players[i].myPower = PowerState.Normal;
                                        Players[i].Collision();
                                    }
                                    else if (Players[i].myPower == PowerState.Thorns) //if player has thorns kills the owner of the projectile
                                    {
                                        if (Players[Projectile.ProjectileList[j].player - 1].myPower == PowerState.Thorns || Players[Projectile.ProjectileList[j].player - 1].myPower == PowerState.Immune) //if the reflected player has thorns or shield loses shield/thorns and goes to normal
                                            Players[Projectile.ProjectileList[j].player - 1].myPower = PowerState.Normal;
                                        else
                                            Players[Projectile.ProjectileList[j].player - 1].Collision(); //Kills reflected player
                                    }
                                    else
                                        Players[i].Collision();
                                    Players[i].myPower = PowerState.Normal; //No matter who the projectile hits they should go back to normal
                                    PlayArea.Controls.Remove(Projectile.ProjectileList[j].pb); //Delete projectile
                                    Projectile.ProjectileList.Remove(Projectile.ProjectileList[j]);
                                }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void CheckBounds() //Check if player hits boundary
        {
            try
            {
                if (pbCycle.Left > PlayArea.Width - pbCycle.Width || pbCycle.Left < 0)
                {
                    myPower = PowerState.Normal;
                    Collision();
                }
                if (pbCycle.Top > PlayArea.Height - pbCycle.Height || pbCycle.Top < 0)
                {
                    myPower = PowerState.Normal;
                    Collision();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        private void Collision() //Changes Player to the hit state, if not immune
        {
            try
            {
                if (myPower != PowerState.Immune && myPower != PowerState.SpeedUp)
                {
                    if (myCycle != CycleState.Exploding)
                    {
                        myCycle = CycleState.Hit;
                        CycleMessage?.Invoke("Explosion.wav");
                        PlayArea.Controls.Remove(pbCycle);
                        Label lbltmp = new Label();
                        lbltmp.Text = "Dead"; //Sets player label to dead
                        lbltmp.Font = new Font("OCR A", 18, FontStyle.Bold);
                        lbltmp.Width = TextRenderer.MeasureText(lbltmp.Text, lbltmp.Font, lbltmp.MaximumSize).Width;
                        lbltmp.ForeColor = Color.Red;
                        lbltmp.AutoSize = true;
                        lbltmp.Top = (ArenaForm.PlayerInfo[playerNumber - 1].Height * 2 / 3) - (lbltmp.Height / 2);
                        lbltmp.Left = (ArenaForm.PlayerInfo[playerNumber - 1].Width / 2) - (lbltmp.Width / 2);
                        ArenaForm.PlayerInfo[playerNumber - 1].Controls.Add(lbltmp);
                    }
                }
                else if (myPower == PowerState.Immune)
                {
                    myPower = PowerState.Normal;
                    pbCycle.Image = SpawnPosition(cycleImage, EffectImage.Image);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void PowerUpCycle(PowerUp pu) //Gives cycles different power ups based on PowerUp picked up
        {
            try
            {
                if (pu is PowerUp.SlowTime)
                {
                    CycleMessage?.Invoke("slowtime.wav");
                    for (int i = 0; i < Players.Count; i++)
                    {
                        if (Players[i].playerNumber != playerNumber)
                        {
                            if (Players[i].myPower == PowerState.Immune) //Removes immune shield on slowtime
                                Players[i].myPower = PowerState.Normal;
                            else if (Players[i].myPower == PowerState.Thorns) //Thorns Reflects Slow onto play who picked it up
                            {
                                myPower = PowerState.Slowed;
                                PowerUpLength = 166;
                                speed = 1;
                            }
                            else
                            {
                                Players[i].myPower = PowerState.Slowed;
                                Players[i].speed = 1;
                                Players[i].PowerUpLength = 166;
                            }
                            if (Players[i].pbCycle != null)
                                Players[i].pbCycle.Image = Players[i].SpawnPosition(Players[i].cycleImage, Players[i].EffectImage.Image);
                        }
                    }
                }
                if (pu is PowerUp.SpeedUp) //Set speeds and reset poweruplength
                {
                    CycleMessage?.Invoke("speed.wav");
                    myPower = PowerState.SpeedUp;
                    speed = 9;
                    pbCycle.Image = SpawnPosition(cycleImage, EffectImage.Image);
                    PowerUpLength = 166;
                }
                if (pu is PowerUp.Immunity) //Sets to immune and reset poweruplength
                {
                    CycleMessage?.Invoke("shield.wav");
                    myPower = PowerState.Immune;
                    speed = 5;
                    pbCycle.Image = SpawnPosition(cycleImage, EffectImage.Image);
                    PowerUpLength = 166;
                }
                if (pu is PowerUp.Thorns) 
                {
                    CycleMessage?.Invoke("thorns.wav");  //Sets to thorns and reset poweruplength
                    myPower = PowerState.Thorns;
                    speed = 5;
                    pbCycle.Image = SpawnPosition(cycleImage, EffectImage.Image);
                    PowerUpLength = 166;
                }
                pu.Hit(); //Deletes PowerUp
                PowerUpList.Remove(pu);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private static void CheckPowerUp() //Checks if player hit PowerUp
        {
            try
            {
                if (PowerUpList.Count > 0)
                {
                    for (int i = 0; i < Players.Count; i++)
                        for (int j = 0; j < PowerUpList.Count; j++)
                            if (Players[i].pbCycle != null)
                                if (PowerUpTest(Players[i], PowerUpList[j]))
                                {
                                    Players[i].PowerUpCycle(PowerUpList[j]);
                                }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private static bool PowerUpTest(Cycle One, PowerUp Two) //Checks if Players Collide
        {
            try
            {
                if (One.X + One.Width < Two.X)
                    return false;
                if (Two.X + Two.Width < One.X)
                    return false;
                if (One.Y + One.Height < Two.Y)
                    return false;
                if (Two.Y + Two.Height < One.Y)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        private static bool CrashTest(Cycle One, Cycle Two) //Checks if Players Collide
        {
            try
            {
                if (One.X + One.Width < Two.X)
                    return false;
                if (Two.X + Two.Width < One.X)
                    return false;
                if (One.Y + One.Height < Two.Y)
                    return false;
                if (Two.Y + Two.Height < One.Y)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        private static bool HitTest(Cycle One, Projectile Two) //Checks if a player collides with a projectile
        {
            try
            {
                if (One.X + One.Width < Two.X)
                    return false;
                if (Two.X + Two.Width < One.X)
                    return false;
                if (One.Y + One.Height < Two.Y)
                    return false;
                if (Two.Y + Two.Height < One.Y)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
    }
    //Spawns different cycles and makes there image equal to the cycle color
    public class BlueCycle : Cycle
    {
        public BlueCycle(Form pForm) : base(pForm)
        {
            try
            {
                cycleImage = cycleImages[0];
                pbCycle.Image = SpawnPosition(cycleImage, EffectImages[0]);
                PlayArea.Controls.Add(pbCycle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
    public class GreenCycle : Cycle
    {

        public GreenCycle(Form pForm) : base(pForm)
        {
            try
            {
                cycleImage = cycleImages[1];
                pbCycle.Image = SpawnPosition(cycleImage, EffectImages[0]);
                PlayArea.Controls.Add(pbCycle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
    public class OrangeCycle : Cycle
    {
        public OrangeCycle(Form pForm) : base(pForm)
        {
            try
            {
                cycleImage = cycleImages[2];
                pbCycle.Image = SpawnPosition(cycleImage, EffectImages[0]);
                PlayArea.Controls.Add(pbCycle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
    public class PurpleCycle : Cycle
    {
        public PurpleCycle(Form pForm) : base(pForm)
        {
            try
            {
                cycleImage = cycleImages[3];
                pbCycle.Image = SpawnPosition(cycleImage, EffectImages[0]);
                PlayArea.Controls.Add(pbCycle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
    public class PinkCycle : Cycle
    {
        public PinkCycle(Form pForm) : base(pForm)
        {
            try
            {
                cycleImage = cycleImages[4];
                pbCycle.Image = SpawnPosition(cycleImage, EffectImages[0]);
                PlayArea.Controls.Add(pbCycle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
    public class RedCycle : Cycle
    {
        public RedCycle(Form pForm) : base(pForm)
        {
            try
            {
                cycleImage = cycleImages[5];
                pbCycle.Image = SpawnPosition(cycleImage, EffectImages[0]);
                PlayArea.Controls.Add(pbCycle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
    public class WhiteCycle : Cycle
    {
        public WhiteCycle(Form pForm) : base(pForm)
        {
            try
            {
                cycleImage = cycleImages[6];
                pbCycle.Image = SpawnPosition(cycleImage, EffectImages[0]);
                PlayArea.Controls.Add(pbCycle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
    public class YellowCycle : Cycle
    {
        public YellowCycle(Form pForm) : base(pForm)
        {
            try
            {
                cycleImage = cycleImages[7];
                pbCycle.Image = SpawnPosition(cycleImage, EffectImages[0]);
                PlayArea.Controls.Add(pbCycle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
