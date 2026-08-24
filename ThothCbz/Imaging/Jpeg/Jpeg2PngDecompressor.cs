using System.Numerics;
using ThothCbz.Constants;

namespace ThothCbz.Imaging.Jpeg
{
    /// <summary>
    /// Managed port of the jpeg2png artifact-removal solver.
    ///
    /// The objective combines total variation with a second order (TGV) term. It is
    /// minimised with FISTA, and after every gradient step the iterate is projected
    /// back onto the set of images whose DCT coefficients still quantize to the values
    /// actually stored in the JPEG. That constraint is what removes ringing and
    /// blocking without washing out genuine detail.
    /// </summary>
    internal static class Jpeg2PngDecompressor
    {
        /// <summary>
        /// Reconstructs one component as a float plane in the 0..255 range.
        /// </summary>
        internal static float[] Decompress(
                JpegComponent component,
                float weight,
                int iterations
            )
        {
            var width = component.BlocksPerLine * 8;
            var height = component.BlocksPerColumn * 8;

            var current = Dequantize(component, width, height);

            if (iterations <= 0 || weight <= 0.0f)
                return current;

            var previous = (float[])current.Clone();
            var auxiliary = (float[])current.Clone();

            var gradient = new float[width * height];
            var fieldX = new float[width * height];
            var fieldY = new float[width * height];
            var curvature = new float[width * height];

            // Step size from the Lipschitz constant of the TV/TGV gradient.
            var stepSize = 1.0f / (2.0f * MathF.Sqrt(8.0f));

            var theta = 1.0f;

            var lanes = Vector<float>.Count;
            var vectorLimit = current.Length - (current.Length % lanes);

            for (var iteration = 0; iteration < iterations; iteration++)
            {
                ComputeObjectiveGradient(auxiliary, gradient, fieldX, fieldY, curvature, width, height, weight);

                var step = new Vector<float>(stepSize);

                var i = 0;

                for (; i < vectorLimit; i += lanes)
                {
                    var value = new Vector<float>(auxiliary, i) - (step * new Vector<float>(gradient, i));

                    value.CopyTo(current, i);
                }

                for (; i < current.Length; i++)
                    current[i] = auxiliary[i] - (stepSize * gradient[i]);

                ProjectOntoQuantizationBins(component, current, width);

                var nextTheta = 0.5f * (1.0f + MathF.Sqrt(1.0f + (4.0f * theta * theta)));
                var momentum = (theta - 1.0f) / nextTheta;

                var momentumVector = new Vector<float>(momentum);

                i = 0;

                for (; i < vectorLimit; i += lanes)
                {
                    var value = new Vector<float>(current, i);

                    (value + (momentumVector * (value - new Vector<float>(previous, i)))).CopyTo(auxiliary, i);

                    value.CopyTo(previous, i);
                }

                for (; i < current.Length; i++)
                {
                    auxiliary[i] = current[i] + (momentum * (current[i] - previous[i]));
                    previous[i] = current[i];
                }

                theta = nextTheta;
            }

            return current;
        }

        /// <summary>
        /// Builds the initial estimate by multiplying the stored coefficients by the
        /// quantization table and inverse transforming each block.
        /// </summary>
        private static float[] Dequantize(
                JpegComponent component,
                int width,
                int height
            )
        {
            var plane = new float[width * height];

            Parallel.For(0, component.BlocksPerColumn, blockRow =>
            {
                Span<float> block = stackalloc float[64];

                for (var blockColumn = 0; blockColumn < component.BlocksPerLine; blockColumn++)
                {
                    var offset = component.CoefficientOffset(blockRow, blockColumn);

                    for (var i = 0; i < 64; i++)
                        block[i] = component.Coefficients[offset + i] * component.QuantizationTable[i];

                    Dct.Inverse(block);

                    for (var y = 0; y < 8; y++)
                    {
                        var target = (((blockRow * 8) + y) * width) + (blockColumn * 8);

                        for (var x = 0; x < 8; x++)
                        {
                            // JPEG stores samples level-shifted by 128.
                            plane[target + x] = block[(y * 8) + x] + 128.0f;
                        }
                    }
                }
            });

            return plane;
        }

        /// <summary>
        /// Forward transforms the current estimate and clamps every coefficient into
        /// the interval that would have produced the stored quantized value, i.e.
        /// [q*(c - 0.5), q*(c + 0.5)].
        /// </summary>
        private static void ProjectOntoQuantizationBins(
                JpegComponent component,
                float[] plane,
                int width
            )
        {
            Parallel.For(0, component.BlocksPerColumn, blockRow =>
            {
                Span<float> block = stackalloc float[64];

                for (var blockColumn = 0; blockColumn < component.BlocksPerLine; blockColumn++)
                {
                    var offset = component.CoefficientOffset(blockRow, blockColumn);

                    for (var y = 0; y < 8; y++)
                    {
                        var source = (((blockRow * 8) + y) * width) + (blockColumn * 8);

                        for (var x = 0; x < 8; x++)
                            block[(y * 8) + x] = plane[source + x] - 128.0f;
                    }

                    Dct.Forward(block);

                    for (var i = 0; i < 64; i++)
                    {
                        float quantizationStep = component.QuantizationTable[i];
                        float target = component.Coefficients[offset + i] * quantizationStep;

                        var lower = target - (quantizationStep * 0.5f);
                        var upper = target + (quantizationStep * 0.5f);

                        block[i] = Math.Clamp(block[i], lower, upper);
                    }

                    Dct.Inverse(block);

                    for (var y = 0; y < 8; y++)
                    {
                        var target = (((blockRow * 8) + y) * width) + (blockColumn * 8);

                        for (var x = 0; x < 8; x++)
                            plane[target + x] = block[(y * 8) + x] + 128.0f;
                    }
                }
            });
        }

        private const float SMOOTHING_EPSILON = 1.0e-3f;

        /// <summary>
        /// Gradient of the smoothed total variation term plus the second order term.
        /// Both use a Huber-style smoothing so the objective stays differentiable.
        ///
        /// The computation is expressed as a gather rather than a scatter: each pass
        /// first evaluates a per-pixel field, then every output cell reads the
        /// neighbours it needs. That keeps the rows independent, so no atomics or
        /// locking are required and the inner loops can be vectorised.
        /// </summary>
        private static void ComputeObjectiveGradient(
                float[] plane,
                float[] gradient,
                float[] fieldX,
                float[] fieldY,
                float[] curvature,
                int width,
                int height,
                float weight
            )
        {
            ComputeNormalisedGradientField(plane, fieldX, fieldY, width, height);
            ComputeCurvatureField(plane, curvature, width, height, weight * 0.5f);

            Parallel.For(0, height, y =>
            {
                var rowOffset = y * width;

                for (var x = 0; x < width; x++)
                {
                    var index = rowOffset + x;

                    // Negative divergence of the normalised gradient field.
                    var divergence = fieldX[index] + fieldY[index];

                    if (x > 0)
                        divergence -= fieldX[index - 1];

                    if (y > 0)
                        divergence -= fieldY[index - width];

                    var value = weight * divergence;

                    // Second order term: 4*c(i) minus the four neighbouring curvatures.
                    var second = 4.0f * curvature[index];

                    if (x > 0)
                        second -= curvature[index - 1];

                    if (x + 1 < width)
                        second -= curvature[index + 1];

                    if (y > 0)
                        second -= curvature[index - width];

                    if (y + 1 < height)
                        second -= curvature[index + width];

                    gradient[index] = value + second;
                }
            });
        }

        /// <summary>
        /// Forward differences normalised by the smoothed gradient magnitude.
        /// </summary>
        private static void ComputeNormalisedGradientField(
                float[] plane,
                float[] fieldX,
                float[] fieldY,
                int width,
                int height
            )
        {
            var lanes = Vector<float>.Count;

            Parallel.For(0, height, y =>
            {
                var rowOffset = y * width;
                var hasRowBelow = y + 1 < height;

                var x = 0;

                // The last column needs the clamped boundary rule, so it is excluded
                // from the vectorised span.
                var vectorLimit = width - 1 - ((width - 1) % lanes);

                if (hasRowBelow)
                {
                    var epsilon = new Vector<float>(SMOOTHING_EPSILON);

                    for (; x < vectorLimit; x += lanes)
                    {
                        var index = rowOffset + x;

                        var value = new Vector<float>(plane, index);
                        var right = new Vector<float>(plane, index + 1);
                        var down = new Vector<float>(plane, index + width);

                        var dx = right - value;
                        var dy = down - value;

                        var norm = Vector.SquareRoot((dx * dx) + (dy * dy) + epsilon);

                        (dx / norm).CopyTo(fieldX, index);
                        (dy / norm).CopyTo(fieldY, index);
                    }
                }

                for (; x < width; x++)
                {
                    var index = rowOffset + x;

                    var value = plane[index];

                    var right = x + 1 < width ? plane[index + 1] : value;
                    var down = hasRowBelow ? plane[index + width] : value;

                    var dx = right - value;
                    var dy = down - value;

                    var norm = MathF.Sqrt((dx * dx) + (dy * dy) + SMOOTHING_EPSILON);

                    fieldX[index] = dx / norm;
                    fieldY[index] = dy / norm;
                }
            });
        }

        /// <summary>
        /// Per-pixel normalised Laplacian for the second order (TGV) term. It penalises
        /// curvature, which suppresses the staircase artifacts a pure TV prior would
        /// introduce in smooth gradients.
        /// </summary>
        private static void ComputeCurvatureField(
                float[] plane,
                float[] curvature,
                int width,
                int height,
                float secondOrderWeight
            )
        {
            var lanes = Vector<float>.Count;

            Parallel.For(0, height, y =>
            {
                var rowOffset = y * width;

                var hasRowAbove = y > 0;
                var hasRowBelow = y + 1 < height;

                var x = 0;

                if (hasRowAbove && hasRowBelow)
                {
                    var epsilon = new Vector<float>(SMOOTHING_EPSILON);
                    var weightVector = new Vector<float>(secondOrderWeight);
                    var four = new Vector<float>(4.0f);

                    // Interior columns only; both borders use the clamped rule below.
                    var start = 1;
                    var vectorLimit = width - 1;

                    for (x = start; x + lanes <= vectorLimit; x += lanes)
                    {
                        var index = rowOffset + x;

                        var value = new Vector<float>(plane, index);
                        var left = new Vector<float>(plane, index - 1);
                        var right = new Vector<float>(plane, index + 1);
                        var up = new Vector<float>(plane, index - width);
                        var down = new Vector<float>(plane, index + width);

                        var laplacian = right + left + up + down - (four * value);

                        var scale = weightVector / Vector.SquareRoot((laplacian * laplacian) + epsilon);

                        (laplacian * scale).CopyTo(curvature, index);
                    }
                }

                for (var column = 0; column < width; column++)
                {
                    // Skip the span already handled by the vectorised loop.
                    if (hasRowAbove && hasRowBelow && column >= 1 && column < x)
                        continue;

                    var index = rowOffset + column;

                    var value = plane[index];

                    var left = column > 0 ? plane[index - 1] : value;
                    var right = column + 1 < width ? plane[index + 1] : value;
                    var up = hasRowAbove ? plane[index - width] : value;
                    var down = hasRowBelow ? plane[index + width] : value;

                    var laplacian = right + left + up + down - (4.0f * value);

                    var scale = secondOrderWeight / MathF.Sqrt((laplacian * laplacian) + SMOOTHING_EPSILON);

                    curvature[index] = laplacian * scale;
                }
            });
        }

        internal static float[] Decompress(
                JpegComponent component
            )
        {
            return Decompress(
                    component,
                    GlobalConstants.DEFAULT_JPEG2PNG_WEIGHT,
                    GlobalConstants.DEFAULT_JPEG2PNG_ITERATIONS
                );
        }
    }
}
