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
            Settings.Default.ReadOrder = ((ComboBox)sender!).SelectedItem.ToString() == Resources.LblPagesReadOrderLeftRightText
                                                    ? (int)ReadOrderTypes.LeftToRight
                                                    : (int)ReadOrderTypes.RightToLeft;

            Settings.Default.Save();
        }
    }
}
