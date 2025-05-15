using System.Globalization;
using System.Text.RegularExpressions;
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
        private const string _defaultReplacableTextToFont = "_FONT_";

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

            return value.Replace(_defaultReplacableTextToName, GetRtfFormattedString(name));
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

        internal static string ReplaceRtfFont(
                this string? value,
                string fontIndex
            )
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            if (string.IsNullOrWhiteSpace(fontIndex))
            {
                throw new ArgumentNullException(nameof(fontIndex));
            }

            return value.Replace(_defaultReplacableTextToFont, $"\\f{fontIndex}\\fs18");
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

        internal static string GetStringRtfFontIndex(
                this string text
            )
        {
            return GetSupportedLanguage(text).GetStringRtfFontIndex().ToString();
        }

        private static SupportedLanguageType GetSupportedLanguage(string text)
        {
            SupportedLanguageType defaultLanguange = SupportedLanguageType.English;

            if (string.IsNullOrWhiteSpace(text))
                return defaultLanguange;
            else if (Regex.IsMatch(text, @"\p{IsCJKUnifiedIdeographs}"))
                return SupportedLanguageType.Chinese;
            else if (Regex.IsMatch(text, @"\p{IsArabic}")) // Arabic
                return SupportedLanguageType.Arabic;
            else if (Regex.IsMatch(text, @"\p{IsHiragana}|\p{IsKatakana}|\p{IsCJKUnifiedIdeographs}")) // Japanese
                return SupportedLanguageType.Japanese;
            else if (text.Any(c => CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.LowercaseLetter && c >= 'a' && c <= 'z')) // English
                return SupportedLanguageType.English;
            else if (text.Any(c => CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.LowercaseLetter && c >= 'a' && c <= 'z') || text.Contains("ã") || text.Contains("õ") || text.Contains("á") || text.Contains("é") || text.Contains("í") || text.Contains("ó") || text.Contains("ú") || text.Contains("ç")) // English
                return SupportedLanguageType.BrazilianPortuguese;

            return defaultLanguange;
        }

        private static string GetRtfFormattedString(string? originalValue)
        {
            if(string.IsNullOrWhiteSpace(originalValue))
            {
                return string.Empty;
            }

            using var rtb = new RichTextBox();
            rtb.Text = originalValue;

            var lines = rtb.Rtf.Split("\r\n");

            if (lines.Length < 3)
            {
                return string.Empty;
            }

            lines = lines[2].Replace("\\pard\\f0\\fs18 ", string.Empty).Split("\\f0\\par");

            if (lines.Length < 2)
            {
                return string.Empty;
            }

            return lines[0];
        }
    }
}
