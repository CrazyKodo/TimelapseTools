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
using Emgu.CV.XPhoto;
using System.Xml.Linq;
using System.Threading;

namespace MergePics
{
    public partial class ManualImgRegistration2DShiftForm : Form
    {

        private const string _samplePoint1XSettingKey = "ManualImgRegistration2DShiftSP1x";
        private const string _samplePoint1YSettingKey = "ManualImgRegistration2DShiftSP1y";
        private const string _samplePoint2XSettingKey = "ManualImgRegistration2DShiftSP2x";
        private const string _samplePoint2YSettingKey = "ManualImgRegistration2DShiftSP2y";
        private const string _outputFilenameTrailing = "processed";

        private int _currentImgIdx = 0;
        private int _currentfolderItems = 0;
        private FileInfo[] _loadedFileInfo = null;
        private int _samplePoint1X;
        private int _samplePoint1Y;
        private int _samplePoint2X;
        private int _samplePoint2Y;
        private Image<Bgr, Byte> _sampleImg1 = null;
        private Image<Bgr, Byte> _sampleImg2 = null;
        private bool showedWarning = false;
        private CancellationTokenSource cts = new CancellationTokenSource();
        private DateTime timerStarted { get; set; } = DateTime.UtcNow.AddYears(-1);

        private Point SamplePoint1
        {
            get
            {
                return new Point(_samplePoint1X, _samplePoint1Y);
            }
            set
            {
                _samplePoint1X = value.X;
                _samplePoint1Y = value.Y;
                Helper.SaveAppSettings(_samplePoint1XSettingKey, _samplePoint1X.ToString());
                Helper.SaveAppSettings(_samplePoint1YSettingKey, _samplePoint1Y.ToString());
            }
        }

        private Point SamplePoint2
        {
            get
            {
                return new Point(_samplePoint2X, _samplePoint2Y);
            }
            set
            {
                _samplePoint2X = value.X;
                _samplePoint2Y = value.Y;
                Helper.SaveAppSettings(_samplePoint2XSettingKey, _samplePoint2X.ToString());
                Helper.SaveAppSettings(_samplePoint2YSettingKey, _samplePoint2Y.ToString());
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
                _samplePoint1X = xPoint1;
            }
            if (int.TryParse(ConfigurationManager.AppSettings[_samplePoint1YSettingKey], out int yPoint1))
            {
                _samplePoint1Y = yPoint1;
            }
            if (int.TryParse(ConfigurationManager.AppSettings[_samplePoint2XSettingKey], out int xPoint2))
            {
                _samplePoint2X = xPoint2;
            }
            if (int.TryParse(ConfigurationManager.AppSettings[_samplePoint2YSettingKey], out int yPoint2))
            {
                _samplePoint2Y = yPoint2;
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

            _currentfolderItems = _loadedFileInfo.Length;

            if (_currentfolderItems >= 2)
            {
                _currentImgIdx++;
                LoadImages();
            }
        }

        private void pb1_MouseClick(object sender, MouseEventArgs e)
        {
            var point = pb1.PointToClient(Cursor.Position);
            var unScaledPoint = ManualRegHelper.GetActualPoint(pb1, point);
            SamplePoint1 = unScaledPoint;

            var sourceImg = new Image<Bgr, Byte>(_loadedFileInfo[fram1Idx].FullName);
            _sampleImg1 = PickSample(pb1, pb1SP1, sourceImg, unScaledPoint);
            pb1.Focus();
            RenderMergeResult();
        }

        private void pb2_Click(object sender, EventArgs e)
        {
            var point = pb2.PointToClient(Cursor.Position);
            var unScaledPoint = ManualRegHelper.GetActualPoint(pb1, point);
            SamplePoint2 = unScaledPoint;

            var sourceImg = new Image<Bgr, Byte>(_loadedFileInfo[fram2Idx].FullName);
            _sampleImg2 = PickSample(pb2, pb2SP1, sourceImg, unScaledPoint);
            pb2.Focus();
            RenderMergeResult();
        }

        private Image<Bgr, Byte> PickSample(PictureBox pictureBox, PictureBox samplePB, Image<Bgr, Byte> image, Point point)
        {
            var sampleAreaImg1 = ManualRegHelper.GetSampleAreaImg(image, 240, 200, point);
            var sampleImg = sampleAreaImg1.Clone();
            ManualRegHelper.DrawCrosshairs(sampleAreaImg1, sampleAreaImg1.Width / 2, sampleAreaImg1.Height / 2);
            ManualRegHelper.LoadPictureBox(samplePB, sampleAreaImg1, PictureBoxSizeMode.Zoom);

            ManualRegHelper.DrawSampleArea(image, 240, 200, point);
            ManualRegHelper.DrawCrosshairs(image, point);
            pictureBox.Image = Emgu.CV.BitmapExtension.ToBitmap(image.Mat);

            return sampleImg;
        }

        private Point ProcessKeyMove(int xOffset, int yOffset, Point point, string imgPath, PictureBox pictureBox, PictureBox samplePB, out Image<Bgr, Byte> result)
        {
            point.X = point.X + xOffset;
            point.Y = point.Y + yOffset;

            using (var sourceImg = new Image<Bgr, Byte>(imgPath))
            {
                result = PickSample(pictureBox, samplePB, sourceImg, point);
            }

            RenderMergeResult();
            return point;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Up:
                    if (pb1.Focused)
                    {
                        SamplePoint1 = ProcessKeyMove(0, -1, SamplePoint1, _loadedFileInfo[fram1Idx].FullName, pb1, pb1SP1, out _sampleImg1);
                        return true;
                    }
                    if (pb2.Focused)
                    {
                        SamplePoint2 = ProcessKeyMove(0, -1, SamplePoint2, _loadedFileInfo[fram2Idx].FullName, pb2, pb2SP1, out _sampleImg2);
                        return true;
                    }

                    return true;

                case Keys.Down:
                    if (pb1.Focused)
                    {
                        SamplePoint1 = ProcessKeyMove(0, 1, SamplePoint1, _loadedFileInfo[fram1Idx].FullName, pb1, pb1SP1, out _sampleImg1);
                        return true;
                    }
                    if (pb2.Focused)
                    {
                        SamplePoint2 = ProcessKeyMove(0, 1, SamplePoint2, _loadedFileInfo[fram2Idx].FullName, pb2, pb2SP1, out _sampleImg2);
                        return true;
                    }

                    return true;

                case Keys.Left:
                    if (pb1.Focused)
                    {
                        SamplePoint1 = ProcessKeyMove(-1, 0, SamplePoint1, _loadedFileInfo[fram1Idx].FullName, pb1, pb1SP1, out _sampleImg1);
                        return true;
                    }
                    if (pb2.Focused)
                    {
                        SamplePoint2 = ProcessKeyMove(-1, 0, SamplePoint2, _loadedFileInfo[fram2Idx].FullName, pb2, pb2SP1, out _sampleImg2);
                        return true;
                    }

                    return true;

                case Keys.Right:
                    if (pb1.Focused)
                    {
                        SamplePoint1 = ProcessKeyMove(1, 0, SamplePoint1, _loadedFileInfo[fram1Idx].FullName, pb1, pb1SP1, out _sampleImg1);
                        return true;
                    }
                    if (pb2.Focused)
                    {
                        SamplePoint2 = ProcessKeyMove(1, 0, SamplePoint2, _loadedFileInfo[fram2Idx].FullName, pb2, pb2SP1, out _sampleImg2);
                        return true;
                    }

                    return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        public void RenderMergeResult()
        {
            if (_sampleImg1 == null || _sampleImg2 == null)
            {
                return;
            }
            Throttle(500, _ => RenderMergeResult(_sampleImg1, _sampleImg2));
        }

        public void RenderMergeResult(Image<Bgr, Byte> image, Image<Bgr, Byte> image1)
        {
            var result = ManualRegHelper.MergeImage(image, image1);
            ManualRegHelper.LoadPictureBox(pbMerge, result, PictureBoxSizeMode.Zoom);
        }

        public void Throttle(int interval, Action<object> action, object param = null)
        {
            cts.Cancel();
            cts = new CancellationTokenSource();

            var curTime = DateTime.UtcNow;
            if (curTime.Subtract(timerStarted).TotalMilliseconds < interval)
                interval -= (int)curTime.Subtract(timerStarted).TotalMilliseconds;

            Task.Run(async delegate
            {
                await Task.Delay(interval, cts.Token);
                action.Invoke(param);
                Console.WriteLine("123");
            });

            timerStarted = curTime;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!showedWarning)
            {
                System.Windows.Forms.MessageBox.Show("This action will alter the source files directly, Please backup the folder first! Please backup the folder first! Please backup the folder first!", "Warning");
                showedWarning = true;
                return;
            }

            using (var sourceImg2 = new Image<Bgr, Byte>(_loadedFileInfo[fram2Idx].FullName))
            {
                ProcessReg(sourceImg2, SamplePoint1, SamplePoint2);
            }
            
            _currentImgIdx++;

            if (_currentImgIdx < _currentfolderItems)
            {
                LoadImages();
                pb2.Focus();
            }
        }

        private void ProcessReg(Image<Bgr, byte> inputImage, Point point, Point point1)
        {
            var offsetWidth = point.X - point1.X;
            var offsetHeight = point.Y - point1.Y;
            var absWidth = Math.Abs(offsetWidth);
            var absHeight = Math.Abs(offsetHeight);
            //The output image
            Image<Bgr, byte> image = new Image<Bgr, byte>(inputImage.Width, inputImage.Height);

            if (offsetWidth >= 0 && offsetHeight >= 0)//Shift towards the bottom right, cut off the bottom right, and leave the top left empty area as original.
            {
                inputImage.ROI = new Rectangle(0, 0, inputImage.Width - offsetWidth, inputImage.Height - offsetHeight);
                image.ROI = new Rectangle(offsetWidth, offsetHeight, image.Width - offsetWidth, image.Height - offsetHeight);
            }
            if (offsetWidth >= 0 && offsetHeight < 0)//Shift towards the top right
            {
                inputImage.ROI = new Rectangle(0, absHeight, inputImage.Width - absWidth, inputImage.Height - absHeight);
                image.ROI = new Rectangle(absWidth, 0, image.Width - absWidth, image.Height - absHeight);
            }
            if (offsetWidth < 0 && offsetHeight >= 0)//Shift towards the bottom left
            {
                inputImage.ROI = new Rectangle(absWidth, 0, inputImage.Width - absWidth, inputImage.Height - absHeight);
                image.ROI = new Rectangle(0, absHeight, image.Width - absWidth, image.Height - absHeight);
            }
            if (offsetWidth < 0 && offsetHeight < 0)//Shift towards the top left
            {
                inputImage.ROI = new Rectangle(absWidth, absHeight, inputImage.Width - absWidth, inputImage.Height - absHeight);
                image.ROI = new Rectangle(0, 0, image.Width - absWidth, image.Height - absHeight);
            }

            //Now we can past the image onto the output because the dimensions match
            inputImage.CopyTo(image);

            //Inorder to make our output seem normal, we must empty the ROI of the output image
            image.ROI = Rectangle.Empty;
            var filename = _loadedFileInfo[fram2Idx].FullName;

            //var originalName = Path.GetFileNameWithoutExtension(_loadedFileInfo[fram2Idx].FullName);
            //var path = Path.GetDirectoryName(_loadedFileInfo[fram2Idx].FullName);
            //filename = string.Format("{0}\\{1}_{2}{3}", path, originalName, _outputFilenameTrailing, _loadedFileInfo[fram2Idx].Extension);

            image.Save(filename);
        }

        private void LoadImages()
        {
            using (var sampleImg1 = new Image<Bgr, Byte>(_loadedFileInfo[fram1Idx].FullName))
            {
                ManualRegHelper.LoadPictureBox(pb1, sampleImg1, PictureBoxSizeMode.Zoom, lbP1Idx, fram1Idx, _currentfolderItems);
            }

            using (var sampleImg = new Image<Bgr, Byte>(_loadedFileInfo[fram2Idx].FullName))
            {
                ManualRegHelper.LoadPictureBox(pb2, sampleImg, PictureBoxSizeMode.Zoom, lbP2Idx, fram2Idx, _currentfolderItems);
            }

            using (var sourceImg1 = new Image<Bgr, Byte>(_loadedFileInfo[fram1Idx].FullName))
            {
                _sampleImg1 = PickSample(pb1, pb1SP1, sourceImg1, SamplePoint1);
            }

            using (var sourceImg2 = new Image<Bgr, Byte>(_loadedFileInfo[fram2Idx].FullName))
            {
                _sampleImg2 = PickSample(pb2, pb2SP1, sourceImg2, SamplePoint2);
            }
        }

        private void pb1SP1_Click(object sender, EventArgs e)
        {
            pb1.Focus();
        }

        private void pb1SP1_Paint(object sender, PaintEventArgs e)
        {
            if (pb1.Focused)
            {
                ControlPaint.DrawBorder(e.Graphics, e.ClipRectangle,
                         Color.Red, 2, ButtonBorderStyle.Solid,
                         Color.Red, 2, ButtonBorderStyle.Solid,
                         Color.Red, 2, ButtonBorderStyle.Solid,
                         Color.Red, 2, ButtonBorderStyle.Solid);
            }
        }

        private void pb2SP1_Click(object sender, EventArgs e)
        {
            pb2.Focus();
        }

        private void pb2SP1_Paint(object sender, PaintEventArgs e)
        {
            if (pb2.Focused)
            {
                ControlPaint.DrawBorder(e.Graphics, e.ClipRectangle,
                         Color.Red, 2, ButtonBorderStyle.Solid,
                         Color.Red, 2, ButtonBorderStyle.Solid,
                         Color.Red, 2, ButtonBorderStyle.Solid,
                         Color.Red, 2, ButtonBorderStyle.Solid);
            }
        }
    }
}
