using System.Drawing;
using System.Drawing.Imaging;

namespace ThothCbz.Extensions
{
    /// <summary>
    /// Managed replacement for the ImageMagick "-sharpen 0xN" operator.
    /// ImageMagick sharpens by subtracting a gaussian blurred copy from a doubled original,
    /// which is reproduced here with a separable gaussian kernel.
    /// The vertical pass streams through a small ring of horizontally blurred rows so that
    /// no full sized intermediate buffer is allocated, keeping the memory footprint low
    /// even when several large pages are processed in parallel.
    /// </summary>
    internal static class SharpenExtensions
    {
        /// <summary>
        /// Returns a bitmap whose pixel format can be processed by <see cref="SharpenInPlace"/>.
        /// Indexed, 16 bit and other exotic formats are converted to 32bppArgb.
        /// The returned instance may be the source itself, disposing it twice is safe.
        /// </summary>
        internal static Bitmap EnsureSharpenableFormat(
                this Bitmap bitmap
            )
        {
            if (bitmap.PixelFormat == PixelFormat.Format24bppRgb || bitmap.PixelFormat == PixelFormat.Format32bppArgb || bitmap.PixelFormat == PixelFormat.Format32bppRgb)
            {
                return bitmap;
            }

            var converted = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format32bppArgb);
            converted.SetResolution(bitmap.HorizontalResolution, bitmap.VerticalResolution);

            using (var graphics = Graphics.FromImage(converted))
            {
                graphics.DrawImage(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
            }

            return converted;
        }

        internal static void SharpenInPlace(
                this Bitmap bitmap,
                double sigma
            )
        {
            if (sigma <= 0)
                return;

            var bytesPerPixel = Image.GetPixelFormatSize(bitmap.PixelFormat) / 8;

            if (bytesPerPixel != 3 && bytesPerPixel != 4)
                throw new NotSupportedException($"Unsupported pixel format: {bitmap.PixelFormat}");

            var kernel = BuildGaussianKernel(sigma, out var radius);

            var width = bitmap.Width;
            var height = bitmap.Height;
            var ringSize = (radius * 2) + 1;
            var rowLength = width * bytesPerPixel;

            var ring = new float[ringSize * rowLength];
            var sourceRow = new byte[rowLength];

            var rect = new Rectangle(0, 0, width, height);
            var data = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);

            try
            {
                unsafe
                {
                    var scan0 = (byte*)data.Scan0;

                    for (var j = -radius; j <= radius; j++)
                    {
                        BlurRowHorizontally(scan0, data.Stride, width, height, bytesPerPixel, j, kernel, radius, ring, ringSize, rowLength);
                    }

                    for (var y = 0; y < height; y++)
                    {
                        if (y > 0)
                        {
                            BlurRowHorizontally(scan0, data.Stride, width, height, bytesPerPixel, y + radius, kernel, radius, ring, ringSize, rowLength);
                        }

                        var row = scan0 + ((long)y * data.Stride);

                        for (var i = 0; i < rowLength; i++)
                        {
                            sourceRow[i] = row[i];
                        }

                        for (var i = 0; i < rowLength; i++)
                        {
                            if (bytesPerPixel == 4 && (i % 4) == 3)
                            {
                                continue;
                            }

                            var blurred = 0f;

                            for (var j = -radius; j <= radius; j++)
                            {
                                var slot = RingSlot(y + j, ringSize) * rowLength;
                                blurred += ring[slot + i] * kernel[j + radius];
                            }

                            row[i] = Clamp((2f * sourceRow[i]) - blurred);
                        }
                    }
                }
            }
            finally
            {
                bitmap.UnlockBits(data);
            }
        }

        private static unsafe void BlurRowHorizontally(
                byte* scan0,
                int stride,
                int width,
                int height,
                int bytesPerPixel,
                int logicalRow,
                float[] kernel,
                int radius,
                float[] ring,
                int ringSize,
                int rowLength
            )
        {
            var sampleRow = scan0 + ((long)Math.Clamp(logicalRow, 0, height - 1) * stride);
            var slot = RingSlot(logicalRow, ringSize) * rowLength;

            for (var x = 0; x < width; x++)
            {
                var destinationIndex = slot + (x * bytesPerPixel);

                for (var channel = 0; channel < bytesPerPixel; channel++)
                {
                    var accumulator = 0f;

                    for (var k = -radius; k <= radius; k++)
                    {
                        var sampleX = Math.Clamp(x + k, 0, width - 1);
                        accumulator += sampleRow[(sampleX * bytesPerPixel) + channel] * kernel[k + radius];
                    }

                    ring[destinationIndex + channel] = accumulator;
                }
            }
        }

        private static int RingSlot(
                int logicalRow,
                int ringSize
            )
        {
            var slot = logicalRow % ringSize;

            return slot < 0
                    ? slot + ringSize
                    : slot;
        }

        private static float[] BuildGaussianKernel(
                double sigma,
                out int radius
            )
        {
            radius = Math.Max(1, (int)Math.Ceiling(sigma * 4.0));

            var size = (radius * 2) + 1;
            var kernel = new float[size];
            var twoSigmaSquared = 2.0 * sigma * sigma;
            var sum = 0.0;

            for (var i = -radius; i <= radius; i++)
            {
                var value = Math.Exp(-(i * i) / twoSigmaSquared) / Math.Sqrt(Math.PI * twoSigmaSquared);
                kernel[i + radius] = (float)value;
                sum += value;
            }

            for (var i = 0; i < size; i++)
            {
                kernel[i] = (float)(kernel[i] / sum);
            }

            return kernel;
        }

        private static byte Clamp(float value)
        {
            return value <= 0f
                    ? (byte)0
                    : value >= 255f
                        ? (byte)255
                        : (byte)(value + 0.5f);
        }
    }
}
