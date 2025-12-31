using System.Drawing.Imaging;

using ThothCbz.Constants;
using ThothCbz.Entities;
using ThothCbz.Enumerators;
using ThothCbz.Properties;

namespace ThothCbz.Extensions
{
    internal static class BitmapExtensions
    {
        internal static void SaveAs(
                this Bitmap img,
                FileEntity fileEntity,
                string? uniqueIdentifier
            )
        {
            img.SaveAs(
                    filePath: fileEntity.GetFilePathToImageOutputFileTypeValue(uniqueIdentifier)
                );
        }

        internal static void SaveAs(
                this Bitmap img,
                string filePath
            )
        {
            switch ((ImageOutputFileType)Settings.Default.ImageOutputFileType)
            {
                case ImageOutputFileType.JPG:
                    img.SaveAsJpg(filePath: filePath);
                    break;
                case ImageOutputFileType.PNG:
                    img.SaveAsPng(filePath: filePath);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private static void SaveAsJpg(
                this Bitmap img,
                string filePath
            )
        {
            EncoderParameter qualityParam = new EncoderParameter(
                                                                    System.Drawing.Imaging.Encoder.Quality,
                                                                    100L
                                                                );
            ImageCodecInfo? jpegCodec = ImageCodecInfo.GetImageEncoders()
                                                        .Where(w => w.MimeType == GlobalConstants.DEFAULT_JPEG_MIME_TYPE)
                                                        .FirstOrDefault();
            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;

            img.Save(
                        filePath,
                        jpegCodec!,
                        encoderParams
                    );
        }

        private static void SaveAsPng(
                this Bitmap img,
                string filePath
            )
        {
            img.Save(
                        filePath,
                        ImageFormat.Png
                    );
        }
    }
}
