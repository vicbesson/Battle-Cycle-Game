using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace Prog225FinalProject___Victor_Besson
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try //Error checking to make sure all files are present also have this style of error checking throughtout form to check if the files still exist
            {
                for(int i = 0; i < 15; i++)
                {
                    if (!File.Exists($"Explosion{i}.png"))
                        throw new Exception("Missing Files Please Re-Download - Application Must Close");
                }
                for(int i = 0; i < 8; i++)
                {
                    if (!File.Exists($"Tron{i}.png"))
                        throw new Exception("Missing Files Please Re-Download - Application Must Close");
                }
                for(int i = 1; i < 5; i++)
                {
                    if (!File.Exists($"PowerUp{i}.png"))
                        throw new Exception("Missing Files Please Re-Download - Application Must Close");
                }
                if (!File.Exists("NoEffect.png") || !File.Exists("Slowed.png") || !File.Exists("Immunity.png") || !File.Exists("Speed.png") || !File.Exists("Thorns.png")
                    || !File.Exists("fireball.png") || !File.Exists("gas.png") || !File.Exists("heart.png") || !File.Exists("icebolt.png") || !File.Exists("laser.png") || !File.Exists("lightningbolt.png") || !File.Exists("sludge.png")
                    || !File.Exists("waterball.png") || !File.Exists("Explosion.wav") || !File.Exists("shield.wav") || !File.Exists("slowtime.wav") || !File.Exists("speed.wav") || !File.Exists("thorns.wav")) 
                    throw new Exception("Missing Files Please Re-Download - Application Must Close");
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new IntroForm());
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Exit();
            }
        }
    }
}
