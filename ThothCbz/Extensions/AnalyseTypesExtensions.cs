using ThothCbz.Enumerators;
using ThothCbz.Properties;

namespace ThothCbz.Extensions
{
    internal static class AnalyseTypesExtensions
    {
        internal static Bitmap GetImageFromResource(
                this AnalyseTypes action,
                ImageFromResourceTypes resourceType
            )
        {
            return action switch
            {
                AnalyseTypes.ConversionStatistics => resourceType switch
                {
                    ImageFromResourceTypes.Active => Resources.BtnTransformationEnable512x512,
                    ImageFromResourceTypes.Inactive => Resources.BtnTransformationDisable512x512,
                    _ => throw new NotImplementedException()
                },
                AnalyseTypes.FilesStatistics => resourceType switch
                {
                    ImageFromResourceTypes.Active => Resources.BtnFileEnable512x512,
                    ImageFromResourceTypes.Inactive => Resources.BtnFileDisable512x512,
                    _ => throw new NotImplementedException()
                },
                AnalyseTypes.SeriesStatistics => resourceType switch
                {
                    ImageFromResourceTypes.Active => Resources.BtnSeriesEnable512x512,
                    ImageFromResourceTypes.Inactive => Resources.BtnSeriesDisable512x512,
                    _ => throw new NotImplementedException()
                },
                AnalyseTypes.SplitStatistics => resourceType switch
                {
                    ImageFromResourceTypes.Active => Resources.BtnSplitStatisticsEnable,
                    ImageFromResourceTypes.Inactive => Resources.BtnSplitStatisticsDisable,
                    _ => throw new NotImplementedException()
                },
                AnalyseTypes.UnifyStatistics => resourceType switch
                {
                    ImageFromResourceTypes.Active => Resources.BtnUnifyStatisticsEnable,
                    ImageFromResourceTypes.Inactive => Resources.BtnUnifyStatisticsDisable,
                    _ => throw new NotImplementedException()
                },
                AnalyseTypes.UnknownStatistics => resourceType switch
                {
                    ImageFromResourceTypes.Active => Resources.BtnUnknownStatisticsEnable,
                    ImageFromResourceTypes.Inactive => Resources.BtnUnknownStatisticsDisable,
                    _ => throw new NotImplementedException()
                },
                AnalyseTypes.VolumesStatistics => resourceType switch
                {
                    ImageFromResourceTypes.Active => Resources.BtnVolumesEnable512x512,
                    ImageFromResourceTypes.Inactive => Resources.BtnVolumesDisable512x512,
                    _ => throw new NotImplementedException()
                },
                _ => throw new NotImplementedException(),
            }; ;
        }

        internal static AnalyseTypes GetAnalyseType(
                this PictureBox pbxObject
            )
        {
            return pbxObject.Name switch
            {
                nameof(frmThotCbz.pbxFilesStatistics) => AnalyseTypes.FilesStatistics,
                nameof(frmThotCbz.pbxVolumesStatistics) => AnalyseTypes.VolumesStatistics,
                nameof(frmThotCbz.pbxSeriesStatistics) => AnalyseTypes.SeriesStatistics,
                nameof(frmThotCbz.pbxConversionStatistics) => AnalyseTypes.ConversionStatistics,
                nameof(frmThotCbz.pbxSplitStatistics) => AnalyseTypes.SplitStatistics,
                nameof(frmThotCbz.pbxUnifyStatistics) => AnalyseTypes.UnifyStatistics,
                nameof(frmThotCbz.pbxUnknownStatistics) => AnalyseTypes.UnknownStatistics,
                _ => throw new NotImplementedException()
            };
        }

        internal static AnalyseTypes GetAnalyseType(
                this Label lblObject
            )
        {
            return lblObject.Name switch
            {
                nameof(frmThotCbz.lblFilesAnalytics) => AnalyseTypes.FilesStatistics,
                nameof(frmThotCbz.lblVolumesAnalytics) => AnalyseTypes.VolumesStatistics,
                nameof(frmThotCbz.lblSeriesAnalytics) => AnalyseTypes.SeriesStatistics,
                nameof(frmThotCbz.lblConversionAnalytics) => AnalyseTypes.ConversionStatistics,
                nameof(frmThotCbz.lblSplitAnalytics) => AnalyseTypes.SplitStatistics,
                nameof(frmThotCbz.lblUnifyAnalytics) => AnalyseTypes.UnifyStatistics,
                nameof(frmThotCbz.lblUnknwonAnalytics) => AnalyseTypes.UnknownStatistics,
                _ => throw new NotImplementedException()
            };
        }
    }
}
