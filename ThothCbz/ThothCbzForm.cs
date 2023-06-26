using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

using ThothCbz.Actions;
using ThothCbz.Constants;
using ThothCbz.Entities;
using ThothCbz.Enumerators;
using ThothCbz.EventHandlers;
using ThothCbz.Extensions;
using ThothCbz.Properties;

namespace ThothCbz
{
    public partial class frmThotCbz : Form
    {
        private string _rtfExecutionLogsTextColorsConfigurationLine = string.Empty;

        public frmThotCbz()
        {
            InitializeComponent();

            tips.SetToolTip(pbxBtnAdjustFiles, Resources.PbxBtnAdjustFilesTooltipText);
            tips.SetToolTip(pbxBtnUnifyPages, Resources.PbxBtnUnifyPagesTooltipText);
            tips.SetToolTip(pbxBtnSplitPages, Resources.PbxBtnSplitPagesTooltipText);
            tips.SetToolTip(pbxBtnGenerateCbz, Resources.PbxBtnGenerateCbzTooltipText);
            tips.SetToolTip(pbxBtnFolder, Resources.PbxBtnFolderTooltipText);
            tips.SetToolTip(pbxBtnHelp, Resources.PbxBtnHelpTooltipText);
            tips.SetToolTip(pbxBtnWarnings, Resources.PbxBtnWarningTooltipText);
            tips.SetToolTip(pbxBtnRefresh, Resources.PbxBtnRefreshTooltipText);
            tips.SetToolTip(pbxBtnExecute, Resources.PbxBtnPlayTooltipText);
            tips.SetToolTip(pbxBtnCancel, Resources.PbxBtnCancelTooltipText);
            tips.SetToolTip(pbxFilesStatistics, Resources.PbxFilesStatisticsTooltipText);
            tips.SetToolTip(pbxVolumesStatistics, Resources.PbxVolumesStatisticsTooltipText);
            tips.SetToolTip(pbxSeriesStatistics, Resources.PbxSeriesStatisticsTooltipText);
            tips.SetToolTip(pbxConversionStatistics, Resources.PbxConversionStatisticsTooltipText);
            tips.SetToolTip(pbxUnifyStatistics, Resources.PbxUnifyStatisticsTooltipText);
            tips.SetToolTip(pbxSplitStatistics, Resources.PbxSplitStatisticsTooltipText);
            tips.SetToolTip(pbxUnknownStatistics, Resources.PbxUnknwonStatisticsTooltipText);

            pbxAnalyticsTitle.Image = Resources.analytics_titles;
            pbxSettingsTitle.Image = Resources.settings_titles;

            toolStripStatusLabelMainArea.Text = Resources.LblCancelRequestCommandText;
            cbxUseSelectedDirectory.Text = Resources.CbxUseSelectedDirectoryText;
            cbxBlankSpace.Text = Resources.CbxBlankSpaceText;
            cbxEnableBrightnessContrast.Text = string.Format(Resources.CbxEnableBrightnessContrastText, GlobalConstants.DEFAULT_FILES_TO_GRAYSCALE_FILE_NAME);
            lblUnifyableDirectory.Text = Resources.LblUnifyableDirectoryText;
            lblSplitableDirectory.Text = Resources.LblSplitableDirectoryText;
            cbxUpscaleImages.Text = Resources.CbxUpscaleImagesText;
            lblReadingOrder.Text = Resources.LblReadingOrderText;
            txtDirectory.Text = Resources.txtDirectoryDefaultText;

            txtDirectory.BackColor = GlobalConstants.DEFAULT_BACKGROUND_COLOR;
            lblFilesAnalytics.BackColor = GlobalConstants.DEFAULT_BACKGROUND_COLOR;
            lblVolumesAnalytics.BackColor = GlobalConstants.DEFAULT_BACKGROUND_COLOR;
            lblSeriesAnalytics.BackColor = GlobalConstants.DEFAULT_BACKGROUND_COLOR;
            lblConversionAnalytics.BackColor = GlobalConstants.DEFAULT_BACKGROUND_COLOR;
            lblUnifyAnalytics.BackColor = GlobalConstants.DEFAULT_BACKGROUND_COLOR;
            lblSplitAnalytics.BackColor = GlobalConstants.DEFAULT_BACKGROUND_COLOR;
            lblUnknwonAnalytics.BackColor = GlobalConstants.DEFAULT_BACKGROUND_COLOR;
            toolStripStatusLabelMainArea.BackColor = GlobalConstants.DEFAULT_BACKGROUND_COLOR;
            rtbExecutionLogs.BackColor = GlobalConstants.DEFAULT_LOG_BACKGROUND_COLOR;
            txtUnifyableDirectory.BackColor = GlobalConstants.DEFAULT_LOG_BACKGROUND_COLOR;
            txtSplitableDirectory.BackColor = GlobalConstants.DEFAULT_LOG_BACKGROUND_COLOR;
            nudMinimalHeight.BackColor = GlobalConstants.DEFAULT_LOG_BACKGROUND_COLOR;
            cbbReadOrder.BackColor = GlobalConstants.DEFAULT_LOG_BACKGROUND_COLOR;

            cbxUseSelectedDirectory.ForeColor = GlobalConstants.DEFAULT_ENABLED_TEXT_COLOR;
            cbxUpscaleImages.ForeColor = GlobalConstants.DEFAULT_ENABLED_TEXT_COLOR;
            cbxBlankSpace.ForeColor = GlobalConstants.DEFAULT_ENABLED_TEXT_COLOR;
            cbxEnableBrightnessContrast.ForeColor = GlobalConstants.DEFAULT_ENABLED_TEXT_COLOR;
            toolStripStatusLabelMainArea.ForeColor = GlobalConstants.DEFAULT_ENABLED_TEXT_COLOR;
            rtbExecutionLogs.ForeColor = GlobalConstants.DEFAULT_ENABLED_TEXT_COLOR;
            lblUnifyableDirectory.ForeColor = GlobalConstants.DEFAULT_ENABLED_TEXT_COLOR;
            lblSplitableDirectory.ForeColor = GlobalConstants.DEFAULT_ENABLED_TEXT_COLOR;
            txtUnifyableDirectory.ForeColor = GlobalConstants.DEFAULT_ENABLED_TEXT_COLOR;
            txtSplitableDirectory.ForeColor = GlobalConstants.DEFAULT_ENABLED_TEXT_COLOR;
            nudMinimalHeight.ForeColor = GlobalConstants.DEFAULT_ENABLED_TEXT_COLOR;
            cbbReadOrder.ForeColor = GlobalConstants.DEFAULT_ENABLED_TEXT_COLOR;
            lblReadingOrder.ForeColor = GlobalConstants.DEFAULT_ENABLED_TEXT_COLOR;
            lblVersion.ForeColor = GlobalConstants.DEFAULT_ENABLED_TEXT_COLOR;

            _rtfExecutionLogsTextColorsConfigurationLine = $@"{{\colortbl;\red{(int)GlobalConstants.DEFAULT_ENABLED_TEXT_COLOR.R}\green{(int)GlobalConstants.DEFAULT_ENABLED_TEXT_COLOR.G}\blue{(int)GlobalConstants.DEFAULT_ENABLED_TEXT_COLOR.B};" +
                        $@"\red{(int)GlobalConstants.DEFAULT_LOG_QUEUE_TEXT_COLOR.R}\green{(int)GlobalConstants.DEFAULT_LOG_QUEUE_TEXT_COLOR.G}\blue{(int)GlobalConstants.DEFAULT_LOG_QUEUE_TEXT_COLOR.B};" +
                        $@"\red{(int)GlobalConstants.DEFAULT_LOG_RUNNING_TEXT_COLOR.R}\green{(int)GlobalConstants.DEFAULT_LOG_RUNNING_TEXT_COLOR.G}\blue{(int)GlobalConstants.DEFAULT_LOG_RUNNING_TEXT_COLOR.B};" +
                        $@"\red{(int)GlobalConstants.DEFAULT_LOG_DONE_TEXT_COLOR.R}\green{(int)GlobalConstants.DEFAULT_LOG_DONE_TEXT_COLOR.G}\blue{(int)GlobalConstants.DEFAULT_LOG_DONE_TEXT_COLOR.B};" +
                        $@"\red{(int)GlobalConstants.DEFAULT_LOG_ERRO_TEXT_COLOR.R}\green{(int)GlobalConstants.DEFAULT_LOG_ERRO_TEXT_COLOR.G}\blue{(int)GlobalConstants.DEFAULT_LOG_ERRO_TEXT_COLOR.B};" +
                        $@"\red{(int)GlobalConstants.DEFAULT_LOG_WARNING_TEXT_COLOR.R}\green{(int)GlobalConstants.DEFAULT_LOG_WARNING_TEXT_COLOR.G}\blue{(int)GlobalConstants.DEFAULT_LOG_WARNING_TEXT_COLOR.B};" +
                        $@"}}";

            cbbReadOrder.Items.AddRange(new[] {
                        Resources.LblPagesReadOrderLeftRightText,
                        Resources.LblPagesReadOrderRightLeftText
                    }
                );

            foreach (var analyse in Enum.GetValues<AnalyseTypes>())
            {
                GetLabelByAnalyseType(analyse).Text = GlobalConstants.DEFAULT_EMPTY_STATISTIC_VALUE;
            }

            lblVersion.Text = string.Format(Resources.LblVersionText, FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion);

            ThothNotifyablePropertiesEntity.Default.Reset();

            SetEventHandlers();
            SettingsControlsDatabinding();

            pbxBtnExecute.Focus();
        }

        #region FORM EVENTS
        private void frmThotCbz_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.Save();
        }
        #endregion FORM EVENTS

        #region RICH TEXT BOX EVENTS
        private void rtbExecutionLogs_LinkClicked(
                object sender,
                LinkClickedEventArgs e
            )
        {
            if (string.IsNullOrWhiteSpace(e.LinkText))
            {
                return;
            }

            Process.Start("explorer.exe", e.LinkText.Replace('|', '\\'));
        }
        #endregion RICH TEXT BOX EVENTS

        #region PANEL EVENTS
        private void pnlBottom_Resize(
                object sender,
                EventArgs e
            )
        {
            pbxBtnExecute.Location = new System.Drawing.Point(pnlBottom.Width - (pbxBtnExecute.Width + 20), 20);
            pbxBtnRefresh.Location = new System.Drawing.Point(pbxBtnExecute.Location.X - (pbxBtnRefresh.Width + 20), 55);
            pbxBtnCancel.Location = new System.Drawing.Point(pbxBtnRefresh.Location.X - (pbxBtnCancel.Width + 20), 55);
            lblVersion.Location = new System.Drawing.Point(pnlBottom.Width - (lblVersion.Width + 25), 176);
        }

        private void pnlSeries_Resize(
                object sender,
                EventArgs e
            )
        {
            progressBarExecution.Location = new System.Drawing.Point(pnlExecutionHeader.Width - (progressBarExecution.Width + 10), 24);
        }
        #endregion PANEL EVENTS

        #region BACKGROUNDWORKER EVENTS
        private void backgroundWorkerExecution_DoWork(
                object sender,
                DoWorkEventArgs e
            )
        {
            Invoke(delegate
            {
                FillExecutionLogs();

                progressBarExecution.BeginInvoke(delegate
                {
                    progressBarExecution.Value = 0;
                    progressBarExecution.Maximum = (
                                                        (ThothNotifyablePropertiesEntity.Default.FilesCount +
                                                        (ThothNotifyablePropertiesEntity.Default.GenerateCbzActive && !Settings.Default.DisableGbzGeneration ? ThothNotifyablePropertiesEntity.Default.SeriesCount : 0) +
                                                        (ThothNotifyablePropertiesEntity.Default.SplitPagesActive ? ThothNotifyablePropertiesEntity.Default.SplittableFilesCount : 0) +
                                                        (ThothNotifyablePropertiesEntity.Default.UnifyPagesActive ? ThothNotifyablePropertiesEntity.Default.UnifyableFilesCount : 0) +
                                                        (ThothNotifyablePropertiesEntity.Default.GenerateCbzActive && !Settings.Default.DisableGbzGeneration ? ThothNotifyablePropertiesEntity.Default.VolumesCount : 0) +
                                                        (ThothNotifyablePropertiesEntity.Default.GenerateCbzActive && !Settings.Default.DisableGbzGeneration ? ThothNotifyablePropertiesEntity.Default.FilesCount : 0))
                                                    );
                });
            });

            if (ThothNotifyablePropertiesEntity.Default.GenerateCbzActive && !Settings.Default.DisableGbzGeneration)
            {
                ThothNotifyablePropertiesEntity.Default.SeriesSetted = new ConcurrentBag<int>();
                ThothNotifyablePropertiesEntity.Default.VolumesSetted = new ConcurrentBag<int>();

                ThothNotifyablePropertiesEntity.Default.SeriesDictionary.Keys.ToList().ForEach(key =>
                {
                    if (ThothNotifyablePropertiesEntity.Default.SeriesDictionary[key].Count == ThothNotifyablePropertiesEntity.Default.SeriesDictionary[key].Where(w => w.FileWasCompressed).Count())
                    {
                        ThothNotifyablePropertiesEntity.Default.SeriesSetted.Add(1);
                    }

                    ThothNotifyablePropertiesEntity.Default.VolumesSetted.Add(
                            ThothNotifyablePropertiesEntity.Default.SeriesDictionary[key]
                                    .GroupBy(g => g.Volume)
                                    .Where(w => !string.IsNullOrWhiteSpace(w.Key) && w.Count() == w.Where(w2 => w2.FileWasCompressed).Count())
                                    .Select(s => 1)
                                    .Sum(s => s)
                        );
                });
            }

            try
            {
                foreach (var seriesKey in ThothNotifyablePropertiesEntity.Default.SeriesDictionary.Keys.OrderBy(o => o))
                {
                    if (ThothNotifyablePropertiesEntity.Default.CancelGenerationProcessQueued)
                    {
                        return;
                    }

                    var filesToGrayscale = new List<string>();

                    var filesToGrayscalePath = ThothNotifyablePropertiesEntity.Default.SeriesDictionary[seriesKey].FilesToGrayScaleFilePath();

                    if (!string.IsNullOrWhiteSpace(filesToGrayscalePath) && File.Exists(filesToGrayscalePath))
                    {
                        filesToGrayscale = File.ReadAllLines(filesToGrayscalePath).ToList();
                    }

                    var volumes = ThothNotifyablePropertiesEntity.Default.SeriesDictionary[seriesKey]
                                                    .OrderBy(o => o.Volume)
                                                    .GroupBy(g => g.Volume)
                                                    .Select(s => s.Select(m => m).ToList())
                                                    .ToList();

                    var customBlankFilePath = Directory.GetFiles(ThothNotifyablePropertiesEntity.Default.SeriesDictionary[seriesKey].First().SeriePath.Replace('|', '\\'), GlobalConstants.DEFAULT_BLANK_FILE_NAME).FirstOrDefault();
                    var defaultFileToSize = Directory.GetFiles(ThothNotifyablePropertiesEntity.Default.SeriesDictionary[seriesKey].First().SeriePath.Replace('|', '\\'), GlobalConstants.DEFAULT_TEMPLATE_FILE_NAME).FirstOrDefault();

                    if (ThothNotifyablePropertiesEntity.Default.AdjustFilesActive)
                    {
                        foreach (var volume in volumes)
                        {
                            VolumeGenerations(
                                                volume,
                                                filesToGrayscale,
                                                customBlankFilePath,
                                                defaultFileToSize
                                            );
                        }
                    }
                    else
                    {
                        Parallel.ForEach(volumes, volume =>
                                                    {
                                                        VolumeGenerations(
                                                            volume,
                                                            filesToGrayscale,
                                                            customBlankFilePath,
                                                            defaultFileToSize
                                                        );
                                                    });
                    }

                    if (!string.IsNullOrWhiteSpace(defaultFileToSize) && File.Exists(defaultFileToSize))
                    {
                        File.Delete(defaultFileToSize);
                    }

                    if (!string.IsNullOrWhiteSpace(customBlankFilePath) && ThothNotifyablePropertiesEntity.Default.GenerateCbzActive && File.Exists(customBlankFilePath))
                    {
                        File.Delete(customBlankFilePath);
                    }

                    Invoke(delegate
                    {
                        if (ThothNotifyablePropertiesEntity.Default.GenerateCbzActive && !Settings.Default.DisableGbzGeneration)
                        {
                            ThothNotifyablePropertiesEntity.Default.SeriesSetted.Add(1);
                            ThothNotifyablePropertiesEntity.Default.ForceNotification(nameof(ThothNotifyablePropertiesEntity.Default.SeriesSetted));
                        }
                    });
                };
            }
            catch (Exception ex) 
            {
                ex.InformAndSaveLog();

                Invoke(delegate
                {
                    ThothNotifyablePropertiesEntity.Default.GenerationProcessRunning = false;
                    ThothNotifyablePropertiesEntity.Default.CancelGenerationProcessQueued = false;

                    FillExecutionLogs();
                });
            }
        }

        private void VolumeGenerations(
                List<FileEntity> volume,
                List<string> filesToGrayscale,
                string? customBlankFilePath,
                string? defaultFileToSize
            )
        {
            if (ThothNotifyablePropertiesEntity.Default.CancelGenerationProcessQueued)
            {
                if (ThothNotifyablePropertiesEntity.Default.GenerationProcessRunning)
                {
                    Invoke(delegate
                    {
                        ThothNotifyablePropertiesEntity.Default.GenerationProcessRunning = false;
                    });
                }

                return;
            }

            if (volume.Count > 1)
            {
                string volumePath = volume.FirstOrDefault()!.SeriePath!.Replace('|', '\\') +
                                        (!string.IsNullOrWhiteSpace(volume.FirstOrDefault()!.Volume) ? $"\\{volume.FirstOrDefault()!.Volume}" : string.Empty);

                var volumeDefaultFileToSize = Directory.GetFiles(volumePath, GlobalConstants.DEFAULT_TEMPLATE_FILE_NAME).FirstOrDefault();

                foreach (var chapter in volume.OrderBy(o => o.Chapter)
                                                .GroupBy(g => g.Chapter)
                                                .Select(s => s.Select(m => m).ToList())
                                                .ToList())
                {
                    if (ThothNotifyablePropertiesEntity.Default.AdjustFilesActive)
                    {
                        System.Drawing.Size? defaultSize = null;

                        var chapterDefaultFileToSize = Directory.GetFiles(
                                                            $"{volumePath}\\{chapter.FirstOrDefault()!.Chapter}"
                                                            , GlobalConstants.DEFAULT_TEMPLATE_FILE_NAME
                                                        ).FirstOrDefault();

                        if (!string.IsNullOrWhiteSpace(chapterDefaultFileToSize) || 
                            !string.IsNullOrWhiteSpace(volumeDefaultFileToSize) || 
                            !string.IsNullOrWhiteSpace(defaultFileToSize))
                        {
                            using var img = System.Drawing.Image.FromFile(
                                    !string.IsNullOrWhiteSpace(chapterDefaultFileToSize)
                                        ? chapterDefaultFileToSize
                                        : !string.IsNullOrWhiteSpace(volumeDefaultFileToSize)
                                            ? volumeDefaultFileToSize
                                            : defaultFileToSize!
                                );

                            defaultSize = img.Size;

                            img.Dispose();

                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                        }

                        Adjustments.ExecuteAdjustments(
                                filesToAdjust: chapter.Where(w => !w.FileWasAdjusted).ToList(),
                                filesToGrayscale: filesToGrayscale,
                                defaultSize: defaultSize
                            );

                        if (!string.IsNullOrWhiteSpace(chapterDefaultFileToSize) && File.Exists(chapterDefaultFileToSize))
                        {
                            File.Delete(chapterDefaultFileToSize);
                        }
                    }

                    if (ThothNotifyablePropertiesEntity.Default.SplitPagesActive)
                    {
                        Split.ExecuteSpliting(
                                filesToAdjust: chapter.Where(w => w.IsSplit && !w.FileWasSplit).ToList()
                            );
                    }

                    if (ThothNotifyablePropertiesEntity.Default.UnifyPagesActive)
                    {
                        Unify.ExecuteUnification(
                                filesToAdjust: chapter.Where(w => w.IsUnify && !w.FileWasUnified).ToList()
                            );
                    }
                }

                if (!string.IsNullOrWhiteSpace(volumeDefaultFileToSize) && File.Exists(volumeDefaultFileToSize))
                {
                    File.Delete(volumeDefaultFileToSize);
                }

                if (ThothNotifyablePropertiesEntity.Default.GenerateCbzActive)
                {
                    Renamer.ExecuteRenamingAndMove(
                            filesToAdjust: volume.Where(w => !w.FileWasRenamed).ToList(),
                            customBlankFilePath: customBlankFilePath
                        );

                    if (!Settings.Default.DisableGbzGeneration)
                    {
                        GenerateCbz.ExecuteCbzGeneration(
                                filesToAdjust: volume.Where(w => !w.FileWasCompressed).ToList()
                            );
                    }
                }
            }
            else if (volume.Count == 1 && !volume.First().FileWasAdjusted)
            {
                volume.First().FileWasAdjusted = true;
                volume.First().FileWasCompressed = true;
                volume.First().FileWasRenamed = true;

                Invoke(delegate
                {
                    ThothNotifyablePropertiesEntity.Default.FilesSetted.Add(1);
                    ThothNotifyablePropertiesEntity.Default.ForceNotification(nameof(ThothNotifyablePropertiesEntity.Default.FilesSetted));

                    if (volume.First().NeedConversion)
                    {
                        ThothNotifyablePropertiesEntity.Default.AdjustableFilesSetted.Add(1);
                        ThothNotifyablePropertiesEntity.Default.ForceNotification(nameof(ThothNotifyablePropertiesEntity.Default.AdjustableFilesSetted));
                    }

                    ThothNotifyablePropertiesEntity.Default.CbzFilesSetted.Add(1);
                    ThothNotifyablePropertiesEntity.Default.ForceNotification(nameof(ThothNotifyablePropertiesEntity.Default.CbzFilesSetted));
                });
            }

            if (!string.IsNullOrWhiteSpace(volume.First().Volume) && ThothNotifyablePropertiesEntity.Default.GenerateCbzActive && !Settings.Default.DisableGbzGeneration)
            {
                Invoke(delegate
                {
                    ThothNotifyablePropertiesEntity.Default.VolumesSetted.Add(1);
                    ThothNotifyablePropertiesEntity.Default.ForceNotification(nameof(ThothNotifyablePropertiesEntity.Default.VolumesSetted));
                });
            }
        }

        private void backgroundWorkerExecution_RunWorkerCompleted(
                object sender,
                RunWorkerCompletedEventArgs e
            )
        {
            Invoke(delegate
            {
                FillExecutionLogs();
                ThothNotifyablePropertiesEntity.Default.GenerationProcessRunning = false;
                ThothNotifyablePropertiesEntity.Default.CancelGenerationProcessQueued = false;

                ThothNotifyablePropertiesEntity.Default.AdjustFilesActive = false;
                ThothNotifyablePropertiesEntity.Default.SplitPagesActive = false;
                ThothNotifyablePropertiesEntity.Default.UnifyPagesActive = false;
                ThothNotifyablePropertiesEntity.Default.GenerateCbzActive = false;
            });
        }
        #endregion

        #region LABEL ACTION METHODS
        private Label GetLabelByAnalyseType(
                AnalyseTypes action
            )
        {
            return action switch
            {
                AnalyseTypes.ConversionStatistics => lblConversionAnalytics,
                AnalyseTypes.FilesStatistics => lblFilesAnalytics,
                AnalyseTypes.SeriesStatistics => lblSeriesAnalytics,
                AnalyseTypes.SplitStatistics => lblSplitAnalytics,
                AnalyseTypes.UnifyStatistics => lblUnifyAnalytics,
                AnalyseTypes.UnknownStatistics => lblUnknwonAnalytics,
                AnalyseTypes.VolumesStatistics => lblVolumesAnalytics,
                _ => throw new NotImplementedException()
            };
        }
        #endregion LABEL ACTION METHODS

        #region BUTTON ACTION METHODS
        private PictureBox GetButtonByActionType(
                ActionTypes action
            )
        {
            return action switch
            {
                ActionTypes.Adjustments => pbxBtnAdjustFiles,
                ActionTypes.Unifications => pbxBtnUnifyPages,
                ActionTypes.Splits => pbxBtnSplitPages,
                ActionTypes.Cbzs => pbxBtnGenerateCbz,
                ActionTypes.Folder => pbxBtnFolder,
                ActionTypes.Help => pbxBtnHelp,
                ActionTypes.Warnings => pbxBtnWarnings,
                ActionTypes.Refresh => pbxBtnRefresh,
                ActionTypes.Play => pbxBtnExecute,
                ActionTypes.Cancel => pbxBtnCancel,
                _ => throw new NotImplementedException()
            };
        }

        private PictureBox GetButtonByAnalyseType(
                AnalyseTypes action
            )
        {
            return action switch
            {
                AnalyseTypes.ConversionStatistics => pbxConversionStatistics,
                AnalyseTypes.FilesStatistics => pbxFilesStatistics,
                AnalyseTypes.SeriesStatistics => pbxSeriesStatistics,
                AnalyseTypes.SplitStatistics => pbxSplitStatistics,
                AnalyseTypes.UnifyStatistics => pbxUnifyStatistics,
                AnalyseTypes.UnknownStatistics => pbxUnknownStatistics,
                AnalyseTypes.VolumesStatistics => pbxVolumesStatistics,
                _ => throw new NotImplementedException()
            };
        }

        private void FillExecutionLogs()
        {
            rtbExecutionLogs.BeginInvoke(delegate
            {
                if (!ThothNotifyablePropertiesEntity.Default.SeriesDictionary.Any())
                {
                    rtbExecutionLogs.Clear();
                }
            });

            if (!ThothNotifyablePropertiesEntity.Default.SeriesDictionary.Any())
            {
                return;
            }

            var linesResult = new List<string>
            {
                _rtfExecutionLogsTextColorsConfigurationLine
            };

            var bagLines = new ConcurrentBag<(string, List<string>)>();

            Parallel.ForEach(ThothNotifyablePropertiesEntity.Default.SeriesDictionary.Keys, serie =>
            {
                var lines = new List<string>();
                var items = ThothNotifyablePropertiesEntity.Default.SeriesDictionary[serie];

                var seriesPath = items.First().SeriePath;
                var seriesStatus = GetExecutionStatusByLevel(
                                        items: items
                                    );
                lines.Add(Resources.LblExecutionLogSeriesText.ReplaceRtfName(serie.ToUpper())
                            .ReplaceRtfUri(seriesPath)
                            .ReplaceRtfStatus(seriesStatus.GetExecutionStatusText())
                            .ReplaceRtfColor(seriesStatus.GetExecutionStatusColorText()));

                items.Where(w => !string.IsNullOrWhiteSpace(w?.Volume))
                        .GroupBy(g2 => g2.Volume)
                        .OrderBy(o2 => o2.Key)
                        .ToList()
                        .ForEach(f2 =>
                        {
                            var volumeStatus = GetExecutionStatusByLevel(
                                            items: items,
                                            volumeName: f2.Key
                                        );
                            lines.Add(Resources.LblExecutionLogVolumesText.ReplaceRtfName(f2.Key!.ToUpper())
                                        .ReplaceRtfUri($@"{seriesPath}|{f2.Key}")
                                        .ReplaceRtfStatus(volumeStatus.GetExecutionStatusText())
                                        .ReplaceRtfColor(volumeStatus.GetExecutionStatusColorText()));

                            f2.Where(w2 => !string.IsNullOrWhiteSpace(w2?.Chapter))
                                .GroupBy(g3 => g3.Chapter)
                                .OrderBy(o3 => o3.Key)
                                .ToList()
                                .ForEach(f3 =>
                                {
                                    var chapterStatus = GetExecutionStatusByLevel(
                                            items: items,
                                            volumeName: f2.Key,
                                            chapterName: f3.Key
                                        );

                                    lines.Add(Resources.LblExecutionLogChaptersText.ReplaceRtfName(f3.Key!.ToUpper())
                                                .ReplaceRtfUri($@"{seriesPath}|{f2.Key}|{f3.Key}")
                                                .ReplaceRtfStatus(chapterStatus.GetExecutionStatusText())
                                                .ReplaceRtfColor(chapterStatus.GetExecutionStatusColorText()));
                                });
                        });

                lines.Add(string.Empty);
                bagLines.Add((serie, lines));
            });

            linesResult.AddRange(bagLines.OrderBy(o => o.Item1).SelectMany(s => s.Item2).ToList());
            linesResult.Add(string.Empty);

            rtbExecutionLogs.BeginInvoke(delegate
            {
                rtbExecutionLogs.Rtf = GenerateRtfWithMultipleLines(
                                        linesResult
                                    );
            });
        }

        private ExecutionStatusType GetExecutionStatusByLevel(
                List<FileEntity> items,
                string? volumeName = null,
                string? chapterName = null
            )
        {
            if (!items.Any())
            {
                return ExecutionStatusType.NotRunning;
            }

            var result = items
                            .Where(w => (string.IsNullOrWhiteSpace(volumeName) || w.Volume == volumeName)
                                    && (string.IsNullOrWhiteSpace(chapterName) || w.Chapter == chapterName))
                            .ToList();

            return !ThothNotifyablePropertiesEntity.Default.GenerationProcessRunning && result.Where(w => !w.IsAcceptableFileType).Any()
                    ? ExecutionStatusType.Error
                    : !ThothNotifyablePropertiesEntity.Default.GenerationProcessRunning && ThothNotifyablePropertiesEntity.Default.UnifyPagesActive && result.Where(w => w.IsUnify).Count() % 2 != 0
                        ? ExecutionStatusType.Error
                        : !ThothNotifyablePropertiesEntity.Default.GenerationProcessRunning && (result.Where(w => w.NeedConversion).Any() && result.Where(w => w.NeedConversion).Count() != result.Where(w => w.NeedConversion && w.FileWasAdjusted).Count())
                            ? ExecutionStatusType.Warning
                            : (!ThothNotifyablePropertiesEntity.Default.AdjustFilesActive || (result.Count == result.Where(w => w.FileWasAdjusted).Count())) &&
                                (!ThothNotifyablePropertiesEntity.Default.SplitPagesActive || (result.Where(w => w.IsSplit).Count() == result.Where(w => w.FileWasSplit).Count())) &&
                                (!ThothNotifyablePropertiesEntity.Default.UnifyPagesActive || (result.Where(w => w.IsUnify).Count() == result.Where(w => w.FileWasUnified).Count())) &&
                                (!ThothNotifyablePropertiesEntity.Default.GenerateCbzActive || (result.Count() == result.Where(w => w.FileWasRenamed).Count() &&
                                                                                                (Settings.Default.DisableGbzGeneration || result.Count() == result.Where(w => w.FileWasCompressed).Count()))
                                )
                                ? ExecutionStatusType.Done
                                : result.Where(w => w.FileWasAdjusted).Any() && ThothNotifyablePropertiesEntity.Default.GenerationProcessRunning
                                    ? ExecutionStatusType.Running
                                    : !ThothNotifyablePropertiesEntity.Default.GenerationProcessRunning
                                        ? ExecutionStatusType.NotRunning
                                        : ExecutionStatusType.Queued;
        }
        #endregion BUTTON ACTION METHODS

        #region RICHTEXTBOX ACTION METHODS
        private string GenerateRtfWithMultipleLines(
                List<string>? lines
            )
        {
            if (lines?.Where(w => !string.IsNullOrWhiteSpace(w))?.Any() != true)
            {
                return string.Empty;
            }

            var stb = new StringBuilder();

            stb.Append(@"{\rtf1 ");

            var addNewLine = false;

            foreach (var line in lines)
            {
                if (addNewLine)
                {
                    stb.Append(@"\line ");
                }

                stb.Append(line);
                addNewLine = true;
            }

            stb.Append(@"}");

            return stb.ToString();
        }
        #endregion

        #region GLOBAL ACTION METHODS
        private void SettingsControlsDatabinding()
        {
            pbxBtnFolder.DataBindings.Add(nameof(pbxBtnFolder.Enabled), ThothNotifyablePropertiesEntity.Default, nameof(ThothNotifyablePropertiesEntity.Default.EnableControls), false, DataSourceUpdateMode.OnPropertyChanged);

            pbxBtnAdjustFiles.DataBindings.Add(nameof(pbxBtnAdjustFiles.Enabled), ThothNotifyablePropertiesEntity.Default, nameof(ThothNotifyablePropertiesEntity.Default.EnabledAdjustFiles), false, DataSourceUpdateMode.OnPropertyChanged);
            pbxBtnSplitPages.DataBindings.Add(nameof(pbxBtnSplitPages.Enabled), ThothNotifyablePropertiesEntity.Default, nameof(ThothNotifyablePropertiesEntity.Default.EnabledSplitPages), false, DataSourceUpdateMode.OnPropertyChanged);
            pbxBtnUnifyPages.DataBindings.Add(nameof(pbxBtnUnifyPages.Enabled), ThothNotifyablePropertiesEntity.Default, nameof(ThothNotifyablePropertiesEntity.Default.EnabledUnifyPages), false, DataSourceUpdateMode.OnPropertyChanged);
            pbxBtnGenerateCbz.DataBindings.Add(nameof(pbxBtnGenerateCbz.Enabled), ThothNotifyablePropertiesEntity.Default, nameof(ThothNotifyablePropertiesEntity.Default.EnabledGenerateCbz), false, DataSourceUpdateMode.OnPropertyChanged);

            pbxBtnWarnings.DataBindings.Add(nameof(pbxBtnWarnings.Visible), ThothNotifyablePropertiesEntity.Default, nameof(ThothNotifyablePropertiesEntity.Default.ExistWarnings), false, DataSourceUpdateMode.Never);
            pbxBtnWarnings.DataBindings.Add(nameof(pbxBtnWarnings.Enabled), ThothNotifyablePropertiesEntity.Default, nameof(ThothNotifyablePropertiesEntity.Default.ExistWarnings), false, DataSourceUpdateMode.Never);

            pbxBtnRefresh.DataBindings.Add(nameof(pbxBtnRefresh.Enabled), ThothNotifyablePropertiesEntity.Default, nameof(ThothNotifyablePropertiesEntity.Default.EnableOptionControls), false, DataSourceUpdateMode.OnPropertyChanged);

            pbxBtnExecute.DataBindings.Add(nameof(pbxBtnExecute.Enabled), ThothNotifyablePropertiesEntity.Default, nameof(ThothNotifyablePropertiesEntity.Default.EnableGenerationProcess), false, DataSourceUpdateMode.OnPropertyChanged);

            pbxBtnCancel.DataBindings.Add(nameof(pbxBtnCancel.Enabled), ThothNotifyablePropertiesEntity.Default, nameof(ThothNotifyablePropertiesEntity.Default.EnabledCancelGenerationProcess), false, DataSourceUpdateMode.OnPropertyChanged);
            pbxBtnCancel.DataBindings.Add(nameof(pbxBtnCancel.Visible), ThothNotifyablePropertiesEntity.Default, nameof(ThothNotifyablePropertiesEntity.Default.GenerationProcessRunning), false, DataSourceUpdateMode.OnPropertyChanged);

            cbxUseSelectedDirectory.DataBindings.Add(nameof(cbxUseSelectedDirectory.Checked), Settings.Default, nameof(Settings.Default.UseSelectedFolderAsPartOfTheFileStructure), false, DataSourceUpdateMode.OnPropertyChanged);
            cbxUseSelectedDirectory.DataBindings.Add(nameof(cbxUseSelectedDirectory.Enabled), ThothNotifyablePropertiesEntity.Default, nameof(ThothNotifyablePropertiesEntity.Default.EnableControls), false, DataSourceUpdateMode.Never);

            cbxUpscaleImages.DataBindings.Add(nameof(cbxUpscaleImages.Checked), Settings.Default, nameof(Settings.Default.EnableUpscale), false, DataSourceUpdateMode.OnPropertyChanged);
            cbxUpscaleImages.DataBindings.Add(nameof(cbxUpscaleImages.Enabled), ThothNotifyablePropertiesEntity.Default, nameof(ThothNotifyablePropertiesEntity.Default.EnableAdjustControls), false, DataSourceUpdateMode.Never);
            cbxUpscaleImages.DataBindings.Add(nameof(cbxUpscaleImages.Visible), ThothNotifyablePropertiesEntity.Default, nameof(ThothNotifyablePropertiesEntity.Default.AnalysisExecuted), false, DataSourceUpdateMode.Never);

            cbxEnableBrightnessContrast.DataBindings.Add(nameof(cbxEnableBrightnessContrast.Checked), Settings.Default, nameof(Settings.Default.EnableBrightnessAndContrastAdjustments), false, DataSourceUpdateMode.OnPropertyChanged);
            cbxEnableBrightnessContrast.DataBindings.Add(nameof(cbxEnableBrightnessContrast.Enabled), ThothNotifyablePropertiesEntity.Default, nameof(ThothNotifyablePropertiesEntity.Default.EnableAdjustControls), false, DataSourceUpdateMode.Never);
            cbxEnableBrightnessContrast.DataBindings.Add(nameof(cbxEnableBrightnessContrast.Visible), ThothNotifyablePropertiesEntity.Default, nameof(ThothNotifyablePropertiesEntity.Default.AnalysisExecuted), false, DataSourceUpdateMode.Never);

            txtSplitableDirectory.DataBindings.Add(nameof(txtSplitableDirectory.Text), Settings.Default, nameof(Settings.Default.DefaultSplitFolderName), false, DataSourceUpdateMode.OnPropertyChanged);
            txtSplitableDirectory.DataBindings.Add(nameof(txtSplitableDirectory.Enabled), ThothNotifyablePropertiesEntity.Default, nameof(ThothNotifyablePropertiesEntity.Default.EnableControls), false, DataSourceUpdateMode.Never);

            txtUnifyableDirectory.DataBindings.Add(nameof(txtUnifyableDirectory.Text), Settings.Default, nameof(Settings.Default.DefaultUnifyFolderName), false, DataSourceUpdateMode.OnPropertyChanged);
            txtUnifyableDirectory.DataBindings.Add(nameof(txtUnifyableDirectory.Enabled), ThothNotifyablePropertiesEntity.Default, nameof(ThothNotifyablePropertiesEntity.Default.EnableControls), false, DataSourceUpdateMode.Never);

            nudMinimalHeight.DataBindings.Add(nameof(nudMinimalHeight.Value), Settings.Default, nameof(Settings.Default.MinimalImageHeight), false, DataSourceUpdateMode.OnPropertyChanged);
            nudMinimalHeight.DataBindings.Add(nameof(nudMinimalHeight.Enabled), ThothNotifyablePropertiesEntity.Default, nameof(ThothNotifyablePropertiesEntity.Default.EnableAdjustControls), false, DataSourceUpdateMode.Never);
            nudMinimalHeight.DataBindings.Add(nameof(nudMinimalHeight.Visible), ThothNotifyablePropertiesEntity.Default, nameof(ThothNotifyablePropertiesEntity.Default.AnalysisExecuted), false, DataSourceUpdateMode.Never);

            cbbReadOrder.DataBindings.Add(nameof(cbbReadOrder.SelectedIndex), Settings.Default, nameof(Settings.Default.ReadOrder), false, DataSourceUpdateMode.OnPropertyChanged);
            cbbReadOrder.DataBindings.Add(nameof(cbbReadOrder.Enabled), ThothNotifyablePropertiesEntity.Default, nameof(ThothNotifyablePropertiesEntity.Default.EnableAdjustControls), false, DataSourceUpdateMode.Never);
            cbbReadOrder.DataBindings.Add(nameof(cbbReadOrder.Visible), ThothNotifyablePropertiesEntity.Default, nameof(ThothNotifyablePropertiesEntity.Default.AnalysisExecuted), false, DataSourceUpdateMode.Never);

            lblReadingOrder.DataBindings.Add(nameof(lblReadingOrder.Enabled), ThothNotifyablePropertiesEntity.Default, nameof(ThothNotifyablePropertiesEntity.Default.EnableAdjustControls), false, DataSourceUpdateMode.Never);
            lblReadingOrder.DataBindings.Add(nameof(lblReadingOrder.Visible), ThothNotifyablePropertiesEntity.Default, nameof(ThothNotifyablePropertiesEntity.Default.AnalysisExecuted), false, DataSourceUpdateMode.Never);

            pnlSplitter01.DataBindings.Add(nameof(pnlSplitter01.Visible), ThothNotifyablePropertiesEntity.Default, nameof(ThothNotifyablePropertiesEntity.Default.AnalysisExecuted), false, DataSourceUpdateMode.Never);

            progressBarExecution.DataBindings.Add(nameof(progressBarExecution.Visible), ThothNotifyablePropertiesEntity.Default, nameof(ThothNotifyablePropertiesEntity.Default.GenerationProcessRunning), false, DataSourceUpdateMode.Never);

            pbxFilesStatistics.DataBindings.Add(nameof(pbxFilesStatistics.Enabled), ThothNotifyablePropertiesEntity.Default, nameof(ThothNotifyablePropertiesEntity.Default.ExistFiles), false, DataSourceUpdateMode.Never);
            lblFilesAnalytics.DataBindings.Add(nameof(lblFilesAnalytics.Enabled), ThothNotifyablePropertiesEntity.Default, nameof(ThothNotifyablePropertiesEntity.Default.ExistFiles), false, DataSourceUpdateMode.Never);

            pbxVolumesStatistics.DataBindings.Add(nameof(pbxVolumesStatistics.Enabled), ThothNotifyablePropertiesEntity.Default, nameof(ThothNotifyablePropertiesEntity.Default.ExistVolumesFiles), false, DataSourceUpdateMode.Never);
            lblVolumesAnalytics.DataBindings.Add(nameof(lblVolumesAnalytics.Enabled), ThothNotifyablePropertiesEntity.Default, nameof(ThothNotifyablePropertiesEntity.Default.ExistVolumesFiles), false, DataSourceUpdateMode.Never);

            pbxSeriesStatistics.DataBindings.Add(nameof(pbxSeriesStatistics.Enabled), ThothNotifyablePropertiesEntity.Default, nameof(ThothNotifyablePropertiesEntity.Default.ExistSeriesFiles), false, DataSourceUpdateMode.Never);
            lblSeriesAnalytics.DataBindings.Add(nameof(lblSeriesAnalytics.Enabled), ThothNotifyablePropertiesEntity.Default, nameof(ThothNotifyablePropertiesEntity.Default.ExistSeriesFiles), false, DataSourceUpdateMode.Never);

            pbxUnknownStatistics.DataBindings.Add(nameof(pbxUnknownStatistics.Enabled), ThothNotifyablePropertiesEntity.Default, nameof(ThothNotifyablePropertiesEntity.Default.ExistUnknownFiles), false, DataSourceUpdateMode.Never);
            lblUnknwonAnalytics.DataBindings.Add(nameof(lblUnknwonAnalytics.Enabled), ThothNotifyablePropertiesEntity.Default, nameof(ThothNotifyablePropertiesEntity.Default.ExistUnknownFiles), false, DataSourceUpdateMode.Never);

            pbxConversionStatistics.DataBindings.Add(nameof(pbxConversionStatistics.Enabled), ThothNotifyablePropertiesEntity.Default, nameof(ThothNotifyablePropertiesEntity.Default.ExistAdjustableFiles), false, DataSourceUpdateMode.Never);
            lblConversionAnalytics.DataBindings.Add(nameof(lblConversionAnalytics.Enabled), ThothNotifyablePropertiesEntity.Default, nameof(ThothNotifyablePropertiesEntity.Default.ExistAdjustableFiles), false, DataSourceUpdateMode.Never);

            pbxUnifyStatistics.DataBindings.Add(nameof(pbxUnifyStatistics.Enabled), ThothNotifyablePropertiesEntity.Default, nameof(ThothNotifyablePropertiesEntity.Default.ExistUnifyableFiles), false, DataSourceUpdateMode.Never);
            lblUnifyAnalytics.DataBindings.Add(nameof(lblUnifyAnalytics.Enabled), ThothNotifyablePropertiesEntity.Default, nameof(ThothNotifyablePropertiesEntity.Default.ExistUnifyableFiles), false, DataSourceUpdateMode.Never);

            pbxSplitStatistics.DataBindings.Add(nameof(pbxSplitStatistics.Enabled), ThothNotifyablePropertiesEntity.Default, nameof(ThothNotifyablePropertiesEntity.Default.ExistSplittableFiles), false, DataSourceUpdateMode.Never);
            lblSplitAnalytics.DataBindings.Add(nameof(lblSplitAnalytics.Enabled), ThothNotifyablePropertiesEntity.Default, nameof(ThothNotifyablePropertiesEntity.Default.ExistSplittableFiles), false, DataSourceUpdateMode.Never);

            txtDirectory.DataBindings.Add(nameof(txtDirectory.Text), ThothNotifyablePropertiesEntity.Default, nameof(ThothNotifyablePropertiesEntity.Default.DirectoryPathToAnalyze), false, DataSourceUpdateMode.Never);

            pnlSplitter03.DataBindings.Add(nameof(pnlSplitter03.Visible), ThothNotifyablePropertiesEntity.Default, nameof(ThothNotifyablePropertiesEntity.Default.AnalysisExecuted), false, DataSourceUpdateMode.Never);

            cbxBlankSpace.DataBindings.Add(nameof(cbxBlankSpace.Checked), Settings.Default, nameof(Settings.Default.EnableSpaceInUnifyablePages), false, DataSourceUpdateMode.OnPropertyChanged);
            cbxBlankSpace.DataBindings.Add(nameof(cbxBlankSpace.Enabled), ThothNotifyablePropertiesEntity.Default, nameof(ThothNotifyablePropertiesEntity.Default.EnableUnifyableControls), false, DataSourceUpdateMode.Never);
            cbxBlankSpace.DataBindings.Add(nameof(cbxBlankSpace.Visible), ThothNotifyablePropertiesEntity.Default, nameof(ThothNotifyablePropertiesEntity.Default.AnalysisExecuted), false, DataSourceUpdateMode.Never);

            mainStatusStrip.DataBindings.Add(nameof(mainStatusStrip.Visible), ThothNotifyablePropertiesEntity.Default, nameof(ThothNotifyablePropertiesEntity.Default.CancelGenerationProcessQueued), false, DataSourceUpdateMode.Never);
        }

        private void SetEventHandlers()
        {
            foreach (var action in Enum.GetValues<ActionTypes>())
            {
                var pbxButton = GetButtonByActionType(action);

                pbxButton.Click += PictureBoxEventHandler.PictureBoxButtonClick;
                pbxButton.MouseEnter += PictureBoxEventHandler.PictureBoxButtonMouseEnter;
                pbxButton.MouseDown += PictureBoxEventHandler.PictureBoxButtonMouseDown;
                pbxButton.MouseLeave += PictureBoxEventHandler.PictureBoxButtonMouseLeave;
                pbxButton.MouseUp += PictureBoxEventHandler.PictureBoxButtonMouseUp;
                pbxButton.EnabledChanged += PictureBoxEventHandler.PictureBoxButtonEnabledChanged;
            }

            foreach (var analyse in Enum.GetValues<AnalyseTypes>())
            {
                GetButtonByAnalyseType(analyse)
                    .EnabledChanged += PictureBoxEventHandler.PictureBoxEnabledChanged;

                GetLabelByAnalyseType(analyse)
                    .EnabledChanged += LabelEventHandler.LabelEnabledChanged;
            }

            ThothNotifyablePropertiesEntity.Default.PropertyChanged += SeriesList_PropertyChanged;
        }

        private void SeriesList_PropertyChanged(
                object? sender,
                PropertyChangedEventArgs e
            )
        {
            switch (e.PropertyName)
            {
                case nameof(ThothNotifyablePropertiesEntity.Default.SeriesDictionary):
                    FillExecutionLogs();
                    break;
                case nameof(ThothNotifyablePropertiesEntity.Default.GenerationProcessRunning):
                    if (ThothNotifyablePropertiesEntity.Default.GenerationProcessRunning
                        && !backgroundWorkerExecution.IsBusy)
                    {
                        backgroundWorkerExecution.RunWorkerAsync();
                    }
                    break;
                case nameof(ThothNotifyablePropertiesEntity.Default.UnifyPagesActive):
                case nameof(ThothNotifyablePropertiesEntity.Default.SplitPagesActive):
                case nameof(ThothNotifyablePropertiesEntity.Default.AdjustFilesActive):
                case nameof(ThothNotifyablePropertiesEntity.Default.GenerateCbzActive):
                    if (!ThothNotifyablePropertiesEntity.Default.GenerationProcessRunning
                        && !backgroundWorkerExecution.IsBusy)
                    {
                        FillExecutionLogs();
                    }
                    break;
                case nameof(ThothNotifyablePropertiesEntity.Default.AdjustableFilesSetted):
                    LabelEventHandler.UpdateStatisticsValues(lblConversionAnalytics);
                    FillExecutionLogs();
                    UpdateProgressExecution();
                    break;
                case nameof(ThothNotifyablePropertiesEntity.Default.SplittableFilesSetted):
                    LabelEventHandler.UpdateStatisticsValues(lblSplitAnalytics);
                    FillExecutionLogs();
                    UpdateProgressExecution();
                    break;
                case nameof(ThothNotifyablePropertiesEntity.Default.UnifyableFilesSetted):
                    LabelEventHandler.UpdateStatisticsValues(lblUnifyAnalytics);
                    FillExecutionLogs();
                    UpdateProgressExecution();
                    break;
                case nameof(ThothNotifyablePropertiesEntity.Default.FilesSetted):
                    LabelEventHandler.UpdateStatisticsValues(lblFilesAnalytics);
                    FillExecutionLogs();
                    UpdateProgressExecution();
                    break;
                case nameof(ThothNotifyablePropertiesEntity.Default.VolumesSetted):
                    LabelEventHandler.UpdateStatisticsValues(lblVolumesAnalytics);
                    FillExecutionLogs();
                    UpdateProgressExecution();
                    break;
                case nameof(ThothNotifyablePropertiesEntity.Default.SeriesSetted):
                    LabelEventHandler.UpdateStatisticsValues(lblSeriesAnalytics);
                    FillExecutionLogs();
                    UpdateProgressExecution();
                    break;
            }
        }

        private void UpdateProgressExecution()
        {
            Invoke(delegate
            {
                progressBarExecution.BeginInvoke(delegate
                {
                    progressBarExecution.Value = (
                                                        (ThothNotifyablePropertiesEntity.Default.FilesSetted.Sum(s => s) +
                                                        (ThothNotifyablePropertiesEntity.Default.GenerateCbzActive && !Settings.Default.DisableGbzGeneration ? ThothNotifyablePropertiesEntity.Default.SeriesSetted.Sum(s => s) : 0) +
                                                        (ThothNotifyablePropertiesEntity.Default.SplitPagesActive ? ThothNotifyablePropertiesEntity.Default.SplittableFilesSetted.Sum(s => s) : 0) +
                                                        (ThothNotifyablePropertiesEntity.Default.UnifyPagesActive ? ThothNotifyablePropertiesEntity.Default.UnifyableFilesSetted.Sum(s => s) : 0) +
                                                        (ThothNotifyablePropertiesEntity.Default.GenerateCbzActive && Settings.Default.DisableGbzGeneration ? ThothNotifyablePropertiesEntity.Default.VolumesSetted.Sum(s => s) : 0) +
                                                        (ThothNotifyablePropertiesEntity.Default.GenerateCbzActive && !Settings.Default.DisableGbzGeneration ? ThothNotifyablePropertiesEntity.Default.CbzFilesSetted.Sum(s => s) : 0))
                                                    );
                });
            });
        }
        #endregion GLOBAL ACTION METHODS
    }
}