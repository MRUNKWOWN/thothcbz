using ThothCbz.Enumerators;

namespace ThothCbz.Extensions
{
    internal static class LinkClickedEventArgsExtensions
    {
        internal static SettingsTypes GetPreAnalysisTypePerLinkAction(
                this LinkClickedEventArgs? value
            )
        {
            if (string.IsNullOrWhiteSpace(value?.LinkText))
            {
                return SettingsTypes.None;
            }

            return value.LinkText!.GetPreAnalysisTypesFromLinkAction();
        }
    }
}
