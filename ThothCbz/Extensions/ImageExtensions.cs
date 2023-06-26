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
                Color backgroundColor,
                System.Drawing.Size? defaultSize = null
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
                    backgroundColor,
                    defaultSize
                );
        }

        internal static Bitmap NewImage(
                this System.Drawing.Image img,
                int width,
                int height,
                float horizontalResolution,
                float verticalResolution,
                Color backgroundColor,
                System.Drawing.Size? defaultSize = null
            )
        {
            var targetHeight = height;

            if (defaultSize.HasValue)
            {
                var factor = defaultSize.Value.Width > defaultSize.Value.Height
                                ? width > height ? 1 : 0.5
                                : width > height ? 2 : 1;
                
                targetHeight = (int)((defaultSize.Value.Height * width) / (defaultSize.Value.Width * factor));
                targetHeight = targetHeight > height ? height : targetHeight;
            }

            using var newImg = new Bitmap(width, targetHeight, PixelFormat.Format32bppRgb);

            newImg.SetResolution(horizontalResolution, verticalResolution);

            using var graphics = Graphics.FromImage(newImg).SetDefaultQuality(backgroundColor: backgroundColor);

            graphics.DrawImage(
                                img,
                                0,0,
                                new Rectangle(0, 0, newImg.Width, newImg.Height),
                                GraphicsUnit.Pixel
                            );

            return (Bitmap) newImg.Clone();
        }
    }
}
