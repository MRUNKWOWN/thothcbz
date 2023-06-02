using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Xml.Linq;

using ThothCbz.Constants;
using ThothCbz.Entities;
using ThothCbz.Enumerators;
using ThothCbz.Extensions;

using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ThothCbz.EventHandlers
{
    internal static class LabelEventHandler
    {
        internal static void LabelEnabledChanged(
                object? sender, 
                EventArgs e
            )
        {
            if (sender is null)
            {
                return;
            }

            UpdateStatisticsValues(((Label)sender));
        }
        
        internal static void UpdateStatisticsValues(
                Label lblStatistics
        )
        {
            lblStatistics.BeginInvoke(delegate 
                    {
                        var type = lblStatistics.GetAnalyseType();

                        lblStatistics.Text = !lblStatistics.Enabled
                                        ? type switch
                                        {
                                            AnalyseTypes.UnknownStatistics => "--",
                                            _ => GlobalConstants.DEFAULT_EMPTY_STATISTIC_VALUE
                                        }
                                        : type switch
                                        {
                                            AnalyseTypes.ConversionStatistics => $@"{ThothNotifyablePropertiesEntity.Default.AdjustableFilesSetted.Sum(s => s):N0} / {ThothNotifyablePropertiesEntity.Default.AdjustableFilesCount:N0}",
                                            AnalyseTypes.FilesStatistics => $@"{ThothNotifyablePropertiesEntity.Default.FilesSetted.Sum(s => s):N0} / {ThothNotifyablePropertiesEntity.Default.FilesCount:N0}",
                                            AnalyseTypes.SeriesStatistics => $@"{ThothNotifyablePropertiesEntity.Default.SeriesSetted.Sum(s => s):N0} / {ThothNotifyablePropertiesEntity.Default.SeriesCount:N0}",
                                            AnalyseTypes.SplitStatistics => $@"{ThothNotifyablePropertiesEntity.Default.SplittableFilesSetted.Sum(s => s):N0} / {ThothNotifyablePropertiesEntity.Default.SplittableFilesCount:N0}",
                                            AnalyseTypes.UnifyStatistics => $@"{ThothNotifyablePropertiesEntity.Default.UnifyableFilesSetted.Sum(s => s):N0} / {ThothNotifyablePropertiesEntity.Default.UnifyableFilesCount:N0}",
                                            AnalyseTypes.UnknownStatistics => $@"{ThothNotifyablePropertiesEntity.Default.UnknownFilesCount:N0}",
                                            AnalyseTypes.VolumesStatistics => $@"{ThothNotifyablePropertiesEntity.Default.VolumesSetted.Sum(s => s):N0} / {ThothNotifyablePropertiesEntity.Default.VolumesCount:N0}",
                                            _ => throw new NotImplementedException()
                                        }; 
                    });
        }
    }
}
