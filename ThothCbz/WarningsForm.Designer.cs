namespace ThothCbz
{
    partial class WarningsForm
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
            rtbWarnings = new RichTextBox();
            ((System.ComponentModel.ISupportInitialize)pbxAnalysingTitle).BeginInit();
            SuspendLayout();
            // 
            // pbxAnalysingTitle
            // 
            pbxAnalysingTitle.Image = Properties.Resources.warnings_titles;
            pbxAnalysingTitle.Location = new System.Drawing.Point(12, 12);
            pbxAnalysingTitle.Name = "pbxAnalysingTitle";
            pbxAnalysingTitle.Size = new System.Drawing.Size(312, 35);
            pbxAnalysingTitle.SizeMode = PictureBoxSizeMode.StretchImage;
            pbxAnalysingTitle.TabIndex = 18;
            pbxAnalysingTitle.TabStop = false;
            // 
            // rtbWarnings
            // 
            rtbWarnings.BorderStyle = BorderStyle.None;
            rtbWarnings.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            rtbWarnings.Location = new System.Drawing.Point(12, 70);
            rtbWarnings.Name = "rtbWarnings";
            rtbWarnings.Size = new System.Drawing.Size(699, 232);
            rtbWarnings.TabIndex = 19;
            rtbWarnings.Text = "";
            // 
            // WarningsForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.thothBckColor;
            ClientSize = new System.Drawing.Size(721, 315);
            Controls.Add(rtbWarnings);
            Controls.Add(pbxAnalysingTitle);
            MaximizeBox = false;
            MaximumSize = new System.Drawing.Size(737, 354);
            MinimizeBox = false;
            MinimumSize = new System.Drawing.Size(737, 354);
            Name = "WarningsForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "WarningsForm";
            ((System.ComponentModel.ISupportInitialize)pbxAnalysingTitle).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pbxAnalysingTitle;
        private RichTextBox rtbWarnings;
    }
}