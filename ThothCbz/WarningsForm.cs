using ThothCbz.Constants;
using ThothCbz.Entities;
using ThothCbz.Properties;

namespace ThothCbz
{
    public partial class WarningsForm : Form
    {
        public WarningsForm()
        {
            InitializeComponent();

            rtbWarnings.BackColor = GlobalConstants.DEFAULT_BACKGROUND_COLOR;
            rtbWarnings.ForeColor = GlobalConstants.DEFAULT_ENABLED_TEXT_COLOR;

            GenerateWarningInfos();
        }

        private void GenerateWarningInfos()
        {
            var lines = new List<string>();

            var cbzWarnings = ThothNotifyablePropertiesEntity.Default.ExistAdjustableFiles && 
                                !ThothNotifyablePropertiesEntity.Default.AdjustFilesActive;

            var splitWarnings = ThothNotifyablePropertiesEntity.Default.SplittableFilesNeedingConversionCount > 0 && !ThothNotifyablePropertiesEntity.Default.AdjustFilesActive;
            var unifyWarnings = ThothNotifyablePropertiesEntity.Default.UnifyableFilesNeedingConversionCount > 0 && !ThothNotifyablePropertiesEntity.Default.AdjustFilesActive;

            lines.Add(ThothNotifyablePropertiesEntity.Default.ExistUnknownFiles && cbzWarnings
                        ? Resources.RtbWarningsHeaderMultiplesText
                        : Resources.RtbWarningsHeaderOneText);

            lines.Add(string.Empty);

            if (ThothNotifyablePropertiesEntity.Default.ExistUnknownFiles)
            {
                lines.Add(Resources.RtbWarningsForAdjustments001Text);
                lines.Add(Resources.RtbWarningsForAdjustments002Text);
                lines.Add(Resources.RtbWarningsForAdjustments003Text);
                lines.Add(string.Empty);
            }

            if (unifyWarnings)
            {
                lines.Add(Resources.RtbWarningsForUnify001Text);
                lines.Add(Resources.RtbWarningsForUnify002Text);
                lines.Add(string.Empty);
            }

            if(ThothNotifyablePropertiesEntity.Default.ExistOddUnifyablePagesFiles)
            {
                lines.Add(Resources.RtbWarningsForUnify003Text);
                lines.Add(Resources.RtbWarningsForUnify004Text);
                lines.Add(Resources.RtbWarningsForUnify005Text);
                lines.Add(string.Empty);
            }

            if (splitWarnings)
            {
                lines.Add(Resources.RtbWarningsForSplit001Text);
                lines.Add(Resources.RtbWarningsForSplit002Text);
                lines.Add(string.Empty);
            }

            if (cbzWarnings)
            {
                lines.Add(Resources.RtbWarningsForCbz001Text);
                lines.Add(Resources.RtbWarningsForCbz002Text);
                lines.Add(Resources.RtbWarningsForCbz003Text);
            }

            rtbWarnings.Text = string.Join(Environment.NewLine, lines);
        }
    }
}
