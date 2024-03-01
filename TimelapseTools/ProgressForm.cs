using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MergePics
{
    public partial class ProgressForm : Form
    {
        public ProgressForm()
        {
            InitializeComponent();
        }

        public void ProcessRename(RenameType renameType, string sourcePath, string outputPath, bool replace)
        {
            BackgroundWorker backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.ProgressChanged += (obj, e) => backgroundWorker1_ProgressChanged(obj, e);
            backgroundWorker1.RunWorkerCompleted += (obj, e) => backgroundWorker1_RunWorkerCompleted(obj, e);
            backgroundWorker1.DoWork += (obj, e) => Rename(obj, e, renameType, sourcePath, outputPath, replace);
            backgroundWorker1.RunWorkerAsync();
        }

        public void ProcessMidFrame(string sourcePath, string outputPath, bool replace)
        {
            BackgroundWorker backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.ProgressChanged += (obj, e) => backgroundWorker1_ProgressChanged(obj, e);
            backgroundWorker1.RunWorkerCompleted += (obj, e) => backgroundWorker1_RunWorkerCompleted(obj, e);
            backgroundWorker1.DoWork += (obj, e) => CreateMidFrame(obj, e, sourcePath, outputPath, replace);
            backgroundWorker1.RunWorkerAsync();
        }

        public void ProcessGammaCorrection(string sourcePath, string outputPath, string gammaCorrectionSampleFile, bool replace, int threshold, int size, List<Point> points)
        {
            BackgroundWorker backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.ProgressChanged += (obj, e) => backgroundWorker1_ProgressChanged(obj, e);
            backgroundWorker1.RunWorkerCompleted += (obj, e) => backgroundWorker1_RunWorkerCompleted(obj, e);
            backgroundWorker1.DoWork += (obj, e) => GammaCorrection(obj, e, sourcePath, outputPath, gammaCorrectionSampleFile, replace, threshold, size, points);
            backgroundWorker1.RunWorkerAsync();
        }

        public void ProcessRotate(string sourcePath, string outputPath, RotateFlipType rotateFlipType)
        {
            BackgroundWorker backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.ProgressChanged += (obj, e) => backgroundWorker1_ProgressChanged(obj, e);
            backgroundWorker1.RunWorkerCompleted += (obj, e) => backgroundWorker1_RunWorkerCompleted(obj, e);
            backgroundWorker1.DoWork += (obj, e) => Rotate(obj, e, rotateFlipType, sourcePath, outputPath);
            backgroundWorker1.RunWorkerAsync();
        }

        private void GammaCorrection(object obj, DoWorkEventArgs e, string sourcePath, string outputPath, string gammaCorrectionSampleFile, bool replace, int threshold, int size, List<Point> points)
        {
            BackgroundWorker worker = obj as BackgroundWorker;
            GammaCorrectHelper.GammaCorrect(sourcePath, outputPath, gammaCorrectionSampleFile, replace, threshold, size, points, worker);
        }

        private void CreateMidFrame(object obj, DoWorkEventArgs e, string sourcePath, string outputPath, bool replace)
        {
            BackgroundWorker worker = obj as BackgroundWorker;
            MidFrameHelper.CreateFrame(sourcePath, outputPath, replace, worker);
        }

        private void Rename(object sender, DoWorkEventArgs e, RenameType renameType, string sourcePath, string outputPath, bool replace)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            RenameHelper.Rename(renameType, sourcePath, outputPath, replace, worker);
        }

        private void Rotate(object sender, DoWorkEventArgs e, RotateFlipType rotateFlipType, string sourcePath, string outputPath)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            RotateHelper.Rotate(rotateFlipType, sourcePath, outputPath, worker);
        }

        // This event handler updates the progress.
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            label1.Text = (e.ProgressPercentage.ToString() + "%");
            progressBar1.Value = e.ProgressPercentage;
        }

        // This event handler deals with the results of the background operation.
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                label1.Text = "Canceled!";
            }
            else if (e.Error != null)
            {
                label1.Text = "Error: " + e.Error.Message;
            }
            else
            {
                label1.Text = "Done!";
                this.Dispose();
            }
        }

        private void btnCancelProcess_Click(object sender, EventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if (worker.WorkerSupportsCancellation == true)
            {
                // Cancel the asynchronous operation.
                worker.CancelAsync();
            }
        }
    }
}
