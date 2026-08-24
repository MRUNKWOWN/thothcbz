namespace ThothCbz.Imaging.Jpeg
{
    /// <summary>
    /// Progressive decoding: each scan carries only part of the coefficient data,
    /// either a spectral band (Ss..Se) or a further approximation bit (Ah/Al), and
    /// the results accumulate into the component coefficient arrays.
    /// </summary>
    internal sealed partial class JpegCoefficientDecoder
    {
        private void DecodeProgressiveBlock(
                JpegBitReader reader,
                JpegComponent component,
                int offset,
                int spectralStart,
                int spectralEnd,
                int approximationHigh,
                int approximationLow
            )
        {
            if (spectralStart == 0)
            {
                if (approximationHigh == 0)
                    DecodeDcFirst(reader, component, offset, approximationLow);
                else
                    DecodeDcRefine(reader, component, offset, approximationLow);

                return;
            }

            if (approximationHigh == 0)
                DecodeAcFirst(reader, component, offset, spectralStart, spectralEnd, approximationLow);
            else
                DecodeAcRefine(reader, component, offset, spectralStart, spectralEnd, approximationLow);
        }

        /// <summary>
        /// First DC scan: a normal DC difference, shifted left by the approximation.
        /// </summary>
        private void DecodeDcFirst(
                JpegBitReader reader,
                JpegComponent component,
                int offset,
                int approximationLow
            )
        {
            var dcTable = _dcTables[component.DcTableId]
                            ?? throw new UnsupportedJpegException("Missing DC Huffman table.");

            var magnitude = dcTable.Decode(reader);
            var difference = magnitude == 0 ? 0 : JpegBitReader.Extend(reader.ReadBits(magnitude), magnitude);

            component.DcPredictor += difference;

            component.Coefficients[offset] = (short)(component.DcPredictor << approximationLow);
        }

        /// <summary>
        /// DC refinement scan: one correction bit per block.
        /// </summary>
        private static void DecodeDcRefine(
                JpegBitReader reader,
                JpegComponent component,
                int offset,
                int approximationLow
            )
        {
            if (reader.ReadBit() != 0)
                component.Coefficients[offset] |= (short)(1 << approximationLow);
        }

        /// <summary>
        /// First AC scan for a spectral band, with end-of-band run handling.
        /// </summary>
        private void DecodeAcFirst(
                JpegBitReader reader,
                JpegComponent component,
                int offset,
                int spectralStart,
                int spectralEnd,
                int approximationLow
            )
        {
            if (_eobRun > 0)
            {
                _eobRun--;

                return;
            }

            var acTable = _acTables[component.AcTableId]
                            ?? throw new UnsupportedJpegException("Missing AC Huffman table.");

            var index = spectralStart;

            while (index <= spectralEnd)
            {
                var symbol = acTable.Decode(reader);

                var run = symbol >> 4;
                var size = symbol & 0x0F;

                if (size == 0)
                {
                    if (run < 15)
                    {
                        // EOBn: this block ends here, and so do the next (2^run - 1) blocks.
                        _eobRun = (1 << run) - 1;

                        if (run > 0)
                            _eobRun += reader.ReadBits(run);

                        break;
                    }

                    index += 16;

                    continue;
                }

                index += run;

                if (index > spectralEnd || index > 63)
                    break;

                var value = JpegBitReader.Extend(reader.ReadBits(size), size);

                component.Coefficients[offset + JpegImageData.ZigZagOrder[index]] = (short)(value << approximationLow);

                index++;
            }
        }

        /// <summary>
        /// AC refinement scan. Newly non-zero coefficients get their sign and the
        /// current bit, while already non-zero coefficients consume a correction bit;
        /// zero-runs only count coefficients that are still zero.
        /// </summary>
        private void DecodeAcRefine(
                JpegBitReader reader,
                JpegComponent component,
                int offset,
                int spectralStart,
                int spectralEnd,
                int approximationLow
            )
        {
            var positive = (short)(1 << approximationLow);
            var negative = (short)(-1 << approximationLow);

            var index = spectralStart;

            if (_eobRun <= 0)
            {
                var acTable = _acTables[component.AcTableId]
                                ?? throw new UnsupportedJpegException("Missing AC Huffman table.");

                while (index <= spectralEnd)
                {
                    var symbol = acTable.Decode(reader);

                    var run = symbol >> 4;
                    var size = symbol & 0x0F;

                    short newValue = 0;

                    if (size == 0)
                    {
                        if (run < 15)
                        {
                            _eobRun = (1 << run);

                            if (run > 0)
                                _eobRun += reader.ReadBits(run);

                            break;
                        }

                        // ZRL: skip 16 zero-valued coefficients.
                    }
                    else
                    {
                        newValue = reader.ReadBit() != 0 ? positive : negative;
                    }

                    while (index <= spectralEnd)
                    {
                        var position = offset + JpegImageData.ZigZagOrder[index];

                        if (component.Coefficients[position] != 0)
                        {
                            AppendCorrectionBit(reader, component, position, positive);
                        }
                        else
                        {
                            if (run == 0)
                            {
                                if (newValue != 0)
                                    component.Coefficients[position] = newValue;

                                index++;

                                break;
                            }

                            run--;
                        }

                        index++;
                    }
                }
            }

            if (_eobRun > 0)
            {
                // Inside an end-of-band run only the already non-zero coefficients
                // continue to receive correction bits.
                while (index <= spectralEnd)
                {
                    var position = offset + JpegImageData.ZigZagOrder[index];

                    if (component.Coefficients[position] != 0)
                        AppendCorrectionBit(reader, component, position, positive);

                    index++;
                }

                _eobRun--;
            }
        }

        private static void AppendCorrectionBit(
                JpegBitReader reader,
                JpegComponent component,
                int position,
                short positive
            )
        {
            if (reader.ReadBit() == 0)
                return;

            var current = component.Coefficients[position];

            if ((current & positive) != 0)
                return;

            component.Coefficients[position] = current >= 0
                                                    ? (short)(current + positive)
                                                    : (short)(current - positive);
        }
    }
}
