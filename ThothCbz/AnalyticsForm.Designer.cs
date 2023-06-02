namespace ThothCbz
{
    partial class AnalyticsForm
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
            pbxAnalysingTitle = new PictureBox();
            progressBarAnalysis = new ProgressBar();
            txtAnalysis = new TextBox();
            backgroundWorkerAnalizer = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)pbxAnalysingTitle).BeginInit();
            SuspendLayout();
            // 
            // pbxAnalysingTitle
            // 
            pbxAnalysingTitle.Image = Properties.Resources.analysing_titles;
            pbxAnalysingTitle.Location = new System.Drawing.Point(12, 12);
            pbxAnalysingTitle.Name = "pbxAnalysingTitle";
            pbxAnalysingTitle.Size = new System.Drawing.Size(312, 35);
            pbxAnalysingTitle.SizeMode = PictureBoxSizeMode.StretchImage;
            pbxAnalysingTitle.TabIndex = 17;
            pbxAnalysingTitle.TabStop = false;
            // 
            // progressBarAnalysis
            // 
            progressBarAnalysis.Location = new System.Drawing.Point(13, 111);
            progressBarAnalysis.Name = "progressBarAnalysis";
            progressBarAnalysis.Size = new System.Drawing.Size(498, 23);
            progressBarAnalysis.TabIndex = 18;
            // 
            // txtAnalysis
            // 
            txtAnalysis.BorderStyle = BorderStyle.None;
            txtAnalysis.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txtAnalysis.ForeColor = System.Drawing.Color.White;
            txtAnalysis.Location = new System.Drawing.Point(12, 63);
            txtAnalysis.Multiline = true;
            txtAnalysis.Name = "txtAnalysis";
            txtAnalysis.ReadOnly = true;
            txtAnalysis.Size = new System.Drawing.Size(498, 42);
            txtAnalysis.TabIndex = 19;
            // 
            // backgroundWorkerAnalizer
            // 
            backgroundWorkerAnalizer.DoWork += backgroundWorkerAnalizer_DoWork;
            backgroundWorkerAnalizer.RunWorkerCompleted += backgroundWorkerAnalizer_RunWorkerCompleted;
            // 
            // AnalyticsForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.thothBckColor;
            ClientSize = new System.Drawing.Size(523, 147);
            ControlBox = false;
            Controls.Add(txtAnalysis);
            Controls.Add(progressBarAnalysis);
            Controls.Add(pbxAnalysingTitle);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            MaximizeBox = false;
            MaximumSize = new System.Drawing.Size(543, 190);
            MinimizeBox = false;
            MinimumSize = new System.Drawing.Size(543, 190);
            Name = "AnalyticsForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Analysing ...";
            Shown += AnalyticsForm_Shown;
            ((System.ComponentModel.ISupportInitialize)pbxAnalysingTitle).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pbxAnalysingTitle;
        private ProgressBar progressBarAnalysis;
        private TextBox txtAnalysis;
        private System.ComponentModel.BackgroundWorker backgroundWorkerAnalizer;
    }
}