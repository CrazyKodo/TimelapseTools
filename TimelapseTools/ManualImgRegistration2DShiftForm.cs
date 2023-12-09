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
        private FileInfo[] _loadedFileInfo = null;
        private Point samplePoint1;
        private Point samplePoint2;

        public string SequenceFolderPath { get; set; }

        public ManualImgRegistration2DShiftForm()
        {
            InitializeComponent();

        }

        private void ManualImgRegistration2DShiftForm_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SequenceFolderPath))
            {
                return;
            }
            DirectoryInfo d = new DirectoryInfo(SequenceFolderPath);
            _loadedFileInfo = d.GetFiles().OrderBy(f => f.Name).ToArray();

            var totalItems = _loadedFileInfo.Length;

            if (totalItems >= 2)
            {
                var sampleImg1 = new Image<Bgr, Byte>(_loadedFileInfo[_currentImgIdx].FullName);
                ManualRegHelper.LoadPictureBox(pb1, sampleImg1, PictureBoxSizeMode.Zoom, lbP1Idx, _currentImgIdx, totalItems);

                if (samplePoint1 != null)
                {
                    var sampleAreaImg1 = ManualRegHelper.GetSampleAreaImg(sampleImg1, 240, 200, samplePoint1);
                    ManualRegHelper.LoadPictureBox(pb1SP1, sampleImg1, PictureBoxSizeMode.Zoom);
                }

                //GammaCorrectHelper.DrawSampleAreas(sampleImg, Convert.ToInt32(tbSampleSize.Text), points);
                //pb.Image = Emgu.CV.BitmapExtension.ToBitmap(sampleImg.Mat);
                _currentImgIdx++;
                var sampleImg = new Image<Bgr, Byte>(_loadedFileInfo[_currentImgIdx].FullName);
                ////GammaCorrectHelper.DrawSampleAreas(sampleImg, Convert.ToInt32(tbSampleSize.Text), points);
                //pb2.Image = Emgu.CV.BitmapExtension.ToBitmap(sampleImg.Mat);
                ////pb2.Image = Image.FromFile(infos[_currentImgIdx].FullName);
                //pb2.SizeMode = PictureBoxSizeMode.Zoom;
                ManualRegHelper.LoadPictureBox(pb2, sampleImg, PictureBoxSizeMode.Zoom, lbP2Idx, _currentImgIdx, totalItems);
            }
        }

        private void pb1_MouseClick(object sender, MouseEventArgs e)
        {
            var point = pb1.PointToClient(Cursor.Position);

            var unScaledPoint = ManualRegHelper.GetActualPoint(pb1, point);
            samplePoint1 = unScaledPoint;

            var sourceImg = new Image<Bgr, Byte>(_loadedFileInfo[_currentImgIdx].FullName);
            var sampleAreaImg1 = ManualRegHelper.GetSampleAreaImg(sourceImg, 240, 200, samplePoint1);
            ManualRegHelper.DrawCrosshairs(sampleAreaImg1, sampleAreaImg1.Width / 2, sampleAreaImg1.Height / 2);
            ManualRegHelper.LoadPictureBox(pb1SP1, sampleAreaImg1, PictureBoxSizeMode.Zoom);

            ManualRegHelper.DrawSampleArea(sourceImg, 240, 200, unScaledPoint);
            ManualRegHelper.DrawCrosshairs(sourceImg, samplePoint1);
            pb1.Image = Emgu.CV.BitmapExtension.ToBitmap(sourceImg.Mat);
        }


    }
}
