using ThothCbz.Entities;
using ThothCbz.Enumerators;
using ThothCbz.Extensions;
using ThothCbz.Properties;

namespace ThothCbz.EventHandlers
{
    internal static class PictureBoxEventHandler
    {
        internal static void PictureBoxButtonMouseEnter(
                object? sender,
                EventArgs e
            )
        {
            if (sender is null)
            {
                return;
            }

            pbxMouseEventAction(
                    ((PictureBox)sender),
                    MouseEventTypes.Enter
                );
        }

        internal static void PictureBoxButtonMouseLeave(
                object? sender,
                EventArgs e
            )
        {
            if (sender is null)
            {
                return;
            }

            pbxMouseEventAction(
                    ((PictureBox)sender),
                    MouseEventTypes.Leaves
                );
        }

        internal static void PictureBoxButtonMouseDown(
                object? sender,
                MouseEventArgs e
            )
        {
            if (sender is null)
            {
                return;
            }

            pbxMouseEventAction(
                    ((PictureBox)sender),
                    MouseEventTypes.Press
                );
        }

        internal static void PictureBoxButtonMouseUp(
                object? sender,
                MouseEventArgs e
            )
        {
            if (sender is null)
            {
                return;
            }

            pbxMouseEventAction(
                    ((PictureBox)sender),
                    MouseEventTypes.Up
                );
        }

        internal static void PictureBoxButtonEnabledChanged(
                object? sender,
                EventArgs e
            )
        {
            if (sender is null)
            {
                return;
            }

            pbxMouseEventAction(
                    ((PictureBox)sender),
                    MouseEventTypes.EnableChanged
                );
        }

        internal static void PictureBoxButtonClick(
                object? sender,
                EventArgs e
            )
        {
            if (sender is null)
            {
                return;
            }

            pbxClick(
                    ((PictureBox)sender).GetActionType()
                );
        }

        internal static void PictureBoxEnabledChanged(
                object? sender,
                EventArgs e
            )
        {
            if (sender is null)
            {
                return;
            }

            ((PictureBox)sender).Image = ((PictureBox)sender)
                                            .GetAnalyseType()
                                            .GetImageFromResource(
                                                    ((PictureBox)sender).Enabled
                                                        ? ImageFromResourceTypes.Active
                                                        : ImageFromResourceTypes.Inactive
                                                );
        }

        private static void pbxMouseEventAction(
                PictureBox sender,
                MouseEventTypes mouseEvent
            )
        {
            var action = sender.GetActionType();

            switch (mouseEvent)
            {
                case MouseEventTypes.EnableChanged:
                    sender.Cursor = sender.Enabled
                                    ? Cursors.Hand
                                    : Cursors.Default;

                    sender.Image = action.GetImageFromResource(
                                            sender.Enabled && action.GetActiveValue()
                                                ? ImageFromResourceTypes.Active
                                                : ImageFromResourceTypes.Inactive
                                        );
                    break;
                default:
                    if (!sender.Enabled)
                    {
                        return;
                    }

                    sender.Image = action.GetImageFromResource(
                                            mouseEvent == MouseEventTypes.Press
                                                        ? ImageFromResourceTypes.Press
                                                        : mouseEvent == MouseEventTypes.Leaves || mouseEvent == MouseEventTypes.Up
                                                            ? action.GetActiveValue()
                                                                ? ImageFromResourceTypes.Active
                                                                : ImageFromResourceTypes.Inactive
                                                            : action.GetActiveValue()
                                                                ? ImageFromResourceTypes.Active
                                                                : ImageFromResourceTypes.Hover
                                        );
                    break;
            }
        }

        private static void pbxClick(
                ActionTypes action
            )
        {
            switch (action)
            {
                case ActionTypes.Adjustments:
                    ThothNotifyablePropertiesEntity.Default.AdjustFilesActive = !ThothNotifyablePropertiesEntity.Default.AdjustFilesActive;
                    break;
                case ActionTypes.Unifications:
                    ThothNotifyablePropertiesEntity.Default.UnifyPagesActive = !ThothNotifyablePropertiesEntity.Default.UnifyPagesActive;
                    break;
                case ActionTypes.Splits:
                    ThothNotifyablePropertiesEntity.Default.SplitPagesActive = !ThothNotifyablePropertiesEntity.Default.SplitPagesActive;
                    break;
                case ActionTypes.Cbzs:
                    ThothNotifyablePropertiesEntity.Default.GenerateCbzActive = !ThothNotifyablePropertiesEntity.Default.GenerateCbzActive;
                    break;
                case ActionTypes.Help:
                    //_btnGenerateCbzActive = enabled && !_btnGenerateCbzActive;
                    break;
                case ActionTypes.Warnings:
                    var warningsForm = new WarningsForm();
                    warningsForm.Text = Resources.FormWarningsText;
                    warningsForm.ShowDialog();
                    break;
                case ActionTypes.Refresh:
                    GetAndAnalyzeFiles();
                    break;
                case ActionTypes.Folder:
                    var folderDialog = new FolderBrowserDialog();
                    DialogResult result = folderDialog.ShowDialog();

                    if (result != DialogResult.OK)
                    {
                        return;
                    }

                    ThothNotifyablePropertiesEntity.Default.DirectoryPathToAnalyze = folderDialog.SelectedPath;
                    GetAndAnalyzeFiles();
                    break;
                case ActionTypes.Play:
                    Settings.Default.Save();
                    ThothNotifyablePropertiesEntity.Default.KeepUserChoicesBetweenFileAnalyses = true;
                    GetAndAnalyzeFiles();
                    ThothNotifyablePropertiesEntity.Default.KeepUserChoicesBetweenFileAnalyses = false;
                    ThothNotifyablePropertiesEntity.Default.CancelGenerationProcessQueued = false;
                    ThothNotifyablePropertiesEntity.Default.GenerationProcessRunning = true;
                    break;
                case ActionTypes.Cancel:
                    ThothNotifyablePropertiesEntity.Default.CancelGenerationProcessQueued = true;
                    break;
            }
        }

        private static void GetAndAnalyzeFiles()
        {
            ThothNotifyablePropertiesEntity.Default.Reset();

            var analysisForm = new AnalyticsForm(
                                            ThothNotifyablePropertiesEntity.Default.DirectoryPathToAnalyze
                                        );

            analysisForm.Text = Resources.FormAnalysisText;
            DialogResult analysisResult = analysisForm.ShowDialog();

            ThothNotifyablePropertiesEntity.Default.AdjustFilesActive = ((ThothNotifyablePropertiesEntity.Default.AdjustFilesActive && !ThothNotifyablePropertiesEntity.Default.ExistFiles))
                                                                            ? ThothNotifyablePropertiesEntity.Default.ExistFiles
                                                                            : !ThothNotifyablePropertiesEntity.Default.KeepUserChoicesBetweenFileAnalyses
                                                                                ? ThothNotifyablePropertiesEntity.Default.ExistFiles
                                                                                : ThothNotifyablePropertiesEntity.Default.AdjustFilesActive;

            ThothNotifyablePropertiesEntity.Default.SplitPagesActive = ((ThothNotifyablePropertiesEntity.Default.SplitPagesActive && !ThothNotifyablePropertiesEntity.Default.ExistSplittableFiles))
                                                                            ? ThothNotifyablePropertiesEntity.Default.ExistSplittableFiles
                                                                            : !ThothNotifyablePropertiesEntity.Default.KeepUserChoicesBetweenFileAnalyses
                                                                                ? ThothNotifyablePropertiesEntity.Default.ExistSplittableFiles
                                                                                : ThothNotifyablePropertiesEntity.Default.SplitPagesActive;

            ThothNotifyablePropertiesEntity.Default.UnifyPagesActive = ((ThothNotifyablePropertiesEntity.Default.UnifyPagesActive && !ThothNotifyablePropertiesEntity.Default.ExistUnifyableFiles))
                                                                            ? ThothNotifyablePropertiesEntity.Default.ExistUnifyableFiles
                                                                            : !ThothNotifyablePropertiesEntity.Default.KeepUserChoicesBetweenFileAnalyses
                                                                                ? ThothNotifyablePropertiesEntity.Default.ExistUnifyableFiles
                                                                                : ThothNotifyablePropertiesEntity.Default.UnifyPagesActive;

            ThothNotifyablePropertiesEntity.Default.GenerateCbzActive = ((ThothNotifyablePropertiesEntity.Default.GenerateCbzActive && !ThothNotifyablePropertiesEntity.Default.ExistFiles))
                                                                            ? ThothNotifyablePropertiesEntity.Default.ExistFiles
                                                                            : !ThothNotifyablePropertiesEntity.Default.KeepUserChoicesBetweenFileAnalyses
                                                                                ? ThothNotifyablePropertiesEntity.Default.ExistFiles
                                                                                : ThothNotifyablePropertiesEntity.Default.GenerateCbzActive;

            ThothNotifyablePropertiesEntity.Default.AnalysisExecuted = true;
        }
    }
}
