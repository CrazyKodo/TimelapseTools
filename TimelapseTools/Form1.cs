using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.XObjdetect;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.CompilerServices.RuntimeHelpers;
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

        private const string _gammaCorrectionSampleFileSettingKey = "GammaCorrectionSampleFile";
        private const string _sourcePathSettingKey = "SourcePath";
        private const string _outputPathSettingKey = "OutputPath";

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

            if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings[_gammaCorrectionSampleFileSettingKey]))
            {
                try
                {
                    var path = ConfigurationManager.AppSettings[_gammaCorrectionSampleFileSettingKey];
                    if (!string.IsNullOrWhiteSpace(path))
                    {
                        string text = File.ReadAllText(path);
                        _gammaCorrectionSampleFile = path;
                        _gammaCorrectionSampleFileSize = text.Length;
                        lbGammaCorrectionSample.Text = _gammaCorrectionSampleFile;
                    }
                }
                catch (Exception)
                {
                    _gammaCorrectionSampleFile = string.Empty;
                    _gammaCorrectionSampleFileSize = 0;
                }
            }

            if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings[_sourcePathSettingKey]))
            {
                _sourcePath = ConfigurationManager.AppSettings[_sourcePathSettingKey];
                string[] files = Directory.GetFiles(_sourcePath);
                this.lableSourceFolderPath.Text = $": {_sourcePath}";
                this.labelFileCount.Text = $"File count: {files.Length.ToString()}.";
            }

            if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings[_outputPathSettingKey]))
            {
                _outputPath = ConfigurationManager.AppSettings[_outputPathSettingKey];
                this.labelOutputfolderPath.Text = $": {_outputPath}";
            }
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


                Helper.SaveAppSettings(_sourcePathSettingKey, _sourcePath);
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

                Helper.SaveAppSettings(_outputPathSettingKey, _outputPath);
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

            if (cbFileNamePrefix.SelectedItem == "ExactDateTime")
            {
                DirectoryInfo d = new DirectoryInfo(_sourcePath);
                FileInfo[] infos = d.GetFiles();
                foreach (FileInfo f in infos)
                {
                    var photoTakenDateTime = TryGetDateTimeTakenFromExif(f);
                    var fileFullName = $"{_outputPath}\\{photoTakenDateTime.ToString(_dateTimeStringPrefix)}{f.Extension}";
                    Helper.TryCopy(f.FullName, fileFullName, cbRenameReplace.Checked);
                }
            }

            if (cbFileNamePrefix.SelectedItem == "DateTimeWithFileName")
            {
                DirectoryInfo d = new DirectoryInfo(_sourcePath);
                FileInfo[] infos = d.GetFiles();
                foreach (FileInfo f in infos)
                {
                    var photoTakenDateTime = TryGetDateTimeTakenFromExif(f);
                    var fileFullName = $"{_outputPath}\\{photoTakenDateTime.ToString(_dateTimeStringPrefix)}_{f.Name}";
                    Helper.TryCopy(f.FullName, fileFullName, cbRenameReplace.Checked);
                }
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
                    Helper.TryCopy(sortedInfo[si].FullName, fileFullName, cbRenameReplace.Checked);
                }
            }

            System.Windows.Forms.MessageBox.Show("Done", "Message");
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

        private void btnGammaCorrection_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_gammaCorrectionSampleFile) || _gammaCorrectionSampleFileSize < 1)
            {
                System.Windows.Forms.MessageBox.Show("Select a sample file", "Message");
                return;
            }

            if (!int.TryParse(tbSP1y.Text, out int sp1y) || !int.TryParse(tbSP1x.Text, out int sp1x))
            {
                System.Windows.Forms.MessageBox.Show("Set at leaset one sample point", "Message");
                return;
            }

            if (!int.TryParse(tbSampleSize.Text, out int spSize))
            {
                System.Windows.Forms.MessageBox.Show("Set the sample size", "Message");
                return;
            }

            if (!int.TryParse(tbThreshold.Text, out int threshold))
            {
                System.Windows.Forms.MessageBox.Show("Set the threshold", "Message");
                return;
            }

            if (string.IsNullOrWhiteSpace(_sourcePath) || string.IsNullOrWhiteSpace(_outputPath))
            {
                System.Windows.Forms.MessageBox.Show("Select the source/output folders first", "Message");
                return;
            }

            DirectoryInfo d = new DirectoryInfo(_sourcePath);
            FileInfo[] infos = d.GetFiles();

            using (Image<Bgr, Byte> sampleImg = new Image<Bgr, Byte>(_gammaCorrectionSampleFile))
            {
                Parallel.For(0, infos.Length - 1, i =>
                {
                    using (Image<Bgr, Byte> img = new Image<Bgr, Byte>(infos[i].FullName))
                    {
                        var extension = Path.GetExtension(infos[i].FullName);
                        var fileFullName = $"{_outputPath}\\{infos[i].Name.Replace(extension, "")}_GC{extension}";
                        try
                        {
                            var sp2x = int.TryParse(tbSP2x.Text, out int re) ? (int?)re : null;
                            var sp2y = int.TryParse(tbSP2y.Text, out int re1) ? (int?)re1 : null;
                            var result = Helper.GammaCorrect(sampleImg, img, threshold, spSize, sp1x, sp1y, sp2x, sp2y);
                            result.Save(fileFullName);
                        }
                        catch (Exception ex)
                        {
                            System.Windows.Forms.MessageBox.Show(ex.Message, "Message");
                        }

                    }
                });
            }

            System.Windows.Forms.MessageBox.Show("Done", "Message");
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

                    Helper.SaveAppSettings(_gammaCorrectionSampleFileSettingKey, _gammaCorrectionSampleFile);
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
                System.Windows.Forms.MessageBox.Show("Select a sample file", "Message");
                return;
            }

            if (!int.TryParse(tbSP1y.Text, out int sp1y) || !int.TryParse(tbSP1x.Text, out int sp1x))
            {
                System.Windows.Forms.MessageBox.Show("Set at leaset one sample point", "Message");
                return;
            }

            if (!int.TryParse(tbSampleSize.Text, out int spSize))
            {
                System.Windows.Forms.MessageBox.Show("Set the sample size", "Message");
                return;
            }

            using (Image<Bgr, Byte> sampleImg = new Image<Bgr, Byte>(_gammaCorrectionSampleFile))
            {
                for (int x = sp1x; x < sp1x + spSize; x++)
                {
                    for (int y = sp1y; y < sp1y + spSize; y++)
                    {
                        var currentBGR = sampleImg[y, x];
                        var halfBGR = new Bgr(currentBGR.Blue / 3, currentBGR.Green / 3, currentBGR.Red + 255 / 2);
                        sampleImg[y, x] = halfBGR;
                    }
                }

                if (int.TryParse(tbSP2y.Text, out int sp2y) && int.TryParse(tbSP2x.Text, out int sp2x))
                {
                    for (int x = sp2x; x < sp2x + spSize; x++)
                    {
                        for (int y = sp2y; y < sp2y + spSize; y++)
                        {
                            var currentBGR = sampleImg[y, x];
                            var halfBGR = new Bgr(currentBGR.Blue + 255 / 2, currentBGR.Green / 3, currentBGR.Red / 3);
                            sampleImg[y, x] = halfBGR;
                        }
                    }
                }

                using (Form form = new Form())
                {
                    Panel panel = new Panel();
                    var lbMousePosition = new System.Windows.Forms.Label();
                    panel.Controls.Add(lbMousePosition);
                    PictureBox pb = new PictureBox();
                    panel.Controls.Add(pb);
                    form.Controls.Add(panel);

                    form.StartPosition = FormStartPosition.CenterScreen;
                    form.AutoSize = true;
                    panel.Size = new Size(1000, 1000);
                    panel.AutoScroll = true;
                    panel.Dock = DockStyle.Fill;
                    pb.MouseMove += (object mmsender, MouseEventArgs ee) =>
                    {
                        var posPB = pb.PointToClient(Cursor.Position);
                        var posPanel = panel.PointToClient(Cursor.Position);
                        lbMousePosition.Text = posPB.X + ":" + posPB.Y;
                        lbMousePosition.Top = posPanel.Y;
                        lbMousePosition.Left = posPanel.X + 10;
                    };

                    pb.SizeMode = PictureBoxSizeMode.AutoSize;
                    pb.Image = Emgu.CV.BitmapExtension.ToBitmap(sampleImg.Mat);

                    form.ShowDialog();
                }
            }
        }
    }
}
