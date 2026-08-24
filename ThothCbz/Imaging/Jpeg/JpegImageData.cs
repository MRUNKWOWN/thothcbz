namespace ThothCbz.Imaging.Jpeg
{
    /// <summary>
    /// A single colour component of a JPEG frame, holding its quantized DCT
    /// coefficients in block order together with its quantization table.
    /// </summary>
    internal sealed class JpegComponent
    {
        internal int Id;
        internal int HorizontalSamplingFactor;
        internal int VerticalSamplingFactor;
        internal int QuantizationTableId;

        internal int DcTableId;
        internal int AcTableId;

        /// <summary>Blocks across / down for this component.</summary>
        internal int BlocksPerLine;
        internal int BlocksPerColumn;

        /// <summary>Pixel dimensions of this component before chroma upsampling.</summary>
        internal int Width;
        internal int Height;

        /// <summary>Quantized coefficients, 64 per block, in zig-zag natural order.</summary>
        internal short[] Coefficients = [];

        internal ushort[] QuantizationTable = new ushort[64];

        /// <summary>Running DC predictor used while decoding scans.</summary>
        internal int DcPredictor;

        internal int CoefficientOffset(
                int blockRow,
                int blockColumn
            )
        {
            return ((blockRow * BlocksPerLine) + blockColumn) * 64;
        }
    }

    /// <summary>
    /// Fully parsed JPEG frame: dimensions, components and their quantized
    /// coefficients, ready for the artifact-removal solver.
    /// </summary>
    internal sealed class JpegImageData
    {
        internal int Width;
        internal int Height;
        internal bool IsProgressive;

        internal int MaxHorizontalSamplingFactor = 1;
        internal int MaxVerticalSamplingFactor = 1;

        internal int McusPerLine;
        internal int McusPerColumn;

        internal List<JpegComponent> Components { get; } = [];

        /// <summary>
        /// Natural (row-major) order of the 64 zig-zag positions.
        /// </summary>
        internal static readonly int[] ZigZagOrder =
        [
             0,  1,  8, 16,  9,  2,  3, 10,
            17, 24, 32, 25, 18, 11,  4,  5,
            12, 19, 26, 33, 40, 48, 41, 34,
            27, 20, 13,  6,  7, 14, 21, 28,
            35, 42, 49, 56, 57, 50, 43, 36,
            29, 22, 15, 23, 30, 37, 44, 51,
            58, 59, 52, 45, 38, 31, 39, 46,
            53, 60, 61, 54, 47, 55, 62, 63
        ];
    }
}
