using ThothCbz.Enumerators;

namespace ThothCbz.Extensions
{
    internal static class SupportedLanguageTypeExtensions
    {
        internal static int GetStringRtfFontIndex(
                this SupportedLanguageType language
            )
        {
            var defaultFont = 0;

            switch (language)
            {
                case SupportedLanguageType.Arabic:
                    return 4;
                case SupportedLanguageType.BrazilianPortuguese:
                    return 1;
                case SupportedLanguageType.Chinese:
                    return 2;
                case SupportedLanguageType.English:
                    return defaultFont;
                case SupportedLanguageType.Japanese:
                    return 3;
                default:
                    return defaultFont;
            }
        }
    }
}
