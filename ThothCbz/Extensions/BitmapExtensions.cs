using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

using ThothCbz.Constants;
using ThothCbz.Entities;

namespace ThothCbz.Extensions
{
    internal static class BitmapExtensions
    {
        internal static void SaveAsJpg(
                this Bitmap img,
                FileEntity fileEntity,
                string? uniqueIdentifier
            )
        {
            img.SaveAsJpg(
                    filePath: fileEntity.GetFilePathToJpgValue(uniqueIdentifier)
                );
        }

        internal static void SaveAsJpg(
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

        internal static Bitmap Sharpen(
                this Bitmap img,
                double strength = 3
            )
        {
            double FactorCorrectionValue = 12;
            int width = img.Width;
            int height = img.Height;
            strength = strength * 2.5 / 100;

            // Create sharpening filter.
            var filter = new double[,]
                {
                    {-1, -1, -1, -1, -1},
                    {-1,  2,  2,  2, -1},
                    {-1,  2,  16, 2, -1},
                    {-1,  2, -1,  2, -1},
                    {-1, -1, -1, -1, -1}
                };

            //const int filterSize = 3; // wenn die Matrix 3 Zeilen und 3 Spalten besitzt dann 3 bei 4 = 4 usw.                    
            int filterSize = filter.GetLength(0);

            double bias = 1.0 - strength;
            double factor = strength / FactorCorrectionValue;

            //const int s = filterSize / 2;
            int s = filterSize / 2; // Filtersize ist keine Constante mehr darum wurde der befehl const entfernt

            var result = new System.Drawing.Color[img.Width, img.Height];

            BitmapData pbits = img.LockBits(new System.Drawing.Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            // Declare an array to hold the bytes of the bitmap.
            int bytes = pbits.Stride * height;
            var rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(pbits.Scan0, rgbValues, 0, bytes);

            int rgb;
            // Fill the color array with the new sharpened color values.
            for (int x = s; x < width - s; x++)
            {
                for (int y = s; y < height - s; y++)
                {
                    double red = 0.0, green = 0.0, blue = 0.0;

                    for (int filterX = 0; filterX < filterSize; filterX++)
                    {
                        for (int filterY = 0; filterY < filterSize; filterY++)
                        {
                            int imageX = (x - s + filterX + width) % width;
                            int imageY = (y - s + filterY + height) % height;

                            rgb = imageY * pbits.Stride + 3 * imageX;

                            red += rgbValues[rgb + 2] * filter[filterX, filterY];
                            green += rgbValues[rgb + 1] * filter[filterX, filterY];
                            blue += rgbValues[rgb + 0] * filter[filterX, filterY];
                        }

                        rgb = y * pbits.Stride + 3 * x;

                        int r = Math.Min(Math.Max((int)(factor * red + (bias * rgbValues[rgb + 2])), 0), 255);
                        int g = Math.Min(Math.Max((int)(factor * green + (bias * rgbValues[rgb + 1])), 0), 255);
                        int b = Math.Min(Math.Max((int)(factor * blue + (bias * rgbValues[rgb + 0])), 0), 255);

                        result[x, y] = System.Drawing.Color.FromArgb(r, g, b);
                    }
                }
            }

            // Update the image with the sharpened pixels.
            for (int x = s; x < width - s; x++)
            {
                for (int y = s; y < height - s; y++)
                {
                    rgb = y * pbits.Stride + 3 * x;

                    rgbValues[rgb + 2] = result[x, y].R;
                    rgbValues[rgb + 1] = result[x, y].G;
                    rgbValues[rgb + 0] = result[x, y].B;
                }
            }

            // Copy the RGB values back to the bitmap.
            Marshal.Copy(rgbValues, 0, pbits.Scan0, bytes);
            // Release image bits.
            img.UnlockBits(pbits);

            return img;
        }
    }
}
