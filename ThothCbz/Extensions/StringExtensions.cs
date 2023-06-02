using ThothCbz.Enumerators;

namespace ThothCbz.Extensions
{
    internal static class StringExtensions
    {
        private const string _defaultReplacableTextToLinkName = "_LINK_";
        private const string _defaultReplacableTextToLinkUri = "_URI_";
        private const string _defaultReplacableTextToName = "_NAME_";
        private const string _defaultReplacableTextToStatus = "_STATUS_";
        private const string _defaultReplacableTextToColor = "_COLOR_";

        internal static string ReplaceRtfUri(
                this string? value, 
                SettingsTypes? preAnalysisTypes
            )
        {
            if(string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            return value.Replace(_defaultReplacableTextToLinkUri, preAnalysisTypes.GetSettingsTypeLinkUri());
        }

        internal static string ReplaceRtfUri(
                this string? value,
                string uri
            )
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            return value.Replace(_defaultReplacableTextToLinkUri, uri);
        }

        internal static string ReplaceRtfLinkName(
                this string? value, 
                string linkName
            )
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            if(string.IsNullOrWhiteSpace(linkName))
            {
                throw new ArgumentNullException(nameof(linkName));
            }

            return value.Replace(_defaultReplacableTextToLinkName, linkName);
        }

        internal static string ReplaceRtfName(
                this string? value, 
                string name
            )
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            if(string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            return value.Replace(_defaultReplacableTextToName, name);
        }

        internal static string ReplaceRtfStatus(
                this string? value, 
                string status
            )
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            if(status is null)
            {
                throw new ArgumentNullException(nameof(status));
            }

            return value.Replace(_defaultReplacableTextToStatus, status);
        }

        internal static string ReplaceRtfColor(
                this string? value, 
                string color
            )
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            if (string.IsNullOrWhiteSpace(color))
            {
                throw new ArgumentNullException(nameof(color));
            }

            return value.Replace(_defaultReplacableTextToColor, color);
        }

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

        internal static string ToCamelCase(
                this string? value
            )
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            return string.Join(" ",
                                value
                                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                    .Select(s =>
                                    {
                                        return s[0].ToString().ToUpper() + (s.Length > 1 ? s[1..].ToString().ToLower() : string.Empty);
                                    }).ToList()
                            );
        }
    }
}
