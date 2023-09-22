using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

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
            backgroundWorker1.DoWork += (obj, e) => DoWork(obj, e, renameType, sourcePath, outputPath, replace, backgroundWorker1);
            backgroundWorker1.RunWorkerAsync();
        }

        // This event handler is where the time-consuming work is done.
        private void DoWork(object sender, DoWorkEventArgs e, RenameType renameType, string sourcePath, string outputPath, bool replace, BackgroundWorker backgroundWorker1)
        {

            BackgroundWorker worker = sender as BackgroundWorker;

            RenameHelper.Rename(renameType, sourcePath, outputPath, replace, worker);
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
