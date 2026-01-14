namespace ThothCbz
{
    partial class frmThotCbz
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmThotCbz));
            pbxLogo = new PictureBox();
            pbxBtnAdjustFiles = new PictureBox();
            pbxBtnUnifyPages = new PictureBox();
            pbxBtnSplitPages = new PictureBox();
            pnlBottom = new Panel();
            lblVersion = new Label();
            lblUnknwonAnalytics = new Label();
            pbxUnknownStatistics = new PictureBox();
            pbxBtnCancel = new PictureBox();
            pbxBtnRefresh = new PictureBox();
            lblSplitAnalytics = new Label();
            pbxSplitStatistics = new PictureBox();
            lblUnifyAnalytics = new Label();
            pbxUnifyStatistics = new PictureBox();
            lblConversionAnalytics = new Label();
            pbxConversionStatistics = new PictureBox();
            lblSeriesAnalytics = new Label();
            pbxSeriesStatistics = new PictureBox();
            lblVolumesAnalytics = new Label();
            pbxVolumesStatistics = new PictureBox();
            lblFilesAnalytics = new Label();
            pbxFilesStatistics = new PictureBox();
            pbxAnalyticsTitle = new PictureBox();
            pbxBtnExecute = new PictureBox();
            pbxBtnGenerateCbz = new PictureBox();
            tips = new ToolTip(components);
            pnlSettings = new Panel();
            pnlHeaderSettings = new Panel();
            pbxSettingsTitle = new PictureBox();
            pbxBtnFolder = new PictureBox();
            txtDirectory = new TextBox();
            pnlSettingsControlsGroup = new Panel();
            cbxEnableBrightnessContrast = new CheckBox();
            pnlSplitter03 = new Panel();
            cbxBlankSpace = new CheckBox();
            pnlSplitter01 = new Panel();
            cbbReadOrder = new ComboBox();
            lblReadingOrder = new Label();
            nudMinimalHeight = new NumericUpDown();
            cbxUpscaleImages = new CheckBox();
            txtSplitableDirectory = new TextBox();
            lblSplitableDirectory = new Label();
            txtUnifyableDirectory = new TextBox();
            lblUnifyableDirectory = new Label();
            cbxUseSelectedDirectory = new CheckBox();
            folderDialog = new FolderBrowserDialog();
            pbxBtnHelp = new PictureBox();
            pbxBtnWarnings = new PictureBox();
            pnlTools = new Panel();
            pnlMain = new Panel();
            treeViewVolumes = new TreeView();
            pnlExecutionHeader = new Panel();
            progressBarExecution = new ProgressBar();
            pbxExecutionLogsTitle = new PictureBox();
            mainStatusStrip = new StatusStrip();
            toolStripStatusLabelMainArea = new ToolStripStatusLabel();
            backgroundWorkerExecution = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)pbxLogo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbxBtnAdjustFiles).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbxBtnUnifyPages).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbxBtnSplitPages).BeginInit();
            pnlBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbxUnknownStatistics).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbxBtnCancel).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbxBtnRefresh).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbxSplitStatistics).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbxUnifyStatistics).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbxConversionStatistics).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbxSeriesStatistics).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbxVolumesStatistics).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbxFilesStatistics).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbxAnalyticsTitle).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbxBtnExecute).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbxBtnGenerateCbz).BeginInit();
            pnlSettings.SuspendLayout();
            pnlHeaderSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbxSettingsTitle).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbxBtnFolder).BeginInit();
            pnlSettingsControlsGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudMinimalHeight).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbxBtnHelp).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbxBtnWarnings).BeginInit();
            pnlTools.SuspendLayout();
            pnlMain.SuspendLayout();
            pnlExecutionHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbxExecutionLogsTitle).BeginInit();
            mainStatusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // pbxLogo
            // 
            pbxLogo.Image = Properties.Resources.ThothCbz1;
            pbxLogo.Location = new Point(0, -1);
            pbxLogo.Margin = new Padding(2, 3, 2, 3);
            pbxLogo.Name = "pbxLogo";
            pbxLogo.Size = new Size(160, 160);
            pbxLogo.SizeMode = PictureBoxSizeMode.StretchImage;
            pbxLogo.TabIndex = 0;
            pbxLogo.TabStop = false;
            // 
            // pbxBtnAdjustFiles
            // 
            pbxBtnAdjustFiles.Cursor = Cursors.Hand;
            pbxBtnAdjustFiles.Enabled = false;
            pbxBtnAdjustFiles.Image = Properties.Resources.BtnAdjustmentsInactive512x512;
            pbxBtnAdjustFiles.InitialImage = Properties.Resources.BtnAdjustmentsInactive512x512;
            pbxBtnAdjustFiles.Location = new Point(44, 164);
            pbxBtnAdjustFiles.Margin = new Padding(2, 3, 2, 3);
            pbxBtnAdjustFiles.MaximumSize = new Size(72, 72);
            pbxBtnAdjustFiles.MinimumSize = new Size(72, 72);
            pbxBtnAdjustFiles.Name = "pbxBtnAdjustFiles";
            pbxBtnAdjustFiles.Size = new Size(72, 72);
            pbxBtnAdjustFiles.SizeMode = PictureBoxSizeMode.StretchImage;
            pbxBtnAdjustFiles.TabIndex = 6;
            pbxBtnAdjustFiles.TabStop = false;
            // 
            // pbxBtnUnifyPages
            // 
            pbxBtnUnifyPages.Cursor = Cursors.Hand;
            pbxBtnUnifyPages.Enabled = false;
            pbxBtnUnifyPages.Image = Properties.Resources.BtnUnifyInactive512x512;
            pbxBtnUnifyPages.Location = new Point(44, 240);
            pbxBtnUnifyPages.Margin = new Padding(2, 3, 2, 3);
            pbxBtnUnifyPages.Name = "pbxBtnUnifyPages";
            pbxBtnUnifyPages.Size = new Size(72, 72);
            pbxBtnUnifyPages.SizeMode = PictureBoxSizeMode.StretchImage;
            pbxBtnUnifyPages.TabIndex = 8;
            pbxBtnUnifyPages.TabStop = false;
            // 
            // pbxBtnSplitPages
            // 
            pbxBtnSplitPages.Cursor = Cursors.Hand;
            pbxBtnSplitPages.Enabled = false;
            pbxBtnSplitPages.Image = Properties.Resources.BtnSplitInactive512x512;
            pbxBtnSplitPages.Location = new Point(44, 316);
            pbxBtnSplitPages.Margin = new Padding(2, 3, 2, 3);
            pbxBtnSplitPages.MaximumSize = new Size(72, 72);
            pbxBtnSplitPages.MinimumSize = new Size(72, 72);
            pbxBtnSplitPages.Name = "pbxBtnSplitPages";
            pbxBtnSplitPages.Size = new Size(72, 72);
            pbxBtnSplitPages.SizeMode = PictureBoxSizeMode.StretchImage;
            pbxBtnSplitPages.TabIndex = 9;
            pbxBtnSplitPages.TabStop = false;
            // 
            // pnlBottom
            // 
            pnlBottom.BackgroundImage = Properties.Resources.thothBckColor;
            pnlBottom.Controls.Add(lblVersion);
            pnlBottom.Controls.Add(lblUnknwonAnalytics);
            pnlBottom.Controls.Add(pbxUnknownStatistics);
            pnlBottom.Controls.Add(pbxBtnCancel);
            pnlBottom.Controls.Add(pbxBtnRefresh);
            pnlBottom.Controls.Add(lblSplitAnalytics);
            pnlBottom.Controls.Add(pbxSplitStatistics);
            pnlBottom.Controls.Add(lblUnifyAnalytics);
            pnlBottom.Controls.Add(pbxUnifyStatistics);
            pnlBottom.Controls.Add(lblConversionAnalytics);
            pnlBottom.Controls.Add(pbxConversionStatistics);
            pnlBottom.Controls.Add(lblSeriesAnalytics);
            pnlBottom.Controls.Add(pbxSeriesStatistics);
            pnlBottom.Controls.Add(lblVolumesAnalytics);
            pnlBottom.Controls.Add(pbxVolumesStatistics);
            pnlBottom.Controls.Add(lblFilesAnalytics);
            pnlBottom.Controls.Add(pbxFilesStatistics);
            pnlBottom.Controls.Add(pbxAnalyticsTitle);
            pnlBottom.Controls.Add(pbxBtnExecute);
            pnlBottom.Dock = DockStyle.Bottom;
            pnlBottom.Location = new Point(0, 673);
            pnlBottom.Margin = new Padding(2, 3, 2, 3);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new Size(1523, 160);
            pnlBottom.TabIndex = 10;
            pnlBottom.Resize += pnlBottom_Resize;
            // 
            // lblVersion
            // 
            lblVersion.BackColor = Color.Transparent;
            lblVersion.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold | FontStyle.Italic);
            lblVersion.ForeColor = SystemColors.Window;
            lblVersion.Location = new Point(1344, 141);
            lblVersion.Margin = new Padding(2, 0, 2, 0);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new Size(172, 19);
            lblVersion.TabIndex = 33;
            lblVersion.Text = "Version 1.0.0.0";
            lblVersion.TextAlign = ContentAlignment.TopRight;
            // 
            // lblUnknwonAnalytics
            // 
            lblUnknwonAnalytics.Enabled = false;
            lblUnknwonAnalytics.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblUnknwonAnalytics.ForeColor = Color.White;
            lblUnknwonAnalytics.Location = new Point(752, 52);
            lblUnknwonAnalytics.Margin = new Padding(2, 0, 2, 0);
            lblUnknwonAnalytics.Name = "lblUnknwonAnalytics";
            lblUnknwonAnalytics.Size = new Size(160, 24);
            lblUnknwonAnalytics.TabIndex = 30;
            lblUnknwonAnalytics.Text = "99.999.999 / 99.999.999";
            // 
            // pbxUnknownStatistics
            // 
            pbxUnknownStatistics.Enabled = false;
            pbxUnknownStatistics.Image = Properties.Resources.BtnUnknownStatisticsDisable;
            pbxUnknownStatistics.InitialImage = Properties.Resources.BtnAdjustmentsInactive512x512;
            pbxUnknownStatistics.Location = new Point(700, 48);
            pbxUnknownStatistics.Margin = new Padding(2, 3, 2, 3);
            pbxUnknownStatistics.MaximumSize = new Size(40, 40);
            pbxUnknownStatistics.MinimumSize = new Size(40, 40);
            pbxUnknownStatistics.Name = "pbxUnknownStatistics";
            pbxUnknownStatistics.Size = new Size(40, 40);
            pbxUnknownStatistics.SizeMode = PictureBoxSizeMode.StretchImage;
            pbxUnknownStatistics.TabIndex = 29;
            pbxUnknownStatistics.TabStop = false;
            // 
            // pbxBtnCancel
            // 
            pbxBtnCancel.Cursor = Cursors.Hand;
            pbxBtnCancel.Enabled = false;
            pbxBtnCancel.Image = Properties.Resources.BtnCancelInactive512x512;
            pbxBtnCancel.Location = new Point(1208, 44);
            pbxBtnCancel.Margin = new Padding(2, 3, 2, 3);
            pbxBtnCancel.Name = "pbxBtnCancel";
            pbxBtnCancel.Size = new Size(72, 72);
            pbxBtnCancel.SizeMode = PictureBoxSizeMode.StretchImage;
            pbxBtnCancel.TabIndex = 15;
            pbxBtnCancel.TabStop = false;
            // 
            // pbxBtnRefresh
            // 
            pbxBtnRefresh.Cursor = Cursors.Hand;
            pbxBtnRefresh.Enabled = false;
            pbxBtnRefresh.Image = Properties.Resources.BtnRefreshInactive512x512;
            pbxBtnRefresh.Location = new Point(1296, 44);
            pbxBtnRefresh.Margin = new Padding(2, 3, 2, 3);
            pbxBtnRefresh.Name = "pbxBtnRefresh";
            pbxBtnRefresh.Size = new Size(72, 72);
            pbxBtnRefresh.SizeMode = PictureBoxSizeMode.StretchImage;
            pbxBtnRefresh.TabIndex = 28;
            pbxBtnRefresh.TabStop = false;
            // 
            // lblSplitAnalytics
            // 
            lblSplitAnalytics.Enabled = false;
            lblSplitAnalytics.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblSplitAnalytics.ForeColor = Color.White;
            lblSplitAnalytics.Location = new Point(524, 109);
            lblSplitAnalytics.Margin = new Padding(2, 0, 2, 0);
            lblSplitAnalytics.Name = "lblSplitAnalytics";
            lblSplitAnalytics.Size = new Size(160, 24);
            lblSplitAnalytics.TabIndex = 27;
            lblSplitAnalytics.Text = "99.999.999 / 99.999.999";
            // 
            // pbxSplitStatistics
            // 
            pbxSplitStatistics.Enabled = false;
            pbxSplitStatistics.Image = Properties.Resources.BtnSplitStatisticsDisable;
            pbxSplitStatistics.InitialImage = Properties.Resources.BtnAdjustmentsInactive512x512;
            pbxSplitStatistics.Location = new Point(472, 104);
            pbxSplitStatistics.Margin = new Padding(2, 3, 2, 3);
            pbxSplitStatistics.MaximumSize = new Size(40, 40);
            pbxSplitStatistics.MinimumSize = new Size(40, 40);
            pbxSplitStatistics.Name = "pbxSplitStatistics";
            pbxSplitStatistics.Size = new Size(40, 40);
            pbxSplitStatistics.SizeMode = PictureBoxSizeMode.StretchImage;
            pbxSplitStatistics.TabIndex = 26;
            pbxSplitStatistics.TabStop = false;
            // 
            // lblUnifyAnalytics
            // 
            lblUnifyAnalytics.Enabled = false;
            lblUnifyAnalytics.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblUnifyAnalytics.ForeColor = Color.White;
            lblUnifyAnalytics.Location = new Point(296, 109);
            lblUnifyAnalytics.Margin = new Padding(2, 0, 2, 0);
            lblUnifyAnalytics.Name = "lblUnifyAnalytics";
            lblUnifyAnalytics.Size = new Size(160, 24);
            lblUnifyAnalytics.TabIndex = 25;
            lblUnifyAnalytics.Text = "99.999.999 / 99.999.999";
            // 
            // pbxUnifyStatistics
            // 
            pbxUnifyStatistics.Enabled = false;
            pbxUnifyStatistics.Image = Properties.Resources.BtnUnifyStatisticsDisable;
            pbxUnifyStatistics.InitialImage = Properties.Resources.BtnAdjustmentsInactive512x512;
            pbxUnifyStatistics.Location = new Point(244, 104);
            pbxUnifyStatistics.Margin = new Padding(2, 3, 2, 3);
            pbxUnifyStatistics.MaximumSize = new Size(40, 40);
            pbxUnifyStatistics.MinimumSize = new Size(40, 40);
            pbxUnifyStatistics.Name = "pbxUnifyStatistics";
            pbxUnifyStatistics.Size = new Size(40, 40);
            pbxUnifyStatistics.SizeMode = PictureBoxSizeMode.StretchImage;
            pbxUnifyStatistics.TabIndex = 24;
            pbxUnifyStatistics.TabStop = false;
            // 
            // lblConversionAnalytics
            // 
            lblConversionAnalytics.Enabled = false;
            lblConversionAnalytics.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblConversionAnalytics.ForeColor = Color.White;
            lblConversionAnalytics.Location = new Point(68, 109);
            lblConversionAnalytics.Margin = new Padding(2, 0, 2, 0);
            lblConversionAnalytics.Name = "lblConversionAnalytics";
            lblConversionAnalytics.Size = new Size(160, 24);
            lblConversionAnalytics.TabIndex = 23;
            lblConversionAnalytics.Text = "99.999.999 / 99.999.999";
            // 
            // pbxConversionStatistics
            // 
            pbxConversionStatistics.Enabled = false;
            pbxConversionStatistics.Image = Properties.Resources.BtnTransformationDisable512x512;
            pbxConversionStatistics.InitialImage = Properties.Resources.BtnAdjustmentsInactive512x512;
            pbxConversionStatistics.Location = new Point(16, 104);
            pbxConversionStatistics.Margin = new Padding(2, 3, 2, 3);
            pbxConversionStatistics.MaximumSize = new Size(40, 40);
            pbxConversionStatistics.MinimumSize = new Size(40, 40);
            pbxConversionStatistics.Name = "pbxConversionStatistics";
            pbxConversionStatistics.Size = new Size(40, 40);
            pbxConversionStatistics.SizeMode = PictureBoxSizeMode.StretchImage;
            pbxConversionStatistics.TabIndex = 22;
            pbxConversionStatistics.TabStop = false;
            // 
            // lblSeriesAnalytics
            // 
            lblSeriesAnalytics.Enabled = false;
            lblSeriesAnalytics.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblSeriesAnalytics.ForeColor = Color.White;
            lblSeriesAnalytics.Location = new Point(524, 52);
            lblSeriesAnalytics.Margin = new Padding(2, 0, 2, 0);
            lblSeriesAnalytics.Name = "lblSeriesAnalytics";
            lblSeriesAnalytics.Size = new Size(160, 24);
            lblSeriesAnalytics.TabIndex = 21;
            lblSeriesAnalytics.Text = "99.999.999 / 99.999.999";
            // 
            // pbxSeriesStatistics
            // 
            pbxSeriesStatistics.Enabled = false;
            pbxSeriesStatistics.Image = Properties.Resources.BtnSeriesDisable512x512;
            pbxSeriesStatistics.InitialImage = Properties.Resources.BtnAdjustmentsInactive512x512;
            pbxSeriesStatistics.Location = new Point(472, 48);
            pbxSeriesStatistics.Margin = new Padding(2, 3, 2, 3);
            pbxSeriesStatistics.MaximumSize = new Size(40, 40);
            pbxSeriesStatistics.MinimumSize = new Size(40, 40);
            pbxSeriesStatistics.Name = "pbxSeriesStatistics";
            pbxSeriesStatistics.Size = new Size(40, 40);
            pbxSeriesStatistics.SizeMode = PictureBoxSizeMode.StretchImage;
            pbxSeriesStatistics.TabIndex = 20;
            pbxSeriesStatistics.TabStop = false;
            // 
            // lblVolumesAnalytics
            // 
            lblVolumesAnalytics.Enabled = false;
            lblVolumesAnalytics.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblVolumesAnalytics.ForeColor = Color.White;
            lblVolumesAnalytics.Location = new Point(296, 52);
            lblVolumesAnalytics.Margin = new Padding(2, 0, 2, 0);
            lblVolumesAnalytics.Name = "lblVolumesAnalytics";
            lblVolumesAnalytics.Size = new Size(160, 24);
            lblVolumesAnalytics.TabIndex = 19;
            lblVolumesAnalytics.Text = "99.999.999 / 99.999.999";
            // 
            // pbxVolumesStatistics
            // 
            pbxVolumesStatistics.Enabled = false;
            pbxVolumesStatistics.Image = Properties.Resources.BtnVolumesDisable512x512;
            pbxVolumesStatistics.InitialImage = Properties.Resources.BtnAdjustmentsInactive512x512;
            pbxVolumesStatistics.Location = new Point(244, 48);
            pbxVolumesStatistics.Margin = new Padding(2, 3, 2, 3);
            pbxVolumesStatistics.MaximumSize = new Size(40, 40);
            pbxVolumesStatistics.MinimumSize = new Size(40, 40);
            pbxVolumesStatistics.Name = "pbxVolumesStatistics";
            pbxVolumesStatistics.Size = new Size(40, 40);
            pbxVolumesStatistics.SizeMode = PictureBoxSizeMode.StretchImage;
            pbxVolumesStatistics.TabIndex = 18;
            pbxVolumesStatistics.TabStop = false;
            // 
            // lblFilesAnalytics
            // 
            lblFilesAnalytics.Enabled = false;
            lblFilesAnalytics.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblFilesAnalytics.ForeColor = Color.White;
            lblFilesAnalytics.Location = new Point(68, 52);
            lblFilesAnalytics.Margin = new Padding(2, 0, 2, 0);
            lblFilesAnalytics.Name = "lblFilesAnalytics";
            lblFilesAnalytics.Size = new Size(160, 24);
            lblFilesAnalytics.TabIndex = 17;
            lblFilesAnalytics.Text = "99.999.999 / 99.999.999";
            // 
            // pbxFilesStatistics
            // 
            pbxFilesStatistics.Enabled = false;
            pbxFilesStatistics.Image = Properties.Resources.BtnFileDisable512x512;
            pbxFilesStatistics.InitialImage = Properties.Resources.BtnAdjustmentsInactive512x512;
            pbxFilesStatistics.Location = new Point(16, 48);
            pbxFilesStatistics.Margin = new Padding(2, 3, 2, 3);
            pbxFilesStatistics.MaximumSize = new Size(40, 40);
            pbxFilesStatistics.MinimumSize = new Size(40, 40);
            pbxFilesStatistics.Name = "pbxFilesStatistics";
            pbxFilesStatistics.Size = new Size(40, 40);
            pbxFilesStatistics.SizeMode = PictureBoxSizeMode.StretchImage;
            pbxFilesStatistics.TabIndex = 13;
            pbxFilesStatistics.TabStop = false;
            // 
            // pbxAnalyticsTitle
            // 
            pbxAnalyticsTitle.Image = Properties.Resources.analytics_titles;
            pbxAnalyticsTitle.Location = new Point(9, 0);
            pbxAnalyticsTitle.Margin = new Padding(2, 3, 2, 3);
            pbxAnalyticsTitle.Name = "pbxAnalyticsTitle";
            pbxAnalyticsTitle.Size = new Size(286, 38);
            pbxAnalyticsTitle.SizeMode = PictureBoxSizeMode.StretchImage;
            pbxAnalyticsTitle.TabIndex = 16;
            pbxAnalyticsTitle.TabStop = false;
            // 
            // pbxBtnExecute
            // 
            pbxBtnExecute.Cursor = Cursors.Hand;
            pbxBtnExecute.Enabled = false;
            pbxBtnExecute.Image = Properties.Resources.BtnPlayInactive512x512;
            pbxBtnExecute.Location = new Point(1384, 16);
            pbxBtnExecute.Margin = new Padding(2, 3, 2, 3);
            pbxBtnExecute.Name = "pbxBtnExecute";
            pbxBtnExecute.Size = new Size(120, 120);
            pbxBtnExecute.SizeMode = PictureBoxSizeMode.StretchImage;
            pbxBtnExecute.TabIndex = 12;
            pbxBtnExecute.TabStop = false;
            // 
            // pbxBtnGenerateCbz
            // 
            pbxBtnGenerateCbz.Cursor = Cursors.Hand;
            pbxBtnGenerateCbz.Enabled = false;
            pbxBtnGenerateCbz.Image = Properties.Resources.BtnCbzInactive512x512;
            pbxBtnGenerateCbz.Location = new Point(44, 392);
            pbxBtnGenerateCbz.Margin = new Padding(2, 3, 2, 3);
            pbxBtnGenerateCbz.Name = "pbxBtnGenerateCbz";
            pbxBtnGenerateCbz.Size = new Size(72, 72);
            pbxBtnGenerateCbz.SizeMode = PictureBoxSizeMode.StretchImage;
            pbxBtnGenerateCbz.TabIndex = 11;
            pbxBtnGenerateCbz.TabStop = false;
            // 
            // pnlSettings
            // 
            pnlSettings.BackgroundImage = Properties.Resources.thothBckColor;
            pnlSettings.Controls.Add(pnlHeaderSettings);
            pnlSettings.Controls.Add(pnlSettingsControlsGroup);
            pnlSettings.Dock = DockStyle.Right;
            pnlSettings.Location = new Point(1112, 0);
            pnlSettings.Margin = new Padding(2, 3, 2, 3);
            pnlSettings.Name = "pnlSettings";
            pnlSettings.Size = new Size(411, 673);
            pnlSettings.TabIndex = 12;
            // 
            // pnlHeaderSettings
            // 
            pnlHeaderSettings.BackgroundImage = Properties.Resources.thothBckColor;
            pnlHeaderSettings.Controls.Add(pbxSettingsTitle);
            pnlHeaderSettings.Controls.Add(pbxBtnFolder);
            pnlHeaderSettings.Controls.Add(txtDirectory);
            pnlHeaderSettings.Dock = DockStyle.Top;
            pnlHeaderSettings.Location = new Point(0, 0);
            pnlHeaderSettings.Margin = new Padding(2, 3, 2, 3);
            pnlHeaderSettings.Name = "pnlHeaderSettings";
            pnlHeaderSettings.Size = new Size(411, 144);
            pnlHeaderSettings.TabIndex = 21;
            // 
            // pbxSettingsTitle
            // 
            pbxSettingsTitle.Image = Properties.Resources.settings_titles;
            pbxSettingsTitle.Location = new Point(119, 13);
            pbxSettingsTitle.Margin = new Padding(2, 3, 2, 3);
            pbxSettingsTitle.Name = "pbxSettingsTitle";
            pbxSettingsTitle.Size = new Size(286, 38);
            pbxSettingsTitle.SizeMode = PictureBoxSizeMode.StretchImage;
            pbxSettingsTitle.TabIndex = 14;
            pbxSettingsTitle.TabStop = false;
            // 
            // pbxBtnFolder
            // 
            pbxBtnFolder.Cursor = Cursors.Hand;
            pbxBtnFolder.Enabled = false;
            pbxBtnFolder.Image = Properties.Resources.BtnFolderInactive512x512;
            pbxBtnFolder.Location = new Point(15, 62);
            pbxBtnFolder.Margin = new Padding(2, 3, 2, 3);
            pbxBtnFolder.Name = "pbxBtnFolder";
            pbxBtnFolder.Size = new Size(72, 72);
            pbxBtnFolder.SizeMode = PictureBoxSizeMode.StretchImage;
            pbxBtnFolder.TabIndex = 13;
            pbxBtnFolder.TabStop = false;
            // 
            // txtDirectory
            // 
            txtDirectory.BorderStyle = BorderStyle.None;
            txtDirectory.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            txtDirectory.ForeColor = Color.White;
            txtDirectory.Location = new Point(94, 62);
            txtDirectory.Margin = new Padding(2, 3, 2, 3);
            txtDirectory.Multiline = true;
            txtDirectory.Name = "txtDirectory";
            txtDirectory.ReadOnly = true;
            txtDirectory.ShortcutsEnabled = false;
            txtDirectory.Size = new Size(306, 72);
            txtDirectory.TabIndex = 15;
            txtDirectory.TabStop = false;
            // 
            // pnlSettingsControlsGroup
            // 
            pnlSettingsControlsGroup.AutoScroll = true;
            pnlSettingsControlsGroup.BackgroundImage = Properties.Resources.thothBckColor;
            pnlSettingsControlsGroup.Controls.Add(cbxEnableBrightnessContrast);
            pnlSettingsControlsGroup.Controls.Add(pnlSplitter03);
            pnlSettingsControlsGroup.Controls.Add(cbxBlankSpace);
            pnlSettingsControlsGroup.Controls.Add(pnlSplitter01);
            pnlSettingsControlsGroup.Controls.Add(cbbReadOrder);
            pnlSettingsControlsGroup.Controls.Add(lblReadingOrder);
            pnlSettingsControlsGroup.Controls.Add(nudMinimalHeight);
            pnlSettingsControlsGroup.Controls.Add(cbxUpscaleImages);
            pnlSettingsControlsGroup.Controls.Add(txtSplitableDirectory);
            pnlSettingsControlsGroup.Controls.Add(lblSplitableDirectory);
            pnlSettingsControlsGroup.Controls.Add(txtUnifyableDirectory);
            pnlSettingsControlsGroup.Controls.Add(lblUnifyableDirectory);
            pnlSettingsControlsGroup.Controls.Add(cbxUseSelectedDirectory);
            pnlSettingsControlsGroup.Location = new Point(0, 144);
            pnlSettingsControlsGroup.Margin = new Padding(2, 3, 2, 3);
            pnlSettingsControlsGroup.Name = "pnlSettingsControlsGroup";
            pnlSettingsControlsGroup.Size = new Size(411, 522);
            pnlSettingsControlsGroup.TabIndex = 20;
            // 
            // cbxEnableBrightnessContrast
            // 
            cbxEnableBrightnessContrast.BackgroundImage = Properties.Resources.thothBckColor;
            cbxEnableBrightnessContrast.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            cbxEnableBrightnessContrast.ForeColor = SystemColors.Window;
            cbxEnableBrightnessContrast.Location = new Point(9, 398);
            cbxEnableBrightnessContrast.Margin = new Padding(2, 3, 2, 3);
            cbxEnableBrightnessContrast.Name = "cbxEnableBrightnessContrast";
            cbxEnableBrightnessContrast.Size = new Size(385, 53);
            cbxEnableBrightnessContrast.TabIndex = 35;
            cbxEnableBrightnessContrast.Text = "Enable image beautifier (edit file_to_grayscale.txt)";
            cbxEnableBrightnessContrast.UseVisualStyleBackColor = true;
            // 
            // pnlSplitter03
            // 
            pnlSplitter03.BackgroundImage = Properties.Resources.splitter;
            pnlSplitter03.Location = new Point(5, 450);
            pnlSplitter03.Margin = new Padding(2, 3, 2, 3);
            pnlSplitter03.Name = "pnlSplitter03";
            pnlSplitter03.Size = new Size(398, 15);
            pnlSplitter03.TabIndex = 34;
            // 
            // cbxBlankSpace
            // 
            cbxBlankSpace.BackgroundImage = Properties.Resources.thothBckColor;
            cbxBlankSpace.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            cbxBlankSpace.ForeColor = SystemColors.Window;
            cbxBlankSpace.Location = new Point(12, 458);
            cbxBlankSpace.Margin = new Padding(2, 3, 2, 3);
            cbxBlankSpace.Name = "cbxBlankSpace";
            cbxBlankSpace.Size = new Size(385, 58);
            cbxBlankSpace.TabIndex = 33;
            cbxBlankSpace.Text = "Add blank space between unified pages if the pages have diferente widths.";
            cbxBlankSpace.UseVisualStyleBackColor = true;
            // 
            // pnlSplitter01
            // 
            pnlSplitter01.BackgroundImage = Properties.Resources.splitter;
            pnlSplitter01.Location = new Point(6, 222);
            pnlSplitter01.Margin = new Padding(2, 3, 2, 3);
            pnlSplitter01.Name = "pnlSplitter01";
            pnlSplitter01.Size = new Size(398, 15);
            pnlSplitter01.TabIndex = 31;
            // 
            // cbbReadOrder
            // 
            cbbReadOrder.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbReadOrder.FlatStyle = FlatStyle.Flat;
            cbbReadOrder.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold | FontStyle.Italic);
            cbbReadOrder.FormattingEnabled = true;
            cbbReadOrder.Location = new Point(34, 363);
            cbbReadOrder.Margin = new Padding(2, 3, 2, 3);
            cbbReadOrder.Name = "cbbReadOrder";
            cbbReadOrder.Size = new Size(347, 28);
            cbbReadOrder.TabIndex = 29;
            // 
            // lblReadingOrder
            // 
            lblReadingOrder.BackColor = Color.Transparent;
            lblReadingOrder.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            lblReadingOrder.ForeColor = SystemColors.Window;
            lblReadingOrder.Location = new Point(28, 335);
            lblReadingOrder.Margin = new Padding(2, 0, 2, 0);
            lblReadingOrder.Name = "lblReadingOrder";
            lblReadingOrder.Size = new Size(376, 34);
            lblReadingOrder.TabIndex = 28;
            lblReadingOrder.Text = "The order of reading for all the series found are:";
            // 
            // nudMinimalHeight
            // 
            nudMinimalHeight.BorderStyle = BorderStyle.FixedSingle;
            nudMinimalHeight.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold | FontStyle.Italic);
            nudMinimalHeight.Location = new Point(34, 293);
            nudMinimalHeight.Margin = new Padding(2, 3, 2, 3);
            nudMinimalHeight.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            nudMinimalHeight.Name = "nudMinimalHeight";
            nudMinimalHeight.Size = new Size(346, 27);
            nudMinimalHeight.TabIndex = 27;
            nudMinimalHeight.TextAlign = HorizontalAlignment.Center;
            // 
            // cbxUpscaleImages
            // 
            cbxUpscaleImages.BackgroundImage = Properties.Resources.thothBckColor;
            cbxUpscaleImages.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            cbxUpscaleImages.ForeColor = SystemColors.Window;
            cbxUpscaleImages.Location = new Point(12, 238);
            cbxUpscaleImages.Margin = new Padding(2, 3, 2, 3);
            cbxUpscaleImages.Name = "cbxUpscaleImages";
            cbxUpscaleImages.Size = new Size(385, 58);
            cbxUpscaleImages.TabIndex = 26;
            cbxUpscaleImages.Text = "Upscale all the images for the minimal height of (in pixels):";
            cbxUpscaleImages.UseVisualStyleBackColor = true;
            // 
            // txtSplitableDirectory
            // 
            txtSplitableDirectory.BorderStyle = BorderStyle.FixedSingle;
            txtSplitableDirectory.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold | FontStyle.Italic);
            txtSplitableDirectory.Location = new Point(34, 183);
            txtSplitableDirectory.Margin = new Padding(2, 3, 2, 3);
            txtSplitableDirectory.Name = "txtSplitableDirectory";
            txtSplitableDirectory.Size = new Size(347, 27);
            txtSplitableDirectory.TabIndex = 25;
            txtSplitableDirectory.TextAlign = HorizontalAlignment.Center;
            // 
            // lblSplitableDirectory
            // 
            lblSplitableDirectory.BackColor = Color.Transparent;
            lblSplitableDirectory.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            lblSplitableDirectory.ForeColor = SystemColors.Window;
            lblSplitableDirectory.Location = new Point(15, 141);
            lblSplitableDirectory.Margin = new Padding(2, 0, 2, 0);
            lblSplitableDirectory.Name = "lblSplitableDirectory";
            lblSplitableDirectory.Size = new Size(385, 49);
            lblSplitableDirectory.TabIndex = 24;
            lblSplitableDirectory.Text = "All pages that needs to be splitted are located in the follow directory:";
            // 
            // txtUnifyableDirectory
            // 
            txtUnifyableDirectory.BorderStyle = BorderStyle.FixedSingle;
            txtUnifyableDirectory.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold | FontStyle.Italic);
            txtUnifyableDirectory.Location = new Point(34, 98);
            txtUnifyableDirectory.Margin = new Padding(2, 3, 2, 3);
            txtUnifyableDirectory.Name = "txtUnifyableDirectory";
            txtUnifyableDirectory.Size = new Size(347, 27);
            txtUnifyableDirectory.TabIndex = 23;
            txtUnifyableDirectory.TextAlign = HorizontalAlignment.Center;
            // 
            // lblUnifyableDirectory
            // 
            lblUnifyableDirectory.BackColor = Color.Transparent;
            lblUnifyableDirectory.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            lblUnifyableDirectory.ForeColor = SystemColors.Window;
            lblUnifyableDirectory.Location = new Point(12, 56);
            lblUnifyableDirectory.Margin = new Padding(2, 0, 2, 0);
            lblUnifyableDirectory.Name = "lblUnifyableDirectory";
            lblUnifyableDirectory.Size = new Size(389, 49);
            lblUnifyableDirectory.TabIndex = 22;
            lblUnifyableDirectory.Text = "All pages that needs to be unified are located in the follow directory:";
            // 
            // cbxUseSelectedDirectory
            // 
            cbxUseSelectedDirectory.BackgroundImage = Properties.Resources.thothBckColor;
            cbxUseSelectedDirectory.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            cbxUseSelectedDirectory.ForeColor = SystemColors.Window;
            cbxUseSelectedDirectory.Location = new Point(15, 7);
            cbxUseSelectedDirectory.Margin = new Padding(2, 3, 2, 3);
            cbxUseSelectedDirectory.Name = "cbxUseSelectedDirectory";
            cbxUseSelectedDirectory.Size = new Size(385, 40);
            cbxUseSelectedDirectory.TabIndex = 21;
            cbxUseSelectedDirectory.Text = "The selected directory is part of the final file structure!";
            cbxUseSelectedDirectory.UseVisualStyleBackColor = true;
            // 
            // pbxBtnHelp
            // 
            pbxBtnHelp.Cursor = Cursors.Hand;
            pbxBtnHelp.Image = Properties.Resources.BtnHelpInactive512x512;
            pbxBtnHelp.Location = new Point(44, 468);
            pbxBtnHelp.Margin = new Padding(2, 3, 2, 3);
            pbxBtnHelp.Name = "pbxBtnHelp";
            pbxBtnHelp.Size = new Size(72, 72);
            pbxBtnHelp.SizeMode = PictureBoxSizeMode.StretchImage;
            pbxBtnHelp.TabIndex = 13;
            pbxBtnHelp.TabStop = false;
            // 
            // pbxBtnWarnings
            // 
            pbxBtnWarnings.Cursor = Cursors.Hand;
            pbxBtnWarnings.Enabled = false;
            pbxBtnWarnings.Image = Properties.Resources.BtnWarningsInactive512x512;
            pbxBtnWarnings.Location = new Point(44, 544);
            pbxBtnWarnings.Margin = new Padding(2, 3, 2, 3);
            pbxBtnWarnings.Name = "pbxBtnWarnings";
            pbxBtnWarnings.Size = new Size(72, 72);
            pbxBtnWarnings.SizeMode = PictureBoxSizeMode.StretchImage;
            pbxBtnWarnings.TabIndex = 14;
            pbxBtnWarnings.TabStop = false;
            // 
            // pnlTools
            // 
            pnlTools.BackgroundImage = Properties.Resources.thothBckColor;
            pnlTools.Controls.Add(pbxLogo);
            pnlTools.Controls.Add(pbxBtnWarnings);
            pnlTools.Controls.Add(pbxBtnAdjustFiles);
            pnlTools.Controls.Add(pbxBtnHelp);
            pnlTools.Controls.Add(pbxBtnUnifyPages);
            pnlTools.Controls.Add(pbxBtnSplitPages);
            pnlTools.Controls.Add(pbxBtnGenerateCbz);
            pnlTools.Dock = DockStyle.Left;
            pnlTools.Location = new Point(0, 0);
            pnlTools.Margin = new Padding(2, 3, 2, 3);
            pnlTools.Name = "pnlTools";
            pnlTools.Size = new Size(160, 673);
            pnlTools.TabIndex = 15;
            // 
            // pnlMain
            // 
            pnlMain.BackgroundImage = Properties.Resources.thothBckColor;
            pnlMain.Controls.Add(treeViewVolumes);
            pnlMain.Controls.Add(pnlExecutionHeader);
            pnlMain.Controls.Add(mainStatusStrip);
            pnlMain.Dock = DockStyle.Fill;
            pnlMain.Location = new Point(160, 0);
            pnlMain.Margin = new Padding(2, 3, 2, 3);
            pnlMain.Name = "pnlMain";
            pnlMain.Size = new Size(952, 673);
            pnlMain.TabIndex = 16;
            // 
            // treeViewVolumes
            // 
            treeViewVolumes.Location = new Point(0, 62);
            treeViewVolumes.Name = "treeViewVolumes";
            treeViewVolumes.Size = new Size(952, 585);
            treeViewVolumes.TabIndex = 21;
            // 
            // pnlExecutionHeader
            // 
            pnlExecutionHeader.BackgroundImage = Properties.Resources.thothBckColor;
            pnlExecutionHeader.Controls.Add(progressBarExecution);
            pnlExecutionHeader.Controls.Add(pbxExecutionLogsTitle);
            pnlExecutionHeader.Dock = DockStyle.Top;
            pnlExecutionHeader.Location = new Point(0, 0);
            pnlExecutionHeader.Margin = new Padding(2, 3, 2, 3);
            pnlExecutionHeader.Name = "pnlExecutionHeader";
            pnlExecutionHeader.Size = new Size(952, 73);
            pnlExecutionHeader.TabIndex = 16;
            pnlExecutionHeader.Resize += pnlSeries_Resize;
            // 
            // progressBarExecution
            // 
            progressBarExecution.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            progressBarExecution.Location = new Point(678, 19);
            progressBarExecution.Margin = new Padding(2, 3, 2, 3);
            progressBarExecution.Name = "progressBarExecution";
            progressBarExecution.Size = new Size(264, 25);
            progressBarExecution.TabIndex = 19;
            // 
            // pbxExecutionLogsTitle
            // 
            pbxExecutionLogsTitle.Image = Properties.Resources.execution_logs_titles;
            pbxExecutionLogsTitle.Location = new Point(15, 13);
            pbxExecutionLogsTitle.Margin = new Padding(2, 3, 2, 3);
            pbxExecutionLogsTitle.Name = "pbxExecutionLogsTitle";
            pbxExecutionLogsTitle.Size = new Size(286, 38);
            pbxExecutionLogsTitle.SizeMode = PictureBoxSizeMode.StretchImage;
            pbxExecutionLogsTitle.TabIndex = 15;
            pbxExecutionLogsTitle.TabStop = false;
            // 
            // mainStatusStrip
            // 
            mainStatusStrip.BackgroundImage = Properties.Resources.thothBckColor;
            mainStatusStrip.ImageScalingSize = new Size(20, 20);
            mainStatusStrip.Items.AddRange(new ToolStripItem[] { toolStripStatusLabelMainArea });
            mainStatusStrip.Location = new Point(0, 647);
            mainStatusStrip.Name = "mainStatusStrip";
            mainStatusStrip.Padding = new Padding(1, 0, 13, 0);
            mainStatusStrip.Size = new Size(952, 26);
            mainStatusStrip.TabIndex = 0;
            mainStatusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabelMainArea
            // 
            toolStripStatusLabelMainArea.AutoSize = false;
            toolStripStatusLabelMainArea.Margin = new Padding(10, 0, 0, 0);
            toolStripStatusLabelMainArea.Name = "toolStripStatusLabelMainArea";
            toolStripStatusLabelMainArea.Size = new Size(400, 26);
            toolStripStatusLabelMainArea.Text = "toolStripStatusLabel";
            // 
            // backgroundWorkerExecution
            // 
            backgroundWorkerExecution.DoWork += backgroundWorkerExecution_DoWork;
            backgroundWorkerExecution.RunWorkerCompleted += backgroundWorkerExecution_RunWorkerCompleted;
            // 
            // frmThotCbz
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.White;
            BackgroundImage = Properties.Resources.thothBckColor;
            ClientSize = new Size(1523, 833);
            Controls.Add(pnlMain);
            Controls.Add(pnlTools);
            Controls.Add(pnlSettings);
            Controls.Add(pnlBottom);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(2, 3, 2, 3);
            MinimumSize = new Size(1539, 872);
            Name = "frmThotCbz";
            Text = "THOTH CBZ GENERATOR";
            FormClosing += frmThotCbz_FormClosing;
            ((System.ComponentModel.ISupportInitialize)pbxLogo).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbxBtnAdjustFiles).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbxBtnUnifyPages).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbxBtnSplitPages).EndInit();
            pnlBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pbxUnknownStatistics).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbxBtnCancel).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbxBtnRefresh).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbxSplitStatistics).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbxUnifyStatistics).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbxConversionStatistics).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbxSeriesStatistics).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbxVolumesStatistics).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbxFilesStatistics).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbxAnalyticsTitle).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbxBtnExecute).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbxBtnGenerateCbz).EndInit();
            pnlSettings.ResumeLayout(false);
            pnlHeaderSettings.ResumeLayout(false);
            pnlHeaderSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pbxSettingsTitle).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbxBtnFolder).EndInit();
            pnlSettingsControlsGroup.ResumeLayout(false);
            pnlSettingsControlsGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudMinimalHeight).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbxBtnHelp).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbxBtnWarnings).EndInit();
            pnlTools.ResumeLayout(false);
            pnlMain.ResumeLayout(false);
            pnlMain.PerformLayout();
            pnlExecutionHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pbxExecutionLogsTitle).EndInit();
            mainStatusStrip.ResumeLayout(false);
            mainStatusStrip.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pbxLogo;
        private Panel pnlBottom;
        private ToolTip tips;
        private Panel pnlSettings;
        private FolderBrowserDialog folderDialog;
        private PictureBox pbxSettingsTitle;
        private TextBox txtDirectory;
        private PictureBox pbxAnalyticsTitle;
        private Panel pnlSettingsControlsGroup;
        private Panel pnlTools;
        private Panel pnlMain;
        private StatusStrip mainStatusStrip;
        private ToolStripStatusLabel toolStripStatusLabelMainArea;
        private Panel pnlHeaderSettings;
        private PictureBox pbxExecutionLogsTitle;
        private Panel pnlExecutionHeader;
        private ProgressBar progressBarExecution;
        private System.ComponentModel.BackgroundWorker backgroundWorkerExecution;
        private CheckBox cbxUseSelectedDirectory;
        private Label lblUnifyableDirectory;
        private TextBox txtUnifyableDirectory;
        private TextBox txtSplitableDirectory;
        private Label lblSplitableDirectory;
        private CheckBox cbxUpscaleImages;
        private NumericUpDown nudMinimalHeight;
        private Label lblReadingOrder;
        private ComboBox cbbReadOrder;
        private Panel pnlSplitter01;
        private Label lblVersion;
        internal PictureBox pbxBtnAdjustFiles;
        internal PictureBox pbxBtnUnifyPages;
        internal PictureBox pbxBtnSplitPages;
        internal PictureBox pbxBtnGenerateCbz;
        internal PictureBox pbxBtnHelp;
        internal PictureBox pbxBtnWarnings;
        internal protected PictureBox pbxBtnFolder;
        internal PictureBox pbxBtnCancel;
        internal PictureBox pbxBtnRefresh;
        internal PictureBox pbxBtnExecute;
        internal PictureBox pbxFilesStatistics;
        internal Label lblFilesAnalytics;
        internal PictureBox pbxVolumesStatistics;
        internal PictureBox pbxSeriesStatistics;
        internal Label lblVolumesAnalytics;
        internal Label lblSeriesAnalytics;
        internal Label lblConversionAnalytics;
        internal PictureBox pbxConversionStatistics;
        internal Label lblUnifyAnalytics;
        internal PictureBox pbxUnifyStatistics;
        internal Label lblSplitAnalytics;
        internal PictureBox pbxSplitStatistics;
        internal Label lblUnknwonAnalytics;
        internal PictureBox pbxUnknownStatistics;
        private CheckBox cbxBlankSpace;
        private Panel pnlSplitter03;
        private CheckBox cbxEnableBrightnessContrast;
        private TreeView treeViewVolumes;
    }
}