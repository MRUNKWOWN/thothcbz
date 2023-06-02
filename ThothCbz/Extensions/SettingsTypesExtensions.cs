using ThothCbz.Enumerators;

namespace ThothCbz.Extensions
{
    internal static class SettingsTypesExtensions
    {
        internal static string GetSettingsTypeLinkUri(
                this SettingsTypes? value
            )
        {
            if (!value.HasValue || value.Value == SettingsTypes.None)
            {
                return string.Empty;
            }

            return value.Value.ToString();
        }
    }
}
