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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pb1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb2SP1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb1SP1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbMerge)).BeginInit();
            this.SuspendLayout();
            // 
            // pb1
            // 
            this.pb1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pb1.Location = new System.Drawing.Point(12, 12);
            this.pb1.Name = "pb1";
            this.pb1.Size = new System.Drawing.Size(800, 600);
            this.pb1.TabIndex = 0;
            this.pb1.TabStop = false;
            this.pb1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            // 
            // pb2
            // 
            this.pb2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pb2.Location = new System.Drawing.Point(1190, 12);
            this.pb2.Name = "pb2";
            this.pb2.Size = new System.Drawing.Size(800, 600);
            this.pb2.TabIndex = 1;
            this.pb2.TabStop = false;
            // 
            // pb2SP1
            // 
            this.pb2SP1.Location = new System.Drawing.Point(944, 432);
            this.pb2SP1.Name = "pb2SP1";
            this.pb2SP1.Size = new System.Drawing.Size(240, 180);
            this.pb2SP1.TabIndex = 2;
            this.pb2SP1.TabStop = false;
            // 
            // pb1SP1
            // 
            this.pb1SP1.Location = new System.Drawing.Point(818, 12);
            this.pb1SP1.Name = "pb1SP1";
            this.pb1SP1.Size = new System.Drawing.Size(240, 180);
            this.pb1SP1.TabIndex = 2;
            this.pb1SP1.TabStop = false;
            // 
            // pbMerge
            // 
            this.pbMerge.Location = new System.Drawing.Point(878, 225);
            this.pbMerge.Name = "pbMerge";
            this.pbMerge.Size = new System.Drawing.Size(240, 180);
            this.pbMerge.TabIndex = 2;
            this.pbMerge.TabStop = false;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(605, 655);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(1290, 655);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "button1";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(942, 660);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "label1";
            // 
            // ManualImgRegistration2DShiftForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2002, 718);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pbMerge);
            this.Controls.Add(this.pb1SP1);
            this.Controls.Add(this.pb2SP1);
            this.Controls.Add(this.pb2);
            this.Controls.Add(this.pb1);
            this.Name = "ManualImgRegistration2DShiftForm";
            this.Text = "ManualImgRegistration";
            this.Load += new System.EventHandler(this.ManualImgRegistration2DShiftForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pb1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb2SP1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb1SP1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbMerge)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pb1;
        private System.Windows.Forms.PictureBox pb2;
        private System.Windows.Forms.PictureBox pb2SP1;
        private System.Windows.Forms.PictureBox pb1SP1;
        private System.Windows.Forms.PictureBox pbMerge;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
    }
}