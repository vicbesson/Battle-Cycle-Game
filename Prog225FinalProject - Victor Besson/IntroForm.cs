using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Prog225FinalProject___Victor_Besson
{
    public partial class IntroForm : Form //I put comments on lines that seemed like they needed it, also some comments are have ranges and end where it has // again.
    {
        public IntroForm()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.UserPaint, true);
        }
        private void btn_Click(object sender, EventArgs e) //Sets amount of players to be spawned may have made it harder to code cycle static constructor, since I did not know this would fire it off. But I fixed any errors and it works
        {
            try
            {
                string str = ((Button)sender).Text;
                if (str.Contains("1"))
                {
                    Cycle.NumPlayers = 1;
                    Cycle.counter = 0;
                }
                if (str.Contains("2"))
                {
                    Cycle.NumPlayers = 2;
                    Cycle.counter = 1;
                }
                if (str.Contains("3"))
                {
                    Cycle.NumPlayers = 3;
                    Cycle.counter = 2;
                }
                if (str.Contains("4"))
                {
                    Cycle.NumPlayers = 4;
                    Cycle.counter = 3;
                }
                ChooseCycleForm tmpForm = new ChooseCycleForm();
                this.Hide();
                tmpForm.ShowDialog();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void lblHelp_Click(object sender, EventArgs e)
        {
            Form tempform = new HelpForm();
            tempform.ShowDialog();
        }
    }
}
