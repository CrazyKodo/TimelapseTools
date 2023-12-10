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
using System.Reflection.Emit;

namespace MergePics
{
    public partial class ManualImgRegistration2DShiftForm : Form
    {
        private int _currentImgIdx = 0;
        private FileInfo[] _loadedFileInfo = null;
        private Point samplePoint1;
        private Point samplePoint2;

        private int fram1Idx { get { return _currentImgIdx - 1 > 0 ? _currentImgIdx - 1 : 0; } }
        private int fram2Idx { get { return _currentImgIdx > 0 ? _currentImgIdx : 0; } }
        public string SequenceFolderPath { get; set; }

        public ManualImgRegistration2DShiftForm()
        {
            InitializeComponent();
            this.KeyPreview = true;
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

            var sourceImg = new Image<Bgr, Byte>(_loadedFileInfo[fram1Idx].FullName);
            PickSample(pb1,pb1SP1, sourceImg, unScaledPoint);
            pb1.Focus();
        }

        private void pb2_Click(object sender, EventArgs e)
        {
            var point = pb2.PointToClient(Cursor.Position);
            var unScaledPoint = ManualRegHelper.GetActualPoint(pb1, point);
            samplePoint2 = unScaledPoint;

            var sourceImg = new Image<Bgr, Byte>(_loadedFileInfo[fram2Idx].FullName);
            PickSample(pb2, pb2SP1, sourceImg, unScaledPoint);
            pb2.Focus();
        }

        private void PickSample(PictureBox pictureBox, PictureBox samplePB, Image<Bgr, Byte> image, Point point)
        {
            var sampleAreaImg1 = ManualRegHelper.GetSampleAreaImg(image, 240, 200, point);
            ManualRegHelper.DrawCrosshairs(sampleAreaImg1, sampleAreaImg1.Width / 2, sampleAreaImg1.Height / 2);
            ManualRegHelper.LoadPictureBox(samplePB, sampleAreaImg1, PictureBoxSizeMode.Zoom);

            ManualRegHelper.DrawSampleArea(image, 240, 200, point);
            ManualRegHelper.DrawCrosshairs(image, point);
            pictureBox.Image = Emgu.CV.BitmapExtension.ToBitmap(image.Mat);
        }

        private Point ProcessKeyMove(int xOffset,int yOffset, Point point,string imgPath,PictureBox pictureBox, PictureBox samplePB)
        {
            point.X = point.X + xOffset;
            point.Y = point.Y + yOffset;
            
            var sourceImg = new Image<Bgr, Byte>(imgPath);
            PickSample(pictureBox, samplePB, sourceImg, point);
            return point;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Up: 
                    if (pb1.Focused)
                    {
                        samplePoint1 = ProcessKeyMove(0, -1, samplePoint1, _loadedFileInfo[fram1Idx].FullName, pb1, pb1SP1);
                        return true;
                    }
                    if (pb2.Focused)
                    {
                        samplePoint2 = ProcessKeyMove(0, -1, samplePoint2, _loadedFileInfo[fram2Idx].FullName, pb2, pb2SP1);
                        return true;
                    }

                    return true;

                case Keys.Down:
                    if (pb1.Focused)
                    {
                        samplePoint1 = ProcessKeyMove(0, 1, samplePoint1, _loadedFileInfo[fram1Idx].FullName, pb1, pb1SP1);
                        return true;
                    }
                    if (pb2.Focused)
                    {
                        samplePoint2 = ProcessKeyMove(0, 1, samplePoint2, _loadedFileInfo[fram2Idx].FullName, pb2, pb2SP1);
                        return true;
                    }

                    return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

    }
}
