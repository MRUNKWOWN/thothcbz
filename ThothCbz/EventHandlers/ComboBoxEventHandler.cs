using ThothCbz.Enumerators;
using ThothCbz.Properties;

namespace ThothCbz.EventHandlers
{
    internal static class ComboBoxEventHandler
    {
        internal static void CbbReadOrder_SelectedIndexChanged(
                object? sender,
                EventArgs e
            )
        {
            if (sender is not ComboBox comboBox || comboBox.SelectedItem is null)
            {
                return;
            }

            Settings.Default.ReadOrder = comboBox.SelectedItem.ToString() == Resources.LblPagesReadOrderLeftRightText
                                                    ? (int)ReadOrderTypes.LeftToRight
                                                    : (int)ReadOrderTypes.RightToLeft;

            Settings.Default.Save();
        }
    }
}
