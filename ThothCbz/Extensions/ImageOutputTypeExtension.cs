using ThothCbz.Constants;
using ThothCbz.Enumerators;

namespace ThothCbz.Extensions
{
    internal static class ImageOutputFileTypeExtension
    {
        internal static string GetFileExtension(
                this ImageOutputFileType value
            )
        {
            return value switch
            {
                ImageOutputFileType.JPG => GlobalConstants.DEFAULT_JPG_EXTENSION,
                ImageOutputFileType.PNG => GlobalConstants.DEFAULT_PNG_EXTENSION,
                _ => throw new NotImplementedException()
            };
        }

        internal static string GetImageOutputFileTypeExtension(
                this int value
            )
        {
            return ((ImageOutputFileType)value).GetFileExtension();
        }
    }
}
