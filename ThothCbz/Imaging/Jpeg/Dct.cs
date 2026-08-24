namespace ThothCbz.Imaging.Jpeg
{
    /// <summary>
    /// Orthonormal 8x8 DCT-II / DCT-III used by the solver. The transform must be
    /// orthonormal so that projecting in the coefficient domain is equivalent to a
    /// projection in the spatial domain.
    /// </summary>
    internal static class Dct
    {
        private static readonly float[] _basis = BuildBasisTable();

        /// <summary>
        /// Basis table with the 0.5 scale and the alpha normalisation folded in:
        /// basis[k*8+u] = 0.5 * alpha[u] * cos((2k+1)*u*PI/16). Folding these constants
        /// changes no arithmetic result, so the transform stays orthonormal.
        /// </summary>
        private static float[] BuildBasisTable()
        {
            var table = new float[64];

            for (var k = 0; k < 8; k++)
            {
                for (var u = 0; u < 8; u++)
                {
                    var alpha = u == 0 ? 1.0 / Math.Sqrt(2.0) : 1.0;

                    table[(k * 8) + u] = (float)(0.5 * alpha * Math.Cos((2.0 * k + 1.0) * u * Math.PI / 16.0));
                }
            }

            return table;
        }

        /// <summary>
        /// Computes the 8-term dot product of a source row against a basis row.
        /// </summary>
        private static float Dot(
                ReadOnlySpan<float> source,
                int sourceOffset,
                int sourceStride,
                ReadOnlySpan<float> basis,
                int basisOffset,
                int basisStride
            )
        {
            return (source[sourceOffset] * basis[basisOffset])
                 + (source[sourceOffset + sourceStride] * basis[basisOffset + basisStride])
                 + (source[sourceOffset + (sourceStride * 2)] * basis[basisOffset + (basisStride * 2)])
                 + (source[sourceOffset + (sourceStride * 3)] * basis[basisOffset + (basisStride * 3)])
                 + (source[sourceOffset + (sourceStride * 4)] * basis[basisOffset + (basisStride * 4)])
                 + (source[sourceOffset + (sourceStride * 5)] * basis[basisOffset + (basisStride * 5)])
                 + (source[sourceOffset + (sourceStride * 6)] * basis[basisOffset + (basisStride * 6)])
                 + (source[sourceOffset + (sourceStride * 7)] * basis[basisOffset + (basisStride * 7)]);
        }

        /// <summary>
        /// Forward 8x8 DCT, separable rows then columns.
        /// </summary>
        internal static void Forward(
                Span<float> block
            )
        {
            Span<float> temporary = stackalloc float[64];

            ReadOnlySpan<float> basis = _basis;

            for (var y = 0; y < 8; y++)
            {
                for (var u = 0; u < 8; u++)
                    temporary[(y * 8) + u] = Dot(block, y * 8, 1, basis, u, 8);
            }

            for (var u = 0; u < 8; u++)
            {
                for (var v = 0; v < 8; v++)
                    block[(v * 8) + u] = Dot(temporary, u, 8, basis, v, 8);
            }
        }

        /// <summary>
        /// Inverse 8x8 DCT, separable columns then rows.
        /// </summary>
        internal static void Inverse(
                Span<float> block
            )
        {
            Span<float> temporary = stackalloc float[64];

            ReadOnlySpan<float> basis = _basis;

            for (var u = 0; u < 8; u++)
            {
                for (var y = 0; y < 8; y++)
                    temporary[(y * 8) + u] = Dot(block, u, 8, basis, y * 8, 1);
            }

            for (var y = 0; y < 8; y++)
            {
                for (var x = 0; x < 8; x++)
                    block[(y * 8) + x] = Dot(temporary, y * 8, 1, basis, x * 8, 1);
            }
        }
    }
}
