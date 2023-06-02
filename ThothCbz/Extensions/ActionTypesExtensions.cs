using ThothCbz.Entities;
using ThothCbz.Enumerators;
using ThothCbz.Properties;

namespace ThothCbz.Extensions
{
    internal static class ActionTypesExtensions
    {
        internal static bool GetActiveValue(
                this ActionTypes action
            )
        {
            return action switch
            {
                ActionTypes.Adjustments => ThothNotifyablePropertiesEntity.Default.AdjustFilesActive,
                ActionTypes.Unifications => ThothNotifyablePropertiesEntity.Default.UnifyPagesActive,
                ActionTypes.Splits => ThothNotifyablePropertiesEntity.Default.SplitPagesActive,
                ActionTypes.Cbzs => ThothNotifyablePropertiesEntity.Default.GenerateCbzActive,
                ActionTypes.Folder => false,
                ActionTypes.Help => false,
                ActionTypes.Warnings => false,
                ActionTypes.Refresh => false,
                ActionTypes.Play => false,
                ActionTypes.Cancel => false,
                _ => throw new NotImplementedException(),
            };
        }

        internal static Bitmap GetImageFromResource(
                this ActionTypes action,
                ImageFromResourceTypes resourceType
            )
        {
            return action switch
            {
                ActionTypes.Adjustments => resourceType switch
                {
                    ImageFromResourceTypes.Active => Resources.BtnAdjustmentsActive512x512,
                    ImageFromResourceTypes.Hover => Resources.BtnAdjustmentsHover512x512,
                    ImageFromResourceTypes.Inactive => Resources.BtnAdjustmentsInactive512x512,
                    ImageFromResourceTypes.Press => Resources.BtnAdjustmentsPressed512x512,
                    _ => throw new NotImplementedException()
                },
                ActionTypes.Unifications => resourceType switch
                {
                    ImageFromResourceTypes.Active => Resources.BtnUnifyActive512x512,
                    ImageFromResourceTypes.Hover => Resources.BtnUnifyHover512x512,
                    ImageFromResourceTypes.Inactive => Resources.BtnUnifyInactive512x512,
                    ImageFromResourceTypes.Press => Resources.BtnUnifyPressed512x512,
                    _ => throw new NotImplementedException()
                },
                ActionTypes.Splits => resourceType switch
                {
                    ImageFromResourceTypes.Active => Resources.BtnSplitActive512x512,
                    ImageFromResourceTypes.Hover => Resources.BtnSplitHover512x512,
                    ImageFromResourceTypes.Inactive => Resources.BtnSplitInactive512x512,
                    ImageFromResourceTypes.Press => Resources.BtnSplitPressed512x512,
                    _ => throw new NotImplementedException()
                },
                ActionTypes.Cbzs => resourceType switch
                {
                    ImageFromResourceTypes.Active => Resources.BtnCbzActive512x512,
                    ImageFromResourceTypes.Hover => Resources.BtnCbzHover512x512,
                    ImageFromResourceTypes.Inactive => Resources.BtnCbzInactive512x512,
                    ImageFromResourceTypes.Press => Resources.BtnCbzPressed512x512,
                    _ => throw new NotImplementedException()
                },
                ActionTypes.Folder => resourceType switch
                {
                    ImageFromResourceTypes.Active => Resources.BtnFolderHover512x512,
                    ImageFromResourceTypes.Hover => Resources.BtnFolderHover512x512,
                    ImageFromResourceTypes.Inactive => Resources.BtnFolderInactive512x512,
                    ImageFromResourceTypes.Press => Resources.BtnFolderPressed512x512,
                    _ => throw new NotImplementedException()
                },
                ActionTypes.Help => resourceType switch
                {
                    ImageFromResourceTypes.Active => Resources.BtnHelpHover512x512,
                    ImageFromResourceTypes.Hover => Resources.BtnHelpHover512x512,
                    ImageFromResourceTypes.Inactive => Resources.BtnHelpInactive512x512,
                    ImageFromResourceTypes.Press => Resources.BtnHelpPressed512x512,
                    _ => throw new NotImplementedException()
                },
                ActionTypes.Warnings => resourceType switch
                {
                    ImageFromResourceTypes.Active => Resources.BtnWarningsHover512x512,
                    ImageFromResourceTypes.Hover => Resources.BtnWarningsHover512x512,
                    ImageFromResourceTypes.Inactive => Resources.BtnWarningsInactive512x512,
                    ImageFromResourceTypes.Press => Resources.BtnWarningsPressed512x512,
                    _ => throw new NotImplementedException()
                },
                ActionTypes.Refresh => resourceType switch
                {
                    ImageFromResourceTypes.Active => Resources.BtnRefreshHover512x512,
                    ImageFromResourceTypes.Hover => Resources.BtnRefreshHover512x512,
                    ImageFromResourceTypes.Inactive => Resources.BtnRefreshInactive512x512,
                    ImageFromResourceTypes.Press => Resources.BtnRefreshPressed512x512,
                    _ => throw new NotImplementedException()
                },
                ActionTypes.Play => resourceType switch
                {
                    ImageFromResourceTypes.Active => Resources.BtnPlayHover512x512,
                    ImageFromResourceTypes.Hover => Resources.BtnPlayHover512x512,
                    ImageFromResourceTypes.Inactive => Resources.BtnPlayInactive512x512,
                    ImageFromResourceTypes.Press => Resources.BtnPlayPressed512x512,
                    _ => throw new NotImplementedException()
                },
                ActionTypes.Cancel => resourceType switch
                {
                    ImageFromResourceTypes.Active => Resources.BtnCancelHover512x512,
                    ImageFromResourceTypes.Hover => Resources.BtnCancelHover512x512,
                    ImageFromResourceTypes.Inactive => Resources.BtnCancelInactive512x512,
                    ImageFromResourceTypes.Press => Resources.BtnCancelPressed512x512,
                    _ => throw new NotImplementedException()
                },
                _ => throw new NotImplementedException(),
            }; ;
        }

        internal static ActionTypes GetActionType(
                this PictureBox pbxObject
            )
        {
            return pbxObject.Name switch
            {
                nameof(frmThotCbz.pbxBtnAdjustFiles) => ActionTypes.Adjustments,
                nameof(frmThotCbz.pbxBtnUnifyPages) => ActionTypes.Unifications,
                nameof(frmThotCbz.pbxBtnSplitPages) => ActionTypes.Splits,
                nameof(frmThotCbz.pbxBtnGenerateCbz) => ActionTypes.Cbzs,
                nameof(frmThotCbz.pbxBtnFolder) => ActionTypes.Folder,
                nameof(frmThotCbz.pbxBtnHelp) => ActionTypes.Help,
                nameof(frmThotCbz.pbxBtnWarnings) => ActionTypes.Warnings,
                nameof(frmThotCbz.pbxBtnRefresh) => ActionTypes.Refresh,
                nameof(frmThotCbz.pbxBtnExecute) => ActionTypes.Play,
                nameof(frmThotCbz.pbxBtnCancel) => ActionTypes.Cancel,
                _ => throw new NotImplementedException()
            };
        }
    }
}
