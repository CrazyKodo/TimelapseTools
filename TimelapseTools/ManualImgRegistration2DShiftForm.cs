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
using System.Configuration;

namespace MergePics
{
    public partial class ManualImgRegistration2DShiftForm : Form
    {

        private const string _samplePoint1XSettingKey = "ManualImgRegistration2DShiftSP1x";
        private const string _samplePoint1YSettingKey = "ManualImgRegistration2DShiftSP1y";
        private const string _samplePoint2XSettingKey = "ManualImgRegistration2DShiftSP2x";
        private const string _samplePoint2YSettingKey = "ManualImgRegistration2DShiftSP2y";

        private int _currentImgIdx = 0;
        private FileInfo[] _loadedFileInfo = null;
        private int samplePoint1X;
        private int samplePoint1Y;
        private int samplePoint2X;
        private int samplePoint2Y;

        private Point SamplePoint1
        {
            get
            {
                return new Point(samplePoint1X, samplePoint1Y);
            }
            set
            {
                samplePoint1X = value.X;
                samplePoint1Y = value.Y;
                Helper.SaveAppSettings(_samplePoint1XSettingKey, samplePoint1X.ToString());
                Helper.SaveAppSettings(_samplePoint1YSettingKey, samplePoint1Y.ToString());
            }
        }

        private Point SamplePoint2
        {
            get
            {
                return new Point(samplePoint2X, samplePoint2Y);
            }
            set
            {
                samplePoint2X = value.X;
                samplePoint2Y = value.Y;
                Helper.SaveAppSettings(_samplePoint2XSettingKey, samplePoint2X.ToString());
                Helper.SaveAppSettings(_samplePoint2YSettingKey, samplePoint2Y.ToString());
            }
        }

        private int fram1Idx { get { return _currentImgIdx - 1 > 0 ? _currentImgIdx - 1 : 0; } }
        private int fram2Idx { get { return _currentImgIdx > 0 ? _currentImgIdx : 0; } }
        public string SequenceFolderPath { get; set; }

        public ManualImgRegistration2DShiftForm()
        {
            InitializeComponent();
            this.KeyPreview = true;

            if (int.TryParse(ConfigurationManager.AppSettings[_samplePoint1XSettingKey], out int xPoint1))
            {
                samplePoint1X = xPoint1;
            }
            if (int.TryParse(ConfigurationManager.AppSettings[_samplePoint1YSettingKey], out int yPoint1))
            {
                samplePoint1Y = yPoint1;
            }
            if (int.TryParse(ConfigurationManager.AppSettings[_samplePoint2XSettingKey], out int xPoint2))
            {
                samplePoint2X = xPoint2;
            }
            if (int.TryParse(ConfigurationManager.AppSettings[_samplePoint2YSettingKey], out int yPoint2))
            {
                samplePoint2Y = yPoint2;
            }
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

                _currentImgIdx++;
                var sampleImg = new Image<Bgr, Byte>(_loadedFileInfo[_currentImgIdx].FullName);
                ManualRegHelper.LoadPictureBox(pb2, sampleImg, PictureBoxSizeMode.Zoom, lbP2Idx, _currentImgIdx, totalItems);

                var sourceImg1 = new Image<Bgr, Byte>(_loadedFileInfo[fram1Idx].FullName);
                PickSample(pb1, pb1SP1, sourceImg1, SamplePoint1);

                var sourceImg2 = new Image<Bgr, Byte>(_loadedFileInfo[fram2Idx].FullName);
                PickSample(pb2, pb2SP1, sourceImg2, SamplePoint2);
            }
        }

        private void pb1_MouseClick(object sender, MouseEventArgs e)
        {
            var point = pb1.PointToClient(Cursor.Position);
            var unScaledPoint = ManualRegHelper.GetActualPoint(pb1, point);
            SamplePoint1 = unScaledPoint;

            var sourceImg = new Image<Bgr, Byte>(_loadedFileInfo[fram1Idx].FullName);
            PickSample(pb1, pb1SP1, sourceImg, unScaledPoint);
            pb1.Focus();
        }

        private void pb2_Click(object sender, EventArgs e)
        {
            var point = pb2.PointToClient(Cursor.Position);
            var unScaledPoint = ManualRegHelper.GetActualPoint(pb1, point);
            SamplePoint2 = unScaledPoint;

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

        private Point ProcessKeyMove(int xOffset, int yOffset, Point point, string imgPath, PictureBox pictureBox, PictureBox samplePB)
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
                        SamplePoint1 = ProcessKeyMove(0, -1, SamplePoint1, _loadedFileInfo[fram1Idx].FullName, pb1, pb1SP1);
                        return true;
                    }
                    if (pb2.Focused)
                    {
                        SamplePoint2 = ProcessKeyMove(0, -1, SamplePoint2, _loadedFileInfo[fram2Idx].FullName, pb2, pb2SP1);
                        return true;
                    }

                    return true;

                case Keys.Down:
                    if (pb1.Focused)
                    {
                        SamplePoint1 = ProcessKeyMove(0, 1, SamplePoint1, _loadedFileInfo[fram1Idx].FullName, pb1, pb1SP1);
                        return true;
                    }
                    if (pb2.Focused)
                    {
                        SamplePoint2 = ProcessKeyMove(0, 1, SamplePoint2, _loadedFileInfo[fram2Idx].FullName, pb2, pb2SP1);
                        return true;
                    }

                    return true;

                case Keys.Left:
                    if (pb1.Focused)
                    {
                        SamplePoint1 = ProcessKeyMove(-1, 0, SamplePoint1, _loadedFileInfo[fram1Idx].FullName, pb1, pb1SP1);
                        return true;
                    }
                    if (pb2.Focused)
                    {
                        SamplePoint2 = ProcessKeyMove(-1, 0, SamplePoint2, _loadedFileInfo[fram2Idx].FullName, pb2, pb2SP1);
                        return true;
                    }

                    return true;

                case Keys.Right:
                    if (pb1.Focused)
                    {
                        SamplePoint1 = ProcessKeyMove(1, 0, SamplePoint1, _loadedFileInfo[fram1Idx].FullName, pb1, pb1SP1);
                        return true;
                    }
                    if (pb2.Focused)
                    {
                        SamplePoint2 = ProcessKeyMove(1, 0, SamplePoint2, _loadedFileInfo[fram2Idx].FullName, pb2, pb2SP1);
                        return true;
                    }

                    return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

    }
}
