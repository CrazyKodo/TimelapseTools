using Emgu.CV.Structure;
using Emgu.CV;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MergePics
{
    public partial class ManualImgRegistration2DShiftForm : Form
    {
        private int _currentImgIdx = 0;
        public string SequenceFolderPath { get; set; }

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

        private void ManualImgRegistration2DShiftForm_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SequenceFolderPath))
            {
                return;
            }
            DirectoryInfo d = new DirectoryInfo(SequenceFolderPath);
            FileInfo[] infos = d.GetFiles();

            var totalItems = infos.Length;

            if (totalItems >= 2)
            {
                pb1.Image = Image.FromFile(infos[_currentImgIdx].FullName);
                pb1.SizeMode = PictureBoxSizeMode.Zoom;
                //GammaCorrectHelper.DrawSampleAreas(sampleImg, Convert.ToInt32(tbSampleSize.Text), points);
                //pb.Image = Emgu.CV.BitmapExtension.ToBitmap(sampleImg.Mat);
                _currentImgIdx++;
                var sampleImg = new Image<Bgr, Byte>(infos[_currentImgIdx].FullName);
                //GammaCorrectHelper.DrawSampleAreas(sampleImg, Convert.ToInt32(tbSampleSize.Text), points);
                pb2.Image = Emgu.CV.BitmapExtension.ToBitmap(sampleImg.Mat);
                //pb2.Image = Image.FromFile(infos[_currentImgIdx].FullName);
                pb2.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }
    }
}
