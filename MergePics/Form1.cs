using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.XObjdetect;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace MergePics
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        //Mouse actions
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;
        private string _sourcePath;
        private string _outputPath;
        private string _dateTimeStringPrefix = "yyyy-MM-dd_HHmmss";
        private string _gammaCorrectionSampleFile;
        private int _gammaCorrectionSampleFileSize;

        public Form1()
        {
            InitializeComponent();
            cbFileNamePrefix.SelectedIndex = 0;
            cbMidFrameReplace.CheckState = CheckState.Checked;
        }

        public void DoMouseClick()
        {
            //Call the imported function with the cursor's current position
            uint X = (uint)Cursor.Position.X;
            uint Y = (uint)Cursor.Position.Y;
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();

            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog1.SelectedPath))
            {
                this.lableSourceFolderPath.Text = $": {folderBrowserDialog1.SelectedPath}";
                _sourcePath = folderBrowserDialog1.SelectedPath;
                string[] files = Directory.GetFiles(folderBrowserDialog1.SelectedPath);
                this.labelFileCount.Text = $"File count: {files.Length.ToString()}.";
                return;
            }

            this.lableSourceFolderPath.Text = "Select a folder first";
            _sourcePath = string.Empty;
        }

        private void btnOutputFolder_Click(object sender, EventArgs e)
        {
            DialogResult result = fBDOutput.ShowDialog();

            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fBDOutput.SelectedPath))
            {
                this.labelOutputfolderPath.Text = $": {fBDOutput.SelectedPath}";
                _outputPath = fBDOutput.SelectedPath;
                return;
            }

            this.labelOutputfolderPath.Text = "Select a folder first";
            _outputPath = string.Empty;
        }

        private void btnRename_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_sourcePath) || string.IsNullOrWhiteSpace(_outputPath))
            {
                System.Windows.Forms.MessageBox.Show("Select a folder first", "Message");
                return;
            }

            if (cbFileNamePrefix.SelectedItem == "DateTime")
            {
                DirectoryInfo d = new DirectoryInfo(_sourcePath);
                FileInfo[] infos = d.GetFiles();
                foreach (FileInfo f in infos)
                {
                    var photoTakenDateTime = TryGetDateTimeTakenFromExif(f);
                    var fileFullName = $"{_outputPath}\\{photoTakenDateTime.ToString(_dateTimeStringPrefix)}_{f.Name}";

                    System.IO.File.Copy(f.FullName, fileFullName);
                }
                System.Windows.Forms.MessageBox.Show("Done", "Message");
            }

            if (cbFileNamePrefix.SelectedItem == "IntByName")
            {
                DirectoryInfo d = new DirectoryInfo(_sourcePath);
                FileInfo[] infos = d.GetFiles();
                var sortedInfo = infos.OrderBy(x => x.Name).ToList();

                for (int si = 0; si < sortedInfo.Count; si++)
                {
                    var extension = Path.GetExtension(sortedInfo[si].FullName);
                    var fileFullName = $"{_outputPath}\\{(si + 1).ToString().PadLeft(5, '0')}{extension}";

                    System.IO.File.Copy(sortedInfo[si].FullName, fileFullName);
                }
                System.Windows.Forms.MessageBox.Show("Done", "Message");
            }

        }

        private void btnRotate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_sourcePath) || string.IsNullOrWhiteSpace(_outputPath))
            {
                System.Windows.Forms.MessageBox.Show("Select a folder first", "Message");
                return;
            }

            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            ImageCodecInfo jgpEncoder = codecs.First(x => x.FormatID == ImageFormat.Jpeg.Guid);
            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
            var myEncoderParameter = new EncoderParameter(myEncoder, 100L);
            myEncoderParameters.Param[0] = myEncoderParameter;

            DirectoryInfo d = new DirectoryInfo(_sourcePath);
            FileInfo[] infos = d.GetFiles();

            foreach (FileInfo f in infos)
            {
                var fileFullName = $"{_outputPath}\\{f.Name}";
                using (Image img = Image.FromFile(f.FullName))
                {
                    //rotate the picture by 90 degrees and re-save the picture as a Jpeg
                    img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    img.Save(fileFullName, jgpEncoder, myEncoderParameters);
                }
            }

            System.Windows.Forms.MessageBox.Show("Done", "Message");
        }


        private void btnGenerateMidFrame_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_sourcePath) || string.IsNullOrWhiteSpace(_outputPath))
            {
                System.Windows.Forms.MessageBox.Show("Select a folder first", "Message");
                return;
            }

            DirectoryInfo d = new DirectoryInfo(_sourcePath);
            FileInfo[] infos = d.GetFiles();

            Parallel.For(0, infos.Length - 1, i =>
            {
                using (var img = Image.FromFile(infos[i].FullName))
                using (var img1 = Image.FromFile(infos[i + 1].FullName))
                {
                    var extension = Path.GetExtension(infos[i].FullName);
                    var fileFullName = $"{_outputPath}\\{infos[i].Name.Replace(extension, "")}_Mid{extension}";
                    if (!cbMidFrameReplace.Checked && File.Exists(fileFullName))
                    {
                        return;
                    }
                    try
                    {
                        using (var result = new Bitmap(img.Width, img.Height))
                        {
                            var results = MergeImage((Bitmap)img, (Bitmap)img1, result);
                            if (results != null)
                            {
                                results.Save(fileFullName, System.Drawing.Imaging.ImageFormat.Jpeg);
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        System.Windows.Forms.MessageBox.Show(ex.Message, "Message");
                    }

                }
            });

            System.Windows.Forms.MessageBox.Show("Done", "Message");
        }

        private DateTime TryGetDateTimeTakenFromExif(FileInfo fileInfo)
        {
            using (Image image = Image.FromFile(fileInfo.FullName))
            {
                if (!image.PropertyIdList.Any(x => x == 36867))
                {
                    return fileInfo.CreationTime;
                }
                PropertyItem propItem = image.GetPropertyItem(36867);
                string dateTaken = Encoding.UTF8.GetString(propItem.Value);
                string sdate = Encoding.UTF8.GetString(propItem.Value).Trim();
                string secondhalf = sdate.Substring(sdate.IndexOf(" "), (sdate.Length - sdate.IndexOf(" ")));
                string firsthalf = sdate.Substring(0, 10);
                firsthalf = firsthalf.Replace(":", "-");
                sdate = firsthalf + secondhalf;
                var dtaken = DateTime.Parse(sdate);

                return dtaken;
            }
        }

        private Bitmap MergeImage(Bitmap image1, Bitmap image2, Bitmap bitmap)
        {
            bitmap.SetResolution(image1.VerticalResolution, image1.HorizontalResolution);
            for (int y = 0; y < image1.Height; y++)
            {
                for (int x = 0; x < image1.Width; x++)
                {
                    var pix1 = image1.GetPixel(x, y);
                    var pix2 = image2.GetPixel(x, y);
                    if (pix1 != pix2)
                    {
                        var midPix = MergeColor(pix1, pix2);
                        bitmap.SetPixel(x, y, midPix);
                    }
                    else
                    {
                        bitmap.SetPixel(x, y, pix1);
                    }
                }
            }

            return bitmap;
        }

        private Color MergeColor(Color color1, Color color2)
        {
            var midR = ((int)color1.R + (int)color2.R) / 2;
            var midG = ((int)color1.G + (int)color2.G) / 2;
            var midB = ((int)color1.B + (int)color2.B) / 2;

            return Color.FromArgb(midR, midG, midB);
        }

        public int MakeGrayscale(Color color)
        {
            //create the grayscale version of the pixel
            int grayScale = (int)((color.R * .3) + (color.G * .59) + (color.B * .11));

            return grayScale;
        }

        private void btnAutoClick_Click(object sender, EventArgs e)
        {
            while (true)
            {
                DoMouseClick();
                Thread.Sleep(Convert.ToInt32(tbAutoClickWait.Text));
            }
        }

        private void btnImageRegistration_Click(object sender, EventArgs e)
        {
            var img1 = CvInvoke.Imread(@"C:\CrazyKodo\VideoProject\Fig tree\Raw\Horizontal\Temp\1.jpg", Emgu.CV.CvEnum.ImreadModes.AnyColor);
            var img2 = CvInvoke.Imread(@"C:\CrazyKodo\VideoProject\Fig tree\Raw\Horizontal\Temp\2.jpg", Emgu.CV.CvEnum.ImreadModes.AnyColor);

            Mat img1_Gray = new Mat();
            Mat img2_Gray = new Mat();
            CvInvoke.CvtColor(img1, img1_Gray, ColorConversion.Bgr2Gray);
            CvInvoke.CvtColor(img2, img2_Gray, ColorConversion.Bgr2Gray);

            var orb = new ORB(5000);

            VectorOfKeyPoint k1 = new VectorOfKeyPoint();
            Mat d1 = new Mat();
            orb.DetectAndCompute(img1_Gray, null, k1, d1, false);


            VectorOfKeyPoint k2 = new VectorOfKeyPoint();
            IOutputArray d2 = new Mat();
            orb.DetectAndCompute(img2_Gray, null, k2, d2, false);

            var matcher = new BFMatcher(DistanceType.Hamming, true);

            VectorOfDMatch matches = new VectorOfDMatch();
            matcher.Match(d1, d2, matches);

            var sorted = matches.ToArray().OrderBy(x => x.Distance).Take(Convert.ToInt32(matches.Size * 0.9)).ToList();

            var p1 = new List<PointF>();
            var p2 = new List<PointF>();
            foreach (var pair in sorted)
            {
                p1.Add(k1[pair.QueryIdx].Point);
                p2.Add(k2[pair.TrainIdx].Point);
            }
            p1.Reverse();
            p2.Reverse();

            var homography = CvInvoke.FindHomography(p1.ToArray(), p2.ToArray(), Emgu.CV.CvEnum.RobustEstimationAlgorithm.Ransac);

            IOutputArray result = new Mat();
            Size size = new Size(img1.Width, img1.Height);
            CvInvoke.WarpPerspective((IInputArray)img1, result, (IInputArray)homography, size);

            CvInvoke.Imwrite(@"C:\CrazyKodo\VideoProject\Fig tree\Raw\Horizontal\Temp\output.jpg", result);

        }

        private void btnGammaCorrection_Click(object sender, EventArgs e)
        {

            using (Image<Bgr, Byte> sampleImg = new Image<Bgr, Byte>(@"C:\CrazyKodo\VideoProject\Fig tree\Raw\Horizontal\Temp\4.jpeg"))
            using (Image<Bgr, Byte> img2 = new Image<Bgr, Byte>(@"C:\CrazyKodo\VideoProject\Fig tree\Raw\Horizontal\Temp\5.jpeg"))
            {
                //var img1 = CvInvoke.Imread(@"C:\CrazyKodo\VideoProject\Fig tree\Raw\Horizontal\Temp\4.jpeg", Emgu.CV.CvEnum.ImreadModes.AnyColor);


                var point1x = !string.IsNullOrWhiteSpace(tbSP1.Text) ? Convert.ToInt32(tbSP1.Text) : 0;
                var point1y = !string.IsNullOrWhiteSpace(tbSP2.Text) ? Convert.ToInt32(tbSP2.Text) : 0;

                //Bgr bgr = new Bgr(0,0,255);
                var samples1 = new List<Bgr>();
                var samples2 = new List<Bgr>();
                for (int x = point1x; x < point1x + 50; x++)
                {
                    for (int y = point1y; y < point1y + 50; y++)
                    {
                        samples1.Add(sampleImg[x, y]);
                        samples2.Add(img2[x, y]);
                    }
                }

                var sampleImgAvgBrightness = Helper.GetAverageBrightness(samples1.ToList());
                var img2AvgBrightness = Helper.GetAverageBrightness(samples2.ToList());

                if (img2AvgBrightness < sampleImgAvgBrightness)
                {
                    img2._GammaCorrect(0.5d);
                }

                CvInvoke.Imwrite(@"C:\CrazyKodo\VideoProject\Fig tree\Raw\Horizontal\Temp\output.jpg", img2);
            }
        }

        private void btnSelectSample_Click(object sender, EventArgs e)
        {
            int size = -1;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                _gammaCorrectionSampleFile = openFileDialog1.FileName;
                try
                {
                    string text = File.ReadAllText(_gammaCorrectionSampleFile);
                    _gammaCorrectionSampleFileSize = text.Length;
                    lbGammaCorrectionSample.Text = _gammaCorrectionSampleFile;
                }
                catch (IOException)
                {
                }
            }
        }

        private void btnPreviewSampleArea_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_gammaCorrectionSampleFile) || _gammaCorrectionSampleFileSize < 1)
            {
                System.Windows.Forms.MessageBox.Show("Select a sample file first", "Message");
                return;
            }

            if (!int.TryParse(tbSP1.Text, out int sp1) || !int.TryParse(tbSP2.Text, out int sp2) || !int.TryParse(tbSampleSize.Text, out int spSize))
            {
                System.Windows.Forms.MessageBox.Show("Select a sample file first", "Message");
                return;
            }

            using (Image<Bgr, Byte> sampleImg = new Image<Bgr, Byte>(_gammaCorrectionSampleFile))
            {
                var point1x = !string.IsNullOrWhiteSpace(tbSP1.Text) ? Convert.ToInt32(tbSP1.Text) : 0;
                var point1y = !string.IsNullOrWhiteSpace(tbSP2.Text) ? Convert.ToInt32(tbSP2.Text) : 0;

                Bgr bgr = new Bgr(0, 0, 255);
                for (int x = point1x; x < point1x + 50; x++)
                {
                    for (int y = point1y; y < point1y + 50; y++)
                    {
                        sampleImg[x, y] = bgr;
                    }
                }

                using (Form form = new Form())
                {
                    Panel panel = new Panel();
                    PictureBox pb = new PictureBox();
                    panel.Controls.Add(pb);
                    form.Controls.Add(panel);

                    form.StartPosition = FormStartPosition.CenterScreen;
                    form.AutoSize = true;
                    panel.Size = new Size(1000, 1000);
                    panel.AutoScroll = true;
                    panel.Dock = DockStyle.Fill;


                    pb.SizeMode = PictureBoxSizeMode.AutoSize;
                    pb.Image = Emgu.CV.BitmapExtension.ToBitmap(sampleImg.Mat);

                    form.ShowDialog();
                }
            }
        }
    }
}
