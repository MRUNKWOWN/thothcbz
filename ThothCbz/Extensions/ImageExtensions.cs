using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

using ThothCbz.Constants;

using Color = System.Drawing.Color;
using Rectangle = System.Drawing.Rectangle;

namespace ThothCbz.Extensions
{
    internal static class ImageExtensions
    {
        internal static Bitmap Resize(
                this System.Drawing.Image img,
                float resizeFactor,
                Color backgroundColor
            )
        {
            var newWidth = (int)(img.Width * resizeFactor);
            var newHeight = (int)(img.Height * resizeFactor);

            var newHorizontalResolution = img.HorizontalResolution * resizeFactor;
            var newVerticalResolution = img.HorizontalResolution * resizeFactor;

            return img.NewImage(
                    newWidth,
                    newHeight,
                    newHorizontalResolution,
                    newVerticalResolution,
                    backgroundColor
                );
        }

        internal static Bitmap NewImage(
                this System.Drawing.Image img,
                int width,
                int height,
                float horizontalResolution,
                float verticalResolution,
                Color backgroundColor
            )
        {
            using var newImg = new Bitmap(width, height, PixelFormat.Format32bppRgb);

            newImg.SetResolution(horizontalResolution, verticalResolution);

            using var graphics = Graphics.FromImage(newImg).SetDefaultQuality(backgroundColor: backgroundColor);

            graphics.DrawImage(
                                img,
                                new Rectangle(0, 0, newImg.Width, newImg.Height)
                            );

            return (Bitmap) newImg.Clone();
        }
    }
}
