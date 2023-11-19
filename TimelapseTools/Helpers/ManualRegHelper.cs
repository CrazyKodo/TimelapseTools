using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MergePics
{
    internal class ManualRegHelper
    {
        public static void LoadPictureBox(PictureBox pictureBox, Image<Bgr, Byte> image, PictureBoxSizeMode pictureBoxSizeMode, Label label, int index, int totalCount)
        {
            pictureBox.Image = Emgu.CV.BitmapExtension.ToBitmap(image.Mat);
            pictureBox.SizeMode = pictureBoxSizeMode;
            label.Text = $"{(index + 1).ToString()} / {totalCount.ToString()}";
        }
    }
}
