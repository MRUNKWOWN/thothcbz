using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Text;

using ThothCbz.Actions;
using ThothCbz.Constants;
using ThothCbz.Entities;
using ThothCbz.Enumerators;
using ThothCbz.EventHandlers;
using ThothCbz.Extensions;
using ThothCbz.Properties;
using static ThothCbz.Extensions.StringExtensions;

namespace ThothCbz
{
    public partial class frmThotCbz : Form
    {
        private string _rtfExecutionLogsTextColorsConfigurationLine = string.Empty;
        private StringBuilder _rtfExecutionLogsTextFontsConfigurationline = new StringBuilder();

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
            //rtbExecutionLogs.BackColor = GlobalConstants.DEFAULT_LOG_BACKGROUND_COLOR;
            treeViewVolumes.BackColor = GlobalConstants.DEFAULT_BACKGROUND_COLOR;
            txtUnifyableDirectory.BackColor = GlobalConstants.DEFAULT_LOG_BACKGROUND_COLOR;
            txtSplitableDirectory.BackColor = GlobalConstants.DEFAULT_LOG_BACKGROUND_COLOR;
            nudMinimalHeight.BackColor = GlobalConstants.DEFAULT_LOG_BACKGROUND_COLOR;
            cbbReadOrder.BackColor = GlobalConstants.DEFAULT_LOG_BACKGROUND_COLOR;

            cbxUseSelectedDirectory.ForeColor = GlobalConstants.DEFAULT_ENABLED_TEXT_COLOR;
            cbxUpscaleImages.ForeColor = GlobalConstants.DEFAULT_ENABLED_TEXT_COLOR;
            cbxBlankSpace.ForeColor = GlobalConstants.DEFAULT_ENABLED_TEXT_COLOR;
            cbxEnableBrightnessContrast.ForeColor = GlobalConstants.DEFAULT_ENABLED_TEXT_COLOR;
            toolStripStatusLabelMainArea.ForeColor = GlobalConstants.DEFAULT_ENABLED_TEXT_COLOR;
            //rtbExecutionLogs.ForeColor = GlobalConstants.DEFAULT_ENABLED_TEXT_COLOR;
            treeViewVolumes.ForeColor = GlobalConstants.DEFAULT_ENABLED_TEXT_COLOR;
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

            Dictionary<int, string> _rtfExecutionLogsTextFontsConfigurations = new Dictionary<int, string>();
            _rtfExecutionLogsTextFontsConfigurations.Add(SupportedLanguageType.Arabic.GetStringRtfFontIndex(), $@"{{\f{SupportedLanguageType.Arabic.GetStringRtfFontIndex()}\fnil\fcharset178 Segoe UI;}}");
            _rtfExecutionLogsTextFontsConfigurations.Add(SupportedLanguageType.BrazilianPortuguese.GetStringRtfFontIndex(), $@"{{\f{SupportedLanguageType.BrazilianPortuguese.GetStringRtfFontIndex()}\fnil\fcharset0 Segoe UI;}}");
            _rtfExecutionLogsTextFontsConfigurations.Add(SupportedLanguageType.Chinese.GetStringRtfFontIndex(), $@"{{\f{SupportedLanguageType.Chinese.GetStringRtfFontIndex()}\fswiss\fcharset134 Microsoft YaHei;}}");
            _rtfExecutionLogsTextFontsConfigurations.Add(SupportedLanguageType.English.GetStringRtfFontIndex(), $@"{{\f{SupportedLanguageType.English.GetStringRtfFontIndex()}\fnil Segoe UI;}}");
            _rtfExecutionLogsTextFontsConfigurations.Add(SupportedLanguageType.Japanese.GetStringRtfFontIndex(), $@"{{\f{SupportedLanguageType.Japanese.GetStringRtfFontIndex()}\fnil\fcharset128 Yu Gothic;}}");

            _rtfExecutionLogsTextFontsConfigurationline.Append("{\\fonttbl");

            foreach (var key in _rtfExecutionLogsTextFontsConfigurations.Keys.Order())
            {
                _rtfExecutionLogsTextFontsConfigurationline.Append(_rtfExecutionLogsTextFontsConfigurations[key]);
            }

            _rtfExecutionLogsTextFontsConfigurationline.Append("}");

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
            treeViewVolumes.Width = pnlMain.Width;
            treeViewVolumes.Height = pnlMain.Height - pnlExecutionHeader.Height;
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
                UpdateTreeViewStatus();

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
                var generationResults = new ConcurrentDictionary<string, int>();

                foreach(var volume in ThothNotifyablePropertiesEntity.Default.SeriesDictionary.AllFilesGroupedByVolume())
                {
                    if (ThothNotifyablePropertiesEntity.Default.CancelGenerationProcessQueued)
                    {
                        return;
                    }

                    var seriesKey = volume.FirstOrDefault()!.Serie;
                    var filesToGrayscale = new List<string>();

                    var filesToGrayscalePath = ThothNotifyablePropertiesEntity.Default.SeriesDictionary[seriesKey].FilesToGrayScaleFilePath();

                    if (!string.IsNullOrWhiteSpace(filesToGrayscalePath) && File.Exists(filesToGrayscalePath))
                    {
                        filesToGrayscale = File.ReadAllLines(filesToGrayscalePath).ToList();
                    }

                    var customBlankFilePath = Directory.GetFiles(ThothNotifyablePropertiesEntity.Default.SeriesDictionary[seriesKey].First().SeriePath.Replace('|', '\\'), $"{GlobalConstants.DEFAULT_BLANK_FILE_NAME}{Settings.Default.ImageOutputFileType.GetImageOutputFileTypeExtension()}").FirstOrDefault();
                    var defaultFileToSize = Directory.GetFiles(ThothNotifyablePropertiesEntity.Default.SeriesDictionary[seriesKey].First().SeriePath.Replace('|', '\\'), $"{GlobalConstants.DEFAULT_TEMPLATE_FILE_NAME}{Settings.Default.ImageOutputFileType.GetImageOutputFileTypeExtension()}").FirstOrDefault();

                    VolumeGenerations(
                            volume,
                            filesToGrayscale,
                            customBlankFilePath,
                            defaultFileToSize
                        );

                    generationResults.AddOrUpdate(seriesKey, 1, (key, oldValue) => oldValue + 1);

                    if (generationResults[seriesKey] == ThothNotifyablePropertiesEntity.Default.SeriesDictionary[seriesKey].Count)
                    {

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
                    }
                };
            }
            catch (Exception ex)
            {
                ex.InformAndSaveLog();

                Invoke(delegate
                {
                    ThothNotifyablePropertiesEntity.Default.GenerationProcessRunning = false;
                    ThothNotifyablePropertiesEntity.Default.CancelGenerationProcessQueued = false;

                    UpdateTreeViewStatus();
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

            var parallelOptions = new ParallelOptions()
            {
                MaxDegreeOfParallelism = Settings.Default.MaxDegreeOfParallelism
            };

            if (volume.Count > 1)
            {
                string volumePath = volume.FirstOrDefault()!.SeriePath!.Replace('|', '\\') +
                                        (!string.IsNullOrWhiteSpace(volume.FirstOrDefault()!.Volume) ? $"\\{volume.FirstOrDefault()!.Volume}" : string.Empty);

                var volumeDefaultFileToSize = Directory.GetFiles(volumePath, $"{GlobalConstants.DEFAULT_TEMPLATE_FILE_NAME}{Settings.Default.ImageOutputFileType.GetImageOutputFileTypeExtension()}").FirstOrDefault();

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
                                                            , $"{GlobalConstants.DEFAULT_TEMPLATE_FILE_NAME}{Settings.Default.ImageOutputFileType.GetImageOutputFileTypeExtension()}"
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

                        Parallel.ForEach(chapter.Where(w => !w.FileWasAdjusted).ToList().OrderBy(o => o.FilePath), parallelOptions, file =>
                        {
                            Adjustments.ModifyAndSave(
                                fileEntity: file,
                                filesToGrayscale: filesToGrayscale,
                                defaultSize: defaultSize
                            );
                        });

                        if (!string.IsNullOrWhiteSpace(chapterDefaultFileToSize) && File.Exists(chapterDefaultFileToSize))
                        {
                            File.Delete(chapterDefaultFileToSize);
                        }
                    }

                    if (ThothNotifyablePropertiesEntity.Default.SplitPagesActive)
                    {
                        Parallel.ForEach(chapter.Where(w => w.IsSplit && !w.FileWasSplit).ToList().OrderBy(o => o.FilePath), parallelOptions, file =>
                        {
                            Split.ModifyAndSave(
                                fileEntity: file
                            );
                        });
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
                UpdateTreeViewStatus();
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

        private void FillTreeView()
        {
            var treeViewImgList = new ImageList();
            treeViewImgList.Images.Add(Resources.TreeViewStatusDone);
            treeViewImgList.Images.Add(Resources.TreeViewStatusError);
            treeViewImgList.Images.Add(Resources.TreeViewStatusQueue);
            treeViewImgList.Images.Add(Resources.TreeViewStatusRunning);
            treeViewImgList.Images.Add(Resources.TreeViewStatusWaiting);
            treeViewImgList.Images.Add(Resources.TreeViewStatusWarning);

            treeViewVolumes.ImageList = treeViewImgList;

            treeViewVolumes.BeginInvoke(delegate
            {
                if (!ThothNotifyablePropertiesEntity.Default.SeriesDictionary.Any())
                {
                    treeViewVolumes.Nodes.Clear();
                    return;
                }

                if (treeViewVolumes.Nodes.Count > 0)
                    return;

                foreach (var serie in ThothNotifyablePropertiesEntity.Default.SeriesDictionary.Keys.OrderBy(o => o))
                {
                    var serieNode = new TreeNode($"{serie.ToUpper()} [ {ThothNotifyablePropertiesEntity.Default.SeriesDictionary[serie].Count} ]", 4, 4)
                    {
                        Tag = serie
                    };

                    var items = ThothNotifyablePropertiesEntity.Default.SeriesDictionary[serie];
                    var seriesPath = items.First().SeriePath;

                    items.Where(w => !string.IsNullOrWhiteSpace(w?.Volume))
                            .GroupBy(g2 => g2.Volume)
                            .OrderBy(o2 => o2.Key)
                            .ToList()
                            .ForEach(f2 =>
                            {
                                var volumeNode = new TreeNode($"{f2.Key!.ToUpper()} [ {f2.Count()} ]", 4, 4)
                                {
                                    Tag = $@"{seriesPath}|{f2.Key}"
                                };

                                f2.Where(w2 => !string.IsNullOrWhiteSpace(w2?.Chapter))
                                    .GroupBy(g3 => g3.Chapter)
                                    .OrderBy(o3 => o3.Key)
                                    .ToList()
                                    .ForEach(f3 =>
                                    {
                                        var chapterNode = new TreeNode($"{f3.Key!.ToUpper()} [ {f3.Count()} ]", 4, 4)
                                        {
                                            Tag = $@"{seriesPath}|{f2.Key}|{f3.Key}"
                                        };

                                        volumeNode.Nodes.Add(chapterNode);
                                    });

                                serieNode.Nodes.Add(volumeNode);
                            });

                    treeViewVolumes.Nodes.Add(serieNode);
                }
            });
        }

        private void UpdateTreeViewStatus()
        {

            var bagLines = new ConcurrentBag<(string, List<string>)>();

            Parallel.ForEach(ThothNotifyablePropertiesEntity.Default.SeriesDictionary.Keys, serie =>
            {
                var lines = new List<string>();
                var items = ThothNotifyablePropertiesEntity.Default.SeriesDictionary[serie];

                var seriesPath = items.First().SeriePath;
                var seriesStatus = GetExecutionStatusByLevel(items: items);
                var waitingEntitiesAmountSerie = GetWaitingEntitiesAmountByLevel(items: items);

                treeViewVolumes.BeginInvoke( delegate{ 
                    foreach (TreeNode node in treeViewVolumes.Nodes)
                    {
                        if (node.Tag!.ToString() == serie)
                        {
                            UpdateTreeNodeStatus(
                                node: node, 
                                status: seriesStatus,
                                waitingEntitiesAmount: waitingEntitiesAmountSerie);

                            items.Where(w => !string.IsNullOrWhiteSpace(w?.Volume))
                                .GroupBy(g2 => g2.Volume)
                                .OrderBy(o2 => o2.Key)
                                .ToList()
                                .ForEach(f2 =>
                                {
                                    var volumeItems = items.Where(w => w.Volume == f2.Key).ToList();

                                    var volumeStatus = GetExecutionStatusByLevel(
                                                    items: volumeItems,
                                                    volumeName: f2.Key
                                                );

                                    var waitingEntitiesAmountVolume = GetWaitingEntitiesAmountByLevel(
                                        items: volumeItems,
                                        volumeName: f2.Key
                                    );

                                    foreach (TreeNode volumeNode in node.Nodes)
                                    {
                                        if (volumeNode.Tag!.ToString() == $@"{f2.First().SeriePath}|{f2.Key}")
                                        {
                                            UpdateTreeNodeStatus(
                                                node: volumeNode,
                                                status: volumeStatus,
                                                waitingEntitiesAmount: waitingEntitiesAmountVolume);

                                            f2.Where(w2 => !string.IsNullOrWhiteSpace(w2?.Chapter))
                                                .GroupBy(g3 => g3.Chapter)
                                                .OrderBy(o3 => o3.Key)
                                                .ToList()
                                                .ForEach(f3 =>
                                                {
                                                    var chapterItems = volumeItems.Where(w => w.Chapter == f3.Key).ToList();

                                                    var chapterStatus = GetExecutionStatusByLevel(
                                                            items: chapterItems,
                                                            volumeName: f2.Key,
                                                            chapterName: f3.Key
                                                        );

                                                    var waitingEntitiesAmountChapter = GetWaitingEntitiesAmountByLevel(
                                                            items: chapterItems,
                                                            volumeName: f2.Key,
                                                            chapterName: f3.Key
                                                        );

                                                    foreach (TreeNode chapterNode in volumeNode.Nodes)
                                                    {
                                                        if (chapterNode.Tag!.ToString() == $@"{f2.First().SeriePath}|{f2.Key}|{f3.Key}")
                                                        {
                                                            UpdateTreeNodeStatus(
                                                                node: chapterNode, 
                                                                status: chapterStatus,
                                                                waitingEntitiesAmount: waitingEntitiesAmountChapter);
                                                        }
                                                    }
                                                });
                                        }
                                    };
                                });
                        }
                    }
                });
            });
        }

        private void UpdateTreeNodeStatus(
                TreeNode node,
                ExecutionStatusType status,
                int waitingEntitiesAmount = 0
            )
        {
            switch (status)
            {
                case ExecutionStatusType.Done:
                    node.ImageIndex = 0;
                    node.SelectedImageIndex = 0;
                    node.Collapse();
                    break;
                case ExecutionStatusType.Error:
                    node.ImageIndex = 1;
                    node.SelectedImageIndex = 1;
                    node.Expand();
                    break;
                case ExecutionStatusType.Queued:
                    node.ImageIndex = 2;
                    node.SelectedImageIndex = 2;
                    node.Collapse();
                    break;
                case ExecutionStatusType.Running:
                    node.ImageIndex = 3;
                    node.SelectedImageIndex = 3;
                    node.Expand();
                    break;
                case ExecutionStatusType.NotRunning:
                    node.ImageIndex = 4;
                    node.SelectedImageIndex = 4;
                    node.Collapse();
                    break;
                case ExecutionStatusType.Warning:
                    node.ImageIndex = 5;
                    node.SelectedImageIndex = 5;
                    node.Expand();
                    break;
            }

            var amountText = waitingEntitiesAmount > 0 ? $" [ {waitingEntitiesAmount} ]" : string.Empty;
            node.Text = $"{node.Text.Split('[')[0].TrimEnd()}{amountText}";
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

        private int GetWaitingEntitiesAmountByLevel(
                List<FileEntity> items,
                string? volumeName = null,
                string? chapterName = null
            )
        {
            if (!items.Any())
                return 0;

            var result = items
                            .Where(w => (string.IsNullOrWhiteSpace(volumeName) || w.Volume == volumeName)
                                    && (string.IsNullOrWhiteSpace(chapterName) || w.Chapter == chapterName))
                            .ToList();

            var count = result.Where(w => (!ThothNotifyablePropertiesEntity.Default.AdjustFilesActive || w.FileWasAdjusted) &&
                                        (!ThothNotifyablePropertiesEntity.Default.SplitPagesActive || w.FileWasSplit) &&
                                        (!ThothNotifyablePropertiesEntity.Default.UnifyPagesActive || w.FileWasUnified)).
                                    Count();

            return items.Count - count;
        }
        #endregion BUTTON ACTION METHODS

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
                    FillTreeView();
                    UpdateTreeViewStatus();
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
                        UpdateTreeViewStatus();
                    }
                    break;
                case nameof(ThothNotifyablePropertiesEntity.Default.AdjustableFilesSetted):
                    LabelEventHandler.UpdateStatisticsValues(lblConversionAnalytics);
                    UpdateTreeViewStatus();
                    UpdateProgressExecution();
                    break;
                case nameof(ThothNotifyablePropertiesEntity.Default.SplittableFilesSetted):
                    LabelEventHandler.UpdateStatisticsValues(lblSplitAnalytics);
                    UpdateTreeViewStatus();
                    UpdateProgressExecution();
                    break;
                case nameof(ThothNotifyablePropertiesEntity.Default.UnifyableFilesSetted):
                    LabelEventHandler.UpdateStatisticsValues(lblUnifyAnalytics);
                    UpdateTreeViewStatus();
                    UpdateProgressExecution();
                    break;
                case nameof(ThothNotifyablePropertiesEntity.Default.FilesSetted):
                    LabelEventHandler.UpdateStatisticsValues(lblFilesAnalytics);
                    UpdateTreeViewStatus();
                    UpdateProgressExecution();
                    break;
                case nameof(ThothNotifyablePropertiesEntity.Default.VolumesSetted):
                    LabelEventHandler.UpdateStatisticsValues(lblVolumesAnalytics);
                    UpdateTreeViewStatus();
                    UpdateProgressExecution();
                    break;
                case nameof(ThothNotifyablePropertiesEntity.Default.SeriesSetted):
                    LabelEventHandler.UpdateStatisticsValues(lblSeriesAnalytics);
                    UpdateTreeViewStatus();
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