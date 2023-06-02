using ThothCbz.Properties;

namespace ThothCbz.EventHandlers
{
    internal static class CheckBoxEventHandler
    {
        internal static void CbxUpscaleImages_CheckedChanged(
                object? sender,
                EventArgs e
            )
        {
            Settings.Default.EnableUpscale = ((CheckBox)sender!).Checked;
            Settings.Default.Save();
        }

        internal static void CbxUseSelectedDirectory_CheckedChanged(
                object? sender,
                EventArgs e
            )
        {
            Settings.Default.UseSelectedFolderAsPartOfTheFileStructure = ((CheckBox)sender!).Checked;
            Settings.Default.Save();
        }
    }
}
