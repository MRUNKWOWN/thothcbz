using ThothCbz.Enumerators;

namespace ThothCbz.Extensions
{
    internal static class StringExtensions
    {
        internal static SettingsTypes GetPreAnalysisTypesFromLinkAction(
                this string? value
            )
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return SettingsTypes.None;
            }

            return Enum.TryParse(typeof(SettingsTypes), value, out var newValue)
                ? (SettingsTypes)newValue
                : SettingsTypes.None;
        }
    }
}
