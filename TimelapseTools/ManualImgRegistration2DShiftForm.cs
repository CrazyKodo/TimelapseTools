using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MergePics
{
    public partial class ManualImgRegistration2DShiftForm : Form
    {
        public ManualImgRegistration2DShiftForm()
        {
            InitializeComponent();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, e.ClipRectangle,
                     Color.Red, 2, ButtonBorderStyle.Solid,
                     Color.Red, 2, ButtonBorderStyle.Solid,
                     Color.Red, 2, ButtonBorderStyle.Solid,
                     Color.Red, 2, ButtonBorderStyle.Solid);
        }
    }
}
