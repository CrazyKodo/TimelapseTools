namespace MergePics
{
    partial class ManualImgRegistration2DShiftForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pb1 = new System.Windows.Forms.PictureBox();
            this.pb2 = new System.Windows.Forms.PictureBox();
            this.pb2SP1 = new System.Windows.Forms.PictureBox();
            this.pb1SP1 = new System.Windows.Forms.PictureBox();
            this.pbMerge = new System.Windows.Forms.PictureBox();
            this.lbP2Idx = new System.Windows.Forms.Label();
            this.lbP1Idx = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.pb1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb2SP1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb1SP1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbMerge)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pb1
            // 
            this.pb1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pb1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pb1.Location = new System.Drawing.Point(3, 3);
            this.pb1.Name = "pb1";
            this.pb1.Size = new System.Drawing.Size(933, 773);
            this.pb1.TabIndex = 0;
            this.pb1.TabStop = false;
            this.pb1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pb1_MouseClick);
            // 
            // pb2
            // 
            this.pb2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pb2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pb2.Location = new System.Drawing.Point(1247, 3);
            this.pb2.Name = "pb2";
            this.pb2.Size = new System.Drawing.Size(934, 773);
            this.pb2.TabIndex = 1;
            this.pb2.TabStop = false;
            this.pb2.Click += new System.EventHandler(this.pb2_Click);
            // 
            // pb2SP1
            // 
            this.pb2SP1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pb2SP1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pb2SP1.Location = new System.Drawing.Point(3, 517);
            this.pb2SP1.Name = "pb2SP1";
            this.pb2SP1.Size = new System.Drawing.Size(293, 253);
            this.pb2SP1.TabIndex = 2;
            this.pb2SP1.TabStop = false;
            this.pb2SP1.Click += new System.EventHandler(this.pb2SP1_Click);
            this.pb2SP1.Paint += new System.Windows.Forms.PaintEventHandler(this.pb2SP1_Paint);
            // 
            // pb1SP1
            // 
            this.pb1SP1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pb1SP1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pb1SP1.Location = new System.Drawing.Point(3, 3);
            this.pb1SP1.Name = "pb1SP1";
            this.pb1SP1.Size = new System.Drawing.Size(293, 251);
            this.pb1SP1.TabIndex = 2;
            this.pb1SP1.TabStop = false;
            this.pb1SP1.Click += new System.EventHandler(this.pb1SP1_Click);
            this.pb1SP1.Paint += new System.Windows.Forms.PaintEventHandler(this.pb1SP1_Paint);
            // 
            // pbMerge
            // 
            this.pbMerge.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbMerge.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbMerge.Location = new System.Drawing.Point(3, 260);
            this.pbMerge.Name = "pbMerge";
            this.pbMerge.Size = new System.Drawing.Size(293, 251);
            this.pbMerge.TabIndex = 2;
            this.pbMerge.TabStop = false;
            // 
            // lbP2Idx
            // 
            this.lbP2Idx.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbP2Idx.AutoSize = true;
            this.lbP2Idx.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbP2Idx.Location = new System.Drawing.Point(1695, 790);
            this.lbP2Idx.Name = "lbP2Idx";
            this.lbP2Idx.Size = new System.Drawing.Size(38, 15);
            this.lbP2Idx.TabIndex = 4;
            this.lbP2Idx.Text = "label1";
            // 
            // lbP1Idx
            // 
            this.lbP1Idx.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbP1Idx.AutoSize = true;
            this.lbP1Idx.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbP1Idx.Location = new System.Drawing.Point(450, 790);
            this.lbP1Idx.Name = "lbP1Idx";
            this.lbP1Idx.Size = new System.Drawing.Size(38, 15);
            this.lbP1Idx.TabIndex = 4;
            this.lbP1Idx.Text = "label1";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 43F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 43F));
            this.tableLayoutPanel1.Controls.Add(this.pb1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbP2Idx, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.pb2, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbP1Idx, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.button1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 95.2381F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4.761905F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 43F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(2184, 861);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(394, 824);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(150, 30);
            this.button1.TabIndex = 3;
            this.button1.Text = "Save and next";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.pb1SP1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.pb2SP1, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.pbMerge, 0, 1);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(942, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.34F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(299, 773);
            this.tableLayoutPanel2.TabIndex = 5;
            // 
            // ManualImgRegistration2DShiftForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2184, 861);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ManualImgRegistration2DShiftForm";
            this.Text = "ManualImgRegistration";
            this.Load += new System.EventHandler(this.ManualImgRegistration2DShiftForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pb1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb2SP1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb1SP1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbMerge)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pb1;
        private System.Windows.Forms.PictureBox pb2;
        private System.Windows.Forms.PictureBox pb2SP1;
        private System.Windows.Forms.PictureBox pb1SP1;
        private System.Windows.Forms.PictureBox pbMerge;
        private System.Windows.Forms.Label lbP2Idx;
        private System.Windows.Forms.Label lbP1Idx;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button button1;
    }
}