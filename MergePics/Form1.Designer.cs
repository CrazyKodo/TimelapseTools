namespace MergePics
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.btnSelectFolder = new System.Windows.Forms.Button();
            this.lableSourceFolderPath = new System.Windows.Forms.Label();
            this.labelFileCount = new System.Windows.Forms.Label();
            this.btnRename = new System.Windows.Forms.Button();
            this.cbFileNamePrefix = new System.Windows.Forms.ComboBox();
            this.btnOutputFolder = new System.Windows.Forms.Button();
            this.labelOutputfolderPath = new System.Windows.Forms.Label();
            this.fBDOutput = new System.Windows.Forms.FolderBrowserDialog();
            this.btnRotate = new System.Windows.Forms.Button();
            this.btnGenerateMidFrame = new System.Windows.Forms.Button();
            this.btnAutoClick = new System.Windows.Forms.Button();
            this.tbAutoClickWait = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbMidFrameReplace = new System.Windows.Forms.CheckBox();
            this.btnImageRegistration = new System.Windows.Forms.Button();
            this.btnGammaCorrection = new System.Windows.Forms.Button();
            this.tbSP1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbSP2 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnSelectFolder
            // 
            this.btnSelectFolder.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelectFolder.Location = new System.Drawing.Point(104, 94);
            this.btnSelectFolder.Name = "btnSelectFolder";
            this.btnSelectFolder.Size = new System.Drawing.Size(136, 23);
            this.btnSelectFolder.TabIndex = 0;
            this.btnSelectFolder.Text = "Source folder";
            this.btnSelectFolder.UseVisualStyleBackColor = true;
            this.btnSelectFolder.Click += new System.EventHandler(this.btnSelectFolder_Click);
            // 
            // lableSourceFolderPath
            // 
            this.lableSourceFolderPath.AutoSize = true;
            this.lableSourceFolderPath.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lableSourceFolderPath.Location = new System.Drawing.Point(246, 99);
            this.lableSourceFolderPath.Name = "lableSourceFolderPath";
            this.lableSourceFolderPath.Size = new System.Drawing.Size(10, 15);
            this.lableSourceFolderPath.TabIndex = 1;
            this.lableSourceFolderPath.Text = ":";
            // 
            // labelFileCount
            // 
            this.labelFileCount.AutoSize = true;
            this.labelFileCount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFileCount.Location = new System.Drawing.Point(246, 130);
            this.labelFileCount.Name = "labelFileCount";
            this.labelFileCount.Size = new System.Drawing.Size(76, 15);
            this.labelFileCount.TabIndex = 2;
            this.labelFileCount.Text = "Files count: 0";
            // 
            // btnRename
            // 
            this.btnRename.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRename.Location = new System.Drawing.Point(104, 194);
            this.btnRename.Name = "btnRename";
            this.btnRename.Size = new System.Drawing.Size(136, 23);
            this.btnRename.TabIndex = 3;
            this.btnRename.Text = "Rename files";
            this.btnRename.UseVisualStyleBackColor = true;
            this.btnRename.Click += new System.EventHandler(this.btnRename_Click);
            // 
            // cbFileNamePrefix
            // 
            this.cbFileNamePrefix.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbFileNamePrefix.FormattingEnabled = true;
            this.cbFileNamePrefix.Items.AddRange(new object[] {
            "DateTime",
            "IntByName"});
            this.cbFileNamePrefix.Location = new System.Drawing.Point(249, 191);
            this.cbFileNamePrefix.Name = "cbFileNamePrefix";
            this.cbFileNamePrefix.Size = new System.Drawing.Size(121, 23);
            this.cbFileNamePrefix.TabIndex = 5;
            // 
            // btnOutputFolder
            // 
            this.btnOutputFolder.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOutputFolder.Location = new System.Drawing.Point(104, 165);
            this.btnOutputFolder.Name = "btnOutputFolder";
            this.btnOutputFolder.Size = new System.Drawing.Size(136, 23);
            this.btnOutputFolder.TabIndex = 6;
            this.btnOutputFolder.Text = "Output folder";
            this.btnOutputFolder.UseVisualStyleBackColor = true;
            this.btnOutputFolder.Click += new System.EventHandler(this.btnOutputFolder_Click);
            // 
            // labelOutputfolderPath
            // 
            this.labelOutputfolderPath.AutoSize = true;
            this.labelOutputfolderPath.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelOutputfolderPath.Location = new System.Drawing.Point(246, 165);
            this.labelOutputfolderPath.Name = "labelOutputfolderPath";
            this.labelOutputfolderPath.Size = new System.Drawing.Size(10, 15);
            this.labelOutputfolderPath.TabIndex = 7;
            this.labelOutputfolderPath.Text = ":";
            // 
            // btnRotate
            // 
            this.btnRotate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRotate.Location = new System.Drawing.Point(104, 223);
            this.btnRotate.Name = "btnRotate";
            this.btnRotate.Size = new System.Drawing.Size(136, 23);
            this.btnRotate.TabIndex = 8;
            this.btnRotate.Text = "Rotate";
            this.btnRotate.UseVisualStyleBackColor = true;
            this.btnRotate.Click += new System.EventHandler(this.btnRotate_Click);
            // 
            // btnGenerateMidFrame
            // 
            this.btnGenerateMidFrame.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerateMidFrame.Location = new System.Drawing.Point(104, 252);
            this.btnGenerateMidFrame.Name = "btnGenerateMidFrame";
            this.btnGenerateMidFrame.Size = new System.Drawing.Size(136, 23);
            this.btnGenerateMidFrame.TabIndex = 9;
            this.btnGenerateMidFrame.Text = "GenerateMidFrame";
            this.btnGenerateMidFrame.UseVisualStyleBackColor = true;
            this.btnGenerateMidFrame.Click += new System.EventHandler(this.btnGenerateMidFrame_Click);
            // 
            // btnAutoClick
            // 
            this.btnAutoClick.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAutoClick.Location = new System.Drawing.Point(104, 282);
            this.btnAutoClick.Name = "btnAutoClick";
            this.btnAutoClick.Size = new System.Drawing.Size(136, 23);
            this.btnAutoClick.TabIndex = 10;
            this.btnAutoClick.Text = "AutoClick";
            this.btnAutoClick.UseVisualStyleBackColor = true;
            this.btnAutoClick.Click += new System.EventHandler(this.btnAutoClick_Click);
            // 
            // tbAutoClickWait
            // 
            this.tbAutoClickWait.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbAutoClickWait.Location = new System.Drawing.Point(316, 284);
            this.tbAutoClickWait.Name = "tbAutoClickWait";
            this.tbAutoClickWait.Size = new System.Drawing.Size(121, 23);
            this.tbAutoClickWait.TabIndex = 11;
            this.tbAutoClickWait.Text = "3000";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(247, 287);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 15);
            this.label1.TabIndex = 12;
            this.label1.Text = "Wait ms:";
            // 
            // cbMidFrameReplace
            // 
            this.cbMidFrameReplace.AutoSize = true;
            this.cbMidFrameReplace.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbMidFrameReplace.Location = new System.Drawing.Point(249, 258);
            this.cbMidFrameReplace.Name = "cbMidFrameReplace";
            this.cbMidFrameReplace.Size = new System.Drawing.Size(67, 19);
            this.cbMidFrameReplace.TabIndex = 13;
            this.cbMidFrameReplace.Text = "Replace";
            this.cbMidFrameReplace.UseVisualStyleBackColor = true;
            // 
            // btnImageRegistration
            // 
            this.btnImageRegistration.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImageRegistration.Location = new System.Drawing.Point(104, 311);
            this.btnImageRegistration.Name = "btnImageRegistration";
            this.btnImageRegistration.Size = new System.Drawing.Size(196, 23);
            this.btnImageRegistration.TabIndex = 14;
            this.btnImageRegistration.Text = "(Not work)Image registration";
            this.btnImageRegistration.UseVisualStyleBackColor = true;
            this.btnImageRegistration.Click += new System.EventHandler(this.btnImageRegistration_Click);
            // 
            // btnGammaCorrection
            // 
            this.btnGammaCorrection.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGammaCorrection.Location = new System.Drawing.Point(104, 341);
            this.btnGammaCorrection.Name = "btnGammaCorrection";
            this.btnGammaCorrection.Size = new System.Drawing.Size(196, 23);
            this.btnGammaCorrection.TabIndex = 15;
            this.btnGammaCorrection.Text = "Gamma correction";
            this.btnGammaCorrection.UseVisualStyleBackColor = true;
            this.btnGammaCorrection.Click += new System.EventHandler(this.btnGammaCorrection_Click);
            // 
            // tbSP1
            // 
            this.tbSP1.Location = new System.Drawing.Point(389, 344);
            this.tbSP1.Name = "tbSP1";
            this.tbSP1.Size = new System.Drawing.Size(59, 21);
            this.tbSP1.TabIndex = 16;
            this.tbSP1.Text = "1050";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(306, 346);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 15);
            this.label2.TabIndex = 17;
            this.label2.Text = "Sample point";
            // 
            // tbSP2
            // 
            this.tbSP2.Location = new System.Drawing.Point(454, 343);
            this.tbSP2.Name = "tbSP2";
            this.tbSP2.Size = new System.Drawing.Size(59, 21);
            this.tbSP2.TabIndex = 18;
            this.tbSP2.Text = "1180";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tbSP2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbSP1);
            this.Controls.Add(this.btnGammaCorrection);
            this.Controls.Add(this.btnImageRegistration);
            this.Controls.Add(this.cbMidFrameReplace);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbAutoClickWait);
            this.Controls.Add(this.btnAutoClick);
            this.Controls.Add(this.btnGenerateMidFrame);
            this.Controls.Add(this.btnRotate);
            this.Controls.Add(this.labelOutputfolderPath);
            this.Controls.Add(this.btnOutputFolder);
            this.Controls.Add(this.cbFileNamePrefix);
            this.Controls.Add(this.btnRename);
            this.Controls.Add(this.labelFileCount);
            this.Controls.Add(this.lableSourceFolderPath);
            this.Controls.Add(this.btnSelectFolder);
            this.Name = "Form1";
            this.Text = "TimeLapseTools";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button btnSelectFolder;
        private System.Windows.Forms.Label lableSourceFolderPath;
        private System.Windows.Forms.Label labelFileCount;
        private System.Windows.Forms.Button btnRename;
        private System.Windows.Forms.ComboBox cbFileNamePrefix;
        private System.Windows.Forms.Button btnOutputFolder;
        private System.Windows.Forms.Label labelOutputfolderPath;
        private System.Windows.Forms.FolderBrowserDialog fBDOutput;
        private System.Windows.Forms.Button btnRotate;
        private System.Windows.Forms.Button btnGenerateMidFrame;
        private System.Windows.Forms.Button btnAutoClick;
        private System.Windows.Forms.TextBox tbAutoClickWait;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbMidFrameReplace;
        private System.Windows.Forms.Button btnImageRegistration;
        private System.Windows.Forms.Button btnGammaCorrection;
        private System.Windows.Forms.TextBox tbSP1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbSP2;
    }
}

