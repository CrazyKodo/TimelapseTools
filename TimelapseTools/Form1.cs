using Emgu.CV;
using Emgu.CV.Structure;
using MergePics.Models;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

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

        private const string _sourcePathSettingKey = "SourcePath";
        private const string _outputPathSettingKey = "OutputPath";
        private const string _sequenceFolderPathSettingKey = "SequenceFolderPath";
        private const string _samplePointsListBoxItemFormat = "x:{0} y:{1}";

        private string _sourcePath;
        private string _outputPath;
        private string _gammaCorrectionSampleFile;
        private string _sequenceFolderPath;
        private int _gammaCorrectionSampleFileSize;

        private ProgressForm _progressForm;
        private ManualImgRegistration2DShiftForm _manualImgRegistrationForm;
        private GammaCorrectSettingsModel _gammaCorrectSettingsModel = new GammaCorrectSettingsModel();

        public Form1()
        {
            InitializeComponent();

            cbFileNamePrefix.SelectedIndex = 0;
            cbMidFrameReplace.CheckState = CheckState.Checked;

            var rotateOptions = Enum.GetNames(typeof(RotateFlipType));
            cbRotateOptions.DataSource = rotateOptions;
            cbRotateOptions.SelectedIndex = Array.FindIndex(rotateOptions, x => x == "Rotate90FlipNone");

            if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings[Helper.GammaCorrectionSettingsSettingKey]))
            {
                try
                {
                    var settings = ConfigurationManager.AppSettings[Helper.GammaCorrectionSettingsSettingKey];
                    _gammaCorrectSettingsModel = JsonConvert.DeserializeObject<GammaCorrectSettingsModel>(settings);
                    if (!string.IsNullOrWhiteSpace(_gammaCorrectSettingsModel.SampleFilePath))
                    {
                        string text = File.ReadAllText(_gammaCorrectSettingsModel.SampleFilePath);
                        _gammaCorrectionSampleFile = _gammaCorrectSettingsModel.SampleFilePath;
                        _gammaCorrectionSampleFileSize = text.Length;
                        lbGammaCorrectionSample.Text = _gammaCorrectionSampleFile;
                    }

                    this.tbSampleSize.Text = _gammaCorrectSettingsModel.SampleSizePX.ToString();
                    this.tbSamplePointsCount.Text = _gammaCorrectSettingsModel.SamplePointsCount.ToString();

                    _gammaCorrectSettingsModel.SamplePoints.ForEach(x =>
                      this.lbSamplePoints.Items.Add(string.Format(_samplePointsListBoxItemFormat, x.X.ToString(), x.Y.ToString()))
                      );
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
                if (Directory.Exists(_sourcePath))
                {
                    string[] files = Directory.GetFiles(_sourcePath);
                    this.lableSourceFolderPath.Text = $": {_sourcePath}";
                    this.labelFileCount.Text = $"File count: {files.Length.ToString()}.";
                }
            }

            if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings[_outputPathSettingKey]))
            {
                _outputPath = ConfigurationManager.AppSettings[_outputPathSettingKey];
                this.labelOutputfolderPath.Text = $": {_outputPath}";
            }

            if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings[_sequenceFolderPathSettingKey]))
            {
                _sequenceFolderPath = ConfigurationManager.AppSettings[_sequenceFolderPathSettingKey];
                if (Directory.Exists(_sequenceFolderPath))
                {
                    string[] files = Directory.GetFiles(_sequenceFolderPath);
                    this.lbSequenceFolderPath.Text = $": {_sequenceFolderPath}";
                    this.lbSequenceFolderFileCount.Text = $"File count: {files.Length.ToString()}.";
                }
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

            if (_progressForm == null)
            {
                // Start the asynchronous operation.                
                _progressForm = new ProgressForm();
                _progressForm.ProcessRotate(_sourcePath, _outputPath, options);
                _progressForm.StartPosition = FormStartPosition.CenterParent;
                _progressForm.ShowDialog();
            }
            _progressForm = null;
        }


        private void btnGenerateMidFrame_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_sourcePath) || string.IsNullOrWhiteSpace(_outputPath))
            {
                System.Windows.Forms.MessageBox.Show("Select a folder first", "Message");
                return;
            }

            if (_progressForm == null)
            {
                // Start the asynchronous operation.                
                _progressForm = new ProgressForm();
                _progressForm.ProcessMidFrame(_sourcePath, _outputPath, cbMidFrameReplace.Checked);
                _progressForm.StartPosition = FormStartPosition.CenterParent;
                _progressForm.ShowDialog();
            }
            _progressForm = null;
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

            if (!_gammaCorrectSettingsModel.SamplePoints.Any())
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

            if (_progressForm == null)
            {
                // Start the asynchronous operation.                
                _progressForm = new ProgressForm();
                _progressForm.ProcessGammaCorrection(_sourcePath, _outputPath, _gammaCorrectionSampleFile, cbMidFrameReplace.Checked, threshold, spSize, _gammaCorrectSettingsModel.SamplePoints);
                _progressForm.StartPosition = FormStartPosition.CenterParent;
                _progressForm.ShowDialog();
            }
            _progressForm = null;
        }

        private void btnSelectSample_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                _gammaCorrectionSampleFile = openFileDialog1.FileName;
                _gammaCorrectSettingsModel.SampleFilePath = openFileDialog1.FileName;
                try
                {
                    string text = File.ReadAllText(_gammaCorrectionSampleFile);
                    _gammaCorrectionSampleFileSize = text.Length;
                    lbGammaCorrectionSample.Text = _gammaCorrectionSampleFile;
                    Helper.SaveAppSettings(_gammaCorrectSettingsModel);
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

            if (_gammaCorrectSettingsModel.SamplePointsCount < 1)
            {
                System.Windows.Forms.MessageBox.Show("Set the number of sample point", "Message");
                return;
            }

            if (!int.TryParse(tbSampleSize.Text, out int spSize))
            {
                System.Windows.Forms.MessageBox.Show("Set the sample size", "Message");
                return;
            }

            Image<Bgr, Byte> sampleImg = new Image<Bgr, Byte>(_gammaCorrectionSampleFile);
            try
            {
                using (Form form = new Form())
                {
                    GammaCorrectHelper.DrawSampleAreas(sampleImg, Convert.ToInt32(tbSampleSize.Text), _gammaCorrectSettingsModel.SamplePoints);
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

                    var clickFlag = 0;
                    pb.MouseClick += (object mcsender, MouseEventArgs ee) =>
                    {
                        var posPB = pb.PointToClient(Cursor.Position);
                        if (_gammaCorrectSettingsModel.SamplePoints.Count() > clickFlag)
                        {
                            _gammaCorrectSettingsModel.SamplePoints.RemoveAt(clickFlag);
                        }
                        _gammaCorrectSettingsModel.SamplePoints.Insert(clickFlag, new Point(posPB.X, posPB.Y));
                        sampleImg.Dispose();
                        sampleImg = null;
                        sampleImg = new Image<Bgr, Byte>(_gammaCorrectionSampleFile);
                        GammaCorrectHelper.DrawSampleAreas(sampleImg, Convert.ToInt32(tbSampleSize.Text), _gammaCorrectSettingsModel.SamplePoints);
                        pb.Image = Emgu.CV.BitmapExtension.ToBitmap(sampleImg.Mat);

                        clickFlag = clickFlag >= _gammaCorrectSettingsModel.SamplePointsCount - 1 ? 0 : clickFlag + 1;
                        Helper.SaveAppSettings(_gammaCorrectSettingsModel);

                        this.lbSamplePoints.Items.Clear();
                        _gammaCorrectSettingsModel.SamplePoints.ForEach(x => this.lbSamplePoints.Items.Add(string.Format(_samplePointsListBoxItemFormat, x.X.ToString(), x.Y.ToString())));
                    };

                    pb.SizeMode = PictureBoxSizeMode.AutoSize;
                    pb.Image = Emgu.CV.BitmapExtension.ToBitmap(sampleImg.Mat);

                    form.ShowDialog();
                }
            }
            finally
            {
                sampleImg.Dispose();
            }
        }

        private void btnManualImgRegistrationForm_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_sequenceFolderPath))
            {
                System.Windows.Forms.MessageBox.Show("Select a folder first", "Message");
                return;
            }

            if (_manualImgRegistrationForm == null)
            {
                // Start the asynchronous operation.                
                _manualImgRegistrationForm = new ManualImgRegistration2DShiftForm();
                _manualImgRegistrationForm.SequenceFolderPath = _sequenceFolderPath;
                //_progressForm.ProcessMidFrame(_sourcePath, _outputPath, cbMidFrameReplace.Checked);
                _manualImgRegistrationForm.StartPosition = FormStartPosition.CenterParent;
                _manualImgRegistrationForm.ShowDialog();
            }
            _manualImgRegistrationForm = null;
        }

        private void btnSequenceFolder_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();

            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog1.SelectedPath))
            {
                this.lbSequenceFolderPath.Text = $": {folderBrowserDialog1.SelectedPath}";
                _sequenceFolderPath = folderBrowserDialog1.SelectedPath;
                string[] files = Directory.GetFiles(folderBrowserDialog1.SelectedPath);
                this.lbSequenceFolderFileCount.Text = $"File count: {files.Length.ToString()}.";


                Helper.SaveAppSettings(_sequenceFolderPathSettingKey, _sequenceFolderPath);
                return;
            }

            this.lbSequenceFolderPath.Text = "Select a folder first";
            _sequenceFolderPath = string.Empty;
        }

        private void tbSampleSize_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(this.tbSampleSize.Text, out int re))
            {
                _gammaCorrectSettingsModel.SampleSizePX = re;
                Helper.SaveAppSettings(_gammaCorrectSettingsModel);
            }
        }

        private void tbSamplePointsCount_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(this.tbSamplePointsCount.Text, out int re))
            {
                _gammaCorrectSettingsModel.SamplePointsCount = re;
                Helper.SaveAppSettings(_gammaCorrectSettingsModel);
            }
        }
    }
}
