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
    public partial class ChooseCycleForm : Form
    {
        public ChooseCycleForm()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            for (int i = 0; i < Cycle.NumPlayers; i++)
            {  switch(i)
                {
                    case 0:
                        pn1.Visible = false;
                        break;
                    case 1:
                        pn2.Visible = false;
                        break;
                    case 2:
                        pn3.Visible = false;
                        break;
                    case 3:
                        pn4.Visible = false;
                        break;
                }
                CycleSelection.SelectionList.Add(new CycleSelection(this));
            }
        }
    }
}
