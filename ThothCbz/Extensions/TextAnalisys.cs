using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ThothCbz.Enumerators;

namespace ThothCbz.Extensions
{
    internal static class TextAnalisys
    {
        internal static string GetTextFont(
                this string text
            )
        {
            string defaultFont = "Segoe UI";

            if (string.IsNullOrWhiteSpace(text))
                return defaultFont;
            else if (Regex.IsMatch(text, @"\p{IsCJKUnifiedIdeographs}")) // Chinese
                return "Microsoft YaHei";
            else if (Regex.IsMatch(text, @"\p{IsArabic}")) // Arabic
                return defaultFont;
            else if (Regex.IsMatch(text, @"\p{IsHiragana}|\p{IsKatakana}|\p{IsCJKUnifiedIdeographs}")) // Japanese
                return "MS Gothic";
            else if (text.Any(c => CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.LowercaseLetter && c >= 'a' && c <= 'z')) // English
                return defaultFont;
            else if (text.Any(c => CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.LowercaseLetter && c >= 'a' && c <= 'z') || text.Contains("ã") || text.Contains("õ") || text.Contains("á") || text.Contains("é") || text.Contains("í") || text.Contains("ó") || text.Contains("ú") || text.Contains("ç")) // English
                return defaultFont;

            return defaultFont;
        }
    }
}
