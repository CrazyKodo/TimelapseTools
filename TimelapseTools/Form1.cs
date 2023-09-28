using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.XObjdetect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private const string _samplePoint1XSettingKey = "SP1x";
        private const string _samplePoint1YSettingKey = "SP1y";
        private const string _samplePoint2XSettingKey = "SP2x";
        private const string _samplePoint2YSettingKey = "SP2y";
        private const string _samplePoint3XSettingKey = "SP3x";
        private const string _samplePoint3YSettingKey = "SP3y";
        private const string _samplePoint4XSettingKey = "SP4x";
        private const string _samplePoint4YSettingKey = "SP4y";

        private string _sourcePath;
        private string _outputPath;
        private string _gammaCorrectionSampleFile;
        private int _gammaCorrectionSampleFileSize;

        private ProgressForm _progressForm;

        public Form1()
        {
            InitializeComponent();

            cbFileNamePrefix.SelectedIndex = 0;
            cbMidFrameReplace.CheckState = CheckState.Checked;

            var rotateOptions = Enum.GetNames(typeof(RotateFlipType));
            cbRotateOptions.DataSource = rotateOptions;
            cbRotateOptions.SelectedIndex = Array.FindIndex(rotateOptions, x => x == "Rotate90FlipNone");

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

            if (int.TryParse(ConfigurationManager.AppSettings[_samplePoint1XSettingKey], out int xPoint1) && int.TryParse(ConfigurationManager.AppSettings[_samplePoint1YSettingKey], out int yPoint1))
            {
                this.tbSP1x.Text = xPoint1.ToString();
                this.tbSP1y.Text = yPoint1.ToString();
            }

            if (int.TryParse(ConfigurationManager.AppSettings[_samplePoint2XSettingKey], out int xPoint2) && int.TryParse(ConfigurationManager.AppSettings[_samplePoint2YSettingKey], out int yPoint2))
            {
                this.tbSP2x.Text = xPoint2.ToString();
                this.tbSP2y.Text = yPoint2.ToString();
            }

            if (int.TryParse(ConfigurationManager.AppSettings[_samplePoint3XSettingKey], out int xPoint3) && int.TryParse(ConfigurationManager.AppSettings[_samplePoint3YSettingKey], out int yPoint3))
            {
                this.tbSP3x.Text = xPoint3.ToString();
                this.tbSP3y.Text = yPoint3.ToString();
            }

            if (int.TryParse(ConfigurationManager.AppSettings[_samplePoint4XSettingKey], out int xPoint4) && int.TryParse(ConfigurationManager.AppSettings[_samplePoint4YSettingKey], out int yPoint4))
            {
                this.tbSP4x.Text = xPoint4.ToString();
                this.tbSP4y.Text = yPoint4.ToString();
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
                System.Windows.Forms.MessageBox.Show("Select the source and output folders first", "Message");
                return;
            }
            RenameType renameType = RenameType.ExactDateTime;
            switch (cbFileNamePrefix.SelectedItem)
            {
                case "ExactDateTime":
                    renameType = RenameType.ExactDateTime;
                    break;
                case "DateTimeWithFileName":
                    renameType = RenameType.DateTimeWithFileName;
                    break;
                case "IntByName":
                    renameType = RenameType.IntByName;
                    break;
            }

            if (_progressForm == null)
            {
                // Start the asynchronous operation.                
                _progressForm = new ProgressForm();
                _progressForm.ProcessRename(renameType, _sourcePath, _outputPath, cbRenameReplace.Checked);
                _progressForm.StartPosition = FormStartPosition.CenterParent;
                _progressForm.ShowDialog();
            }
            _progressForm = null;
        }

        private void btnRotate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_sourcePath) || string.IsNullOrWhiteSpace(_outputPath))
            {
                System.Windows.Forms.MessageBox.Show("Select a folder first", "Message");
                return;
            }

            RotateFlipType options;
            if (!Enum.TryParse(cbRotateOptions.SelectedValue.ToString(), out options))
            {
                options = RotateFlipType.Rotate90FlipNone;
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
                    img.RotateFlip(options);
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

            if (!int.TryParse(tbSP1y.Text, out int _) || !int.TryParse(tbSP1x.Text, out int _))
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

            List<Point> points = new List<Point>();
            if (int.TryParse(tbSP1y.Text, out int sp1y) && int.TryParse(tbSP1x.Text, out int sp1x))
            {
                points.Add(new Point(sp1x, sp1y));
            }
            if (int.TryParse(tbSP2y.Text, out int sp2y) && int.TryParse(tbSP2x.Text, out int sp2x))
            {
                points.Add(new Point(sp2x, sp2y));
            }
            if (int.TryParse(tbSP3y.Text, out int sp3y) && int.TryParse(tbSP3x.Text, out int sp3x))
            {
                points.Add(new Point(sp3x, sp3y));
            }
            if (int.TryParse(tbSP4y.Text, out int sp4y) && int.TryParse(tbSP4x.Text, out int sp4x))
            {
                points.Add(new Point(sp4x, sp4y));
            }

            DirectoryInfo d = new DirectoryInfo(_sourcePath);
            FileInfo[] infos = d.GetFiles();

            using (Image<Bgr, Byte> sampleImg = new Image<Bgr, Byte>(_gammaCorrectionSampleFile))
            {
                Parallel.For(0, infos.Length, new ParallelOptions { MaxDegreeOfParallelism = 10 }, i =>
                {
                    using (Image<Bgr, Byte> img = new Image<Bgr, Byte>(infos[i].FullName))
                    {
                        var extension = Path.GetExtension(infos[i].FullName);
                        var fileFullName = $"{_outputPath}\\{infos[i].Name.Replace(extension, "")}_GC{extension}";
                        try
                        {
                            var result = Helper.GammaCorrect(sampleImg, img, threshold, spSize, points);
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

                if (int.TryParse(tbSP3y.Text, out int sp3y) && int.TryParse(tbSP3x.Text, out int sp3x))
                {
                    for (int x = sp3x; x < sp3x + spSize; x++)
                    {
                        for (int y = sp3y; y < sp3y + spSize; y++)
                        {
                            var currentBGR = sampleImg[y, x];
                            var halfBGR = new Bgr(currentBGR.Blue + 255 / 2, currentBGR.Green / 3, currentBGR.Red / 3);
                            sampleImg[y, x] = halfBGR;
                        }
                    }
                }

                if (int.TryParse(tbSP4y.Text, out int sp4y) && int.TryParse(tbSP4x.Text, out int sp4x))
                {
                    for (int x = sp4x; x < sp4x + spSize; x++)
                    {
                        for (int y = sp4y; y < sp4y + spSize; y++)
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

        private void tbSP1x_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(tbSP1x.Text,out int re))
            {
                Helper.SaveAppSettings(_samplePoint1XSettingKey, re.ToString());
            }
        }

        private void tbSP1y_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(tbSP1y.Text, out int re))
            {
                Helper.SaveAppSettings(_samplePoint1YSettingKey, re.ToString());
            }
        }

        private void tbSP2x_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(tbSP2x.Text, out int re))
            {
                Helper.SaveAppSettings(_samplePoint2XSettingKey, re.ToString());
            }
        }

        private void tbSP2y_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(tbSP2y.Text, out int re))
            {
                Helper.SaveAppSettings(_samplePoint2YSettingKey, re.ToString());
            }
        }

        private void tbSP3x_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(tbSP3x.Text, out int re))
            {
                Helper.SaveAppSettings(_samplePoint3XSettingKey, re.ToString());
            }
        }

        private void tbSP3y_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(tbSP3y.Text, out int re))
            {
                Helper.SaveAppSettings(_samplePoint3YSettingKey, re.ToString());
            }
        }

        private void tbSP4x_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(tbSP4x.Text, out int re))
            {
                Helper.SaveAppSettings(_samplePoint4XSettingKey, re.ToString());
            }
        }

        private void tbSP4y_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(tbSP4y.Text, out int re))
            {
                Helper.SaveAppSettings(_samplePoint4YSettingKey, re.ToString());
            }
        }
    }
}
