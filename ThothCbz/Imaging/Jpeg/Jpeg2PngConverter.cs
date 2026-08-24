using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using ThothCbz.Constants;

namespace ThothCbz.Imaging.Jpeg
{
    /// <summary>
    /// Managed replacement for jpeg2png_1.02_x64.exe. Decodes a JPEG to its quantized
    /// DCT coefficients, runs the TV/TGV solver to remove compression artifacts, and
    /// writes the result as a PNG.
    /// </summary>
    internal static class Jpeg2PngConverter
    {
        /// <summary>
        /// Converts <paramref name="sourceFilePath"/> into a PNG at
        /// <paramref name="destinationFilePath"/>. Inputs the coefficient decoder
        /// cannot handle fall back to a plain decode so conversion never fails.
        /// </summary>
        internal static void Convert(
                string sourceFilePath,
                string destinationFilePath,
                float weight = GlobalConstants.DEFAULT_JPEG2PNG_WEIGHT,
                int iterations = GlobalConstants.DEFAULT_JPEG2PNG_ITERATIONS
            )
        {
            try
            {
                var data = File.ReadAllBytes(sourceFilePath);

                var image = JpegCoefficientDecoder.Decode(data);

                using var output = Reconstruct(image, weight, iterations);

                output.Save(destinationFilePath, new PngEncoder());
            }
            catch (Exception exception) when (exception is UnsupportedJpegException
                                                or IndexOutOfRangeException
                                                or ArgumentOutOfRangeException
                                                or ArgumentException
                                                or OverflowException)
            {
                // Arithmetic coding, CMYK, 12-bit or otherwise malformed payloads:
                // decode conventionally so the pipeline still produces a PNG.
                ConvertWithFallback(sourceFilePath, destinationFilePath);
            }
        }

        /// <summary>
        /// Runs the solver on every component and assembles the final RGB image.
        /// </summary>
        private static SixLabors.ImageSharp.Image<Rgb24> Reconstruct(
                JpegImageData image,
                float weight,
                int iterations
            )
        {
            var planes = new float[image.Components.Count][];
            var planeWidths = new int[image.Components.Count];
            var planeHeights = new int[image.Components.Count];

            for (var i = 0; i < image.Components.Count; i++)
            {
                var component = image.Components[i];

                planes[i] = Jpeg2PngDecompressor.Decompress(component, weight, iterations);

                planeWidths[i] = component.BlocksPerLine * 8;
                planeHeights[i] = component.BlocksPerColumn * 8;
            }

            using var result = new Image<Rgb24>(image.Width, image.Height);

            var isGrayscale = image.Components.Count == 1;

            result.ProcessPixelRows(accessor =>
            {
                for (var y = 0; y < image.Height; y++)
                {
                    var row = accessor.GetRowSpan(y);

                    for (var x = 0; x < image.Width; x++)
                    {
                        if (isGrayscale)
                        {
                            var luma = Sample(image, planes, planeWidths, planeHeights, 0, x, y);
                            var gray = ClampToByte(luma);

                            row[x] = new Rgb24(gray, gray, gray);

                            continue;
                        }

                        var yValue = Sample(image, planes, planeWidths, planeHeights, 0, x, y);
                        var cb = Sample(image, planes, planeWidths, planeHeights, 1, x, y) - 128.0f;
                        var cr = Sample(image, planes, planeWidths, planeHeights, 2, x, y) - 128.0f;

                        row[x] = new Rgb24(
                                ClampToByte(yValue + (1.402f * cr)),
                                ClampToByte(yValue - (0.344136f * cb) - (0.714136f * cr)),
                                ClampToByte(yValue + (1.772f * cb))
                            );
                    }
                }
            });

            return result;
        }

        /// <summary>
        /// Samples a component at full image resolution, expanding subsampled chroma
        /// planes by replicating according to their sampling factors.
        /// </summary>
        private static float Sample(
                JpegImageData image,
                float[][] planes,
                int[] planeWidths,
                int[] planeHeights,
                int componentIndex,
                int x,
                int y
            )
        {
            var component = image.Components[componentIndex];

            var sourceX = x * component.HorizontalSamplingFactor / image.MaxHorizontalSamplingFactor;
            var sourceY = y * component.VerticalSamplingFactor / image.MaxVerticalSamplingFactor;

            if (sourceX >= planeWidths[componentIndex])
                sourceX = planeWidths[componentIndex] - 1;

            if (sourceY >= planeHeights[componentIndex])
                sourceY = planeHeights[componentIndex] - 1;

            return planes[componentIndex][(sourceY * planeWidths[componentIndex]) + sourceX];
        }

        private static byte ClampToByte(
                float value
            )
        {
            if (value <= 0.0f)
                return 0;

            if (value >= 255.0f)
                return 255;

            return (byte)(value + 0.5f);
        }

        /// <summary>
        /// Plain decode used when the coefficient decoder cannot process the input.
        /// No artifact removal is applied, but a valid PNG is still produced.
        /// </summary>
        private static void ConvertWithFallback(
                string sourceFilePath,
                string destinationFilePath
            )
        {
            using var image = SixLabors.ImageSharp.Image.Load<Rgb24>(sourceFilePath);

            image.Save(destinationFilePath, new PngEncoder());
        }
    }
}
