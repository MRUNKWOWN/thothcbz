namespace ThothCbz.Imaging.Jpeg
{
    /// <summary>
    /// Decodes a JPEG down to its quantized DCT coefficients without ever
    /// reconstructing pixels. Keeping the coefficients and the quantization tables is
    /// what allows the solver to constrain its result to the original quantization
    /// bins, which is the source of jpeg2png's quality.
    /// </summary>
    internal sealed partial class JpegCoefficientDecoder
    {
        private readonly JpegMarkerReader _reader;
        private readonly JpegImageData _image = new();

        private readonly ushort[][] _quantizationTables = new ushort[4][];
        private readonly HuffmanTable?[] _dcTables = new HuffmanTable?[4];
        private readonly HuffmanTable?[] _acTables = new HuffmanTable?[4];

        private int _restartInterval;
        private bool _frameParsed;

        private JpegCoefficientDecoder(
                byte[] data
            )
        {
            _reader = new JpegMarkerReader(data);
        }

        internal static JpegImageData Decode(
                byte[] data
            )
        {
            var decoder = new JpegCoefficientDecoder(data);

            return decoder.Run();
        }

        private JpegImageData Run()
        {
            if (!_reader.TryReadStartOfImage())
                throw new UnsupportedJpegException("Missing JPEG SOI marker.");

            while (_reader.TryReadMarker(out var marker))
            {
                if (marker == JpegMarkerReader.MARKER_EOI)
                    break;

                if (marker == JpegMarkerReader.MARKER_SOI || JpegMarkerReader.IsRestartMarker(marker))
                    continue;

                if (JpegMarkerReader.IsStartOfFrame(marker))
                {
                    JpegMarkerReader.EnsureSupportedFrame(marker);

                    ReadFrameHeader(marker);

                    continue;
                }

                switch (marker)
                {
                    case JpegMarkerReader.MARKER_DQT:
                        ReadQuantizationTables();
                        break;

                    case JpegMarkerReader.MARKER_DHT:
                        ReadHuffmanTables();
                        break;

                    case JpegMarkerReader.MARKER_DRI:
                        ReadRestartInterval();
                        break;

                    case JpegMarkerReader.MARKER_SOS:
                        ReadScan();
                        break;

                    default:
                        _reader.SkipSegment();
                        break;
                }
            }

            if (!_frameParsed)
                throw new UnsupportedJpegException("No JPEG frame header found.");

            AttachQuantizationTables();

            return _image;
        }

        private void ReadQuantizationTables()
        {
            var segment = _reader.ReadSegment();
            var offset = 0;

            while (offset < segment.Length)
            {
                var specification = segment[offset++];
                var precision = specification >> 4;
                var id = specification & 0x0F;

                if (id > 3)
                    throw new UnsupportedJpegException("Invalid quantization table id.");

                var table = new ushort[64];

                for (var i = 0; i < 64; i++)
                {
                    if (precision == 0)
                    {
                        table[JpegImageData.ZigZagOrder[i]] = segment[offset++];
                    }
                    else
                    {
                        table[JpegImageData.ZigZagOrder[i]] = (ushort)((segment[offset] << 8) | segment[offset + 1]);
                        offset += 2;
                    }
                }

                _quantizationTables[id] = table;
            }
        }

        private void ReadHuffmanTables()
        {
            var segment = _reader.ReadSegment();
            var offset = 0;

            while (offset < segment.Length)
            {
                var specification = segment[offset++];
                var tableClass = specification >> 4;
                var id = specification & 0x0F;

                if (id > 3)
                    throw new UnsupportedJpegException("Invalid Huffman table id.");

                var counts = segment.Slice(offset, 16);
                offset += 16;

                var total = 0;

                for (var i = 0; i < 16; i++)
                    total += counts[i];

                var values = segment.Slice(offset, total).ToArray();
                offset += total;

                var table = HuffmanTable.Build(counts, values);

                if (tableClass == 0)
                    _dcTables[id] = table;
                else
                    _acTables[id] = table;
            }
        }

        private void ReadRestartInterval()
        {
            var segment = _reader.ReadSegment();

            _restartInterval = (segment[0] << 8) | segment[1];
        }

        private void ReadFrameHeader(
                byte marker
            )
        {
            var segment = _reader.ReadSegment();

            var precision = segment[0];

            if (precision != 8)
                throw new UnsupportedJpegException($"Unsupported sample precision {precision}.");

            _image.IsProgressive = marker == JpegMarkerReader.MARKER_SOF2;
            _image.Height = (segment[1] << 8) | segment[2];
            _image.Width = (segment[3] << 8) | segment[4];

            var componentCount = segment[5];

            if (_image.Width <= 0 || _image.Height <= 0)
                throw new UnsupportedJpegException("Invalid JPEG dimensions.");

            if (componentCount is not (1 or 3))
                throw new UnsupportedJpegException($"Unsupported component count {componentCount} (CMYK/YCCK not handled).");

            var offset = 6;

            for (var i = 0; i < componentCount; i++)
            {
                var component = new JpegComponent
                {
                    Id = segment[offset],
                    HorizontalSamplingFactor = segment[offset + 1] >> 4,
                    VerticalSamplingFactor = segment[offset + 1] & 0x0F,
                    QuantizationTableId = segment[offset + 2]
                };

                if (component.HorizontalSamplingFactor is < 1 or > 4 || component.VerticalSamplingFactor is < 1 or > 4)
                    throw new UnsupportedJpegException("Unsupported sampling factors.");

                _image.Components.Add(component);

                offset += 3;
            }

            _image.MaxHorizontalSamplingFactor = _image.Components.Max(m => m.HorizontalSamplingFactor);
            _image.MaxVerticalSamplingFactor = _image.Components.Max(m => m.VerticalSamplingFactor);

            _image.McusPerLine = (int)Math.Ceiling(_image.Width / (8.0 * _image.MaxHorizontalSamplingFactor));
            _image.McusPerColumn = (int)Math.Ceiling(_image.Height / (8.0 * _image.MaxVerticalSamplingFactor));

            foreach (var component in _image.Components)
            {
                component.Width = (int)Math.Ceiling(_image.Width * component.HorizontalSamplingFactor / (double)_image.MaxHorizontalSamplingFactor);
                component.Height = (int)Math.Ceiling(_image.Height * component.VerticalSamplingFactor / (double)_image.MaxVerticalSamplingFactor);

                // Allocate on MCU boundaries so edge MCUs always have full blocks.
                component.BlocksPerLine = _image.McusPerLine * component.HorizontalSamplingFactor;
                component.BlocksPerColumn = _image.McusPerColumn * component.VerticalSamplingFactor;

                component.Coefficients = new short[component.BlocksPerLine * component.BlocksPerColumn * 64];
            }

            _frameParsed = true;
        }

        private void AttachQuantizationTables()
        {
            foreach (var component in _image.Components)
            {
                var table = _quantizationTables[component.QuantizationTableId]
                                ?? throw new UnsupportedJpegException($"Missing quantization table {component.QuantizationTableId}.");

                component.QuantizationTable = table;
            }
        }

        /// <summary>
        /// Decodes the components taking part in one scan. Baseline frames carry a
        /// single scan covering every component; progressive frames use many partial
        /// scans that accumulate into the same coefficient arrays.
        /// </summary>
        private void ReadScan()
        {
            if (!_frameParsed)
                throw new UnsupportedJpegException("Scan encountered before frame header.");

            var segment = _reader.ReadSegment();

            var scanComponentCount = segment[0];
            var scanComponents = new List<JpegComponent>(scanComponentCount);

            var offset = 1;

            for (var i = 0; i < scanComponentCount; i++)
            {
                var componentId = segment[offset];

                var component = _image.Components.FirstOrDefault(f => f.Id == componentId)
                                    ?? throw new UnsupportedJpegException($"Scan references unknown component {componentId}.");

                component.DcTableId = segment[offset + 1] >> 4;
                component.AcTableId = segment[offset + 1] & 0x0F;

                scanComponents.Add(component);

                offset += 2;
            }

            var spectralStart = segment[offset];
            var spectralEnd = segment[offset + 1];
            var approximationHigh = segment[offset + 2] >> 4;
            var approximationLow = segment[offset + 2] & 0x0F;

            var bitReader = new JpegBitReader(_reader.Data, _reader.Position);

            foreach (var component in scanComponents)
                component.DcPredictor = 0;

            _eobRun = 0;

            DecodeScanData(
                    bitReader,
                    scanComponents,
                    spectralStart,
                    spectralEnd,
                    approximationHigh,
                    approximationLow
                );

            _reader.Position = bitReader.Position;
        }

        private void DecodeScanData(
                JpegBitReader reader,
                List<JpegComponent> scanComponents,
                int spectralStart,
                int spectralEnd,
                int approximationHigh,
                int approximationLow
            )
        {
            // A non-interleaved scan walks the component's own block grid; an
            // interleaved scan walks MCUs made of several blocks per component.
            var isInterleaved = scanComponents.Count > 1;

            var unitsPerLine = isInterleaved
                                    ? _image.McusPerLine
                                    : (int)Math.Ceiling(scanComponents[0].Width / 8.0);

            var unitsPerColumn = isInterleaved
                                    ? _image.McusPerColumn
                                    : (int)Math.Ceiling(scanComponents[0].Height / 8.0);

            var totalUnits = unitsPerLine * unitsPerColumn;
            var restartInterval = _restartInterval > 0 ? _restartInterval : totalUnits;

            var unit = 0;

            while (unit < totalUnits)
            {
                var unitsThisInterval = Math.Min(restartInterval, totalUnits - unit);

                for (var i = 0; i < unitsThisInterval; i++, unit++)
                {
                    if (isInterleaved)
                    {
                        var mcuRow = unit / unitsPerLine;
                        var mcuColumn = unit % unitsPerLine;

                        foreach (var component in scanComponents)
                        {
                            for (var v = 0; v < component.VerticalSamplingFactor; v++)
                            {
                                for (var h = 0; h < component.HorizontalSamplingFactor; h++)
                                {
                                    var blockRow = (mcuRow * component.VerticalSamplingFactor) + v;
                                    var blockColumn = (mcuColumn * component.HorizontalSamplingFactor) + h;

                                    DecodeBlock(
                                            reader,
                                            component,
                                            blockRow,
                                            blockColumn,
                                            spectralStart,
                                            spectralEnd,
                                            approximationHigh,
                                            approximationLow
                                        );
                                }
                            }
                        }
                    }
                    else
                    {
                        var component = scanComponents[0];

                        var blockRow = unit / unitsPerLine;
                        var blockColumn = unit % unitsPerLine;

                        DecodeBlock(
                                reader,
                                component,
                                blockRow,
                                blockColumn,
                                spectralStart,
                                spectralEnd,
                                approximationHigh,
                                approximationLow
                            );
                    }
                }

                if (unit < totalUnits)
                {
                    reader.SkipRestartMarker();

                    foreach (var component in scanComponents)
                        component.DcPredictor = 0;

                    _eobRun = 0;
                }
            }
        }

        private void DecodeBlock(
                JpegBitReader reader,
                JpegComponent component,
                int blockRow,
                int blockColumn,
                int spectralStart,
                int spectralEnd,
                int approximationHigh,
                int approximationLow
            )
        {
            if (blockRow >= component.BlocksPerColumn || blockColumn >= component.BlocksPerLine)
                return;

            var offset = component.CoefficientOffset(blockRow, blockColumn);

            if (!_image.IsProgressive)
            {
                DecodeBaselineBlock(reader, component, offset);

                return;
            }

            DecodeProgressiveBlock(
                    reader,
                    component,
                    offset,
                    spectralStart,
                    spectralEnd,
                    approximationHigh,
                    approximationLow
                );
        }

        /// <summary>
        /// Baseline block: one DC difference followed by run-length coded AC values.
        /// </summary>
        private void DecodeBaselineBlock(
                JpegBitReader reader,
                JpegComponent component,
                int offset
            )
        {
            var dcTable = _dcTables[component.DcTableId]
                            ?? throw new UnsupportedJpegException("Missing DC Huffman table.");

            var acTable = _acTables[component.AcTableId]
                            ?? throw new UnsupportedJpegException("Missing AC Huffman table.");

            var magnitude = dcTable.Decode(reader);
            var difference = magnitude == 0 ? 0 : JpegBitReader.Extend(reader.ReadBits(magnitude), magnitude);

            component.DcPredictor += difference;

            component.Coefficients[offset] = (short)component.DcPredictor;

            var index = 1;

            while (index < 64)
            {
                var symbol = acTable.Decode(reader);

                var run = symbol >> 4;
                var size = symbol & 0x0F;

                if (size == 0)
                {
                    if (run != 15)
                        break;

                    index += 16;

                    continue;
                }

                index += run;

                if (index > 63)
                    break;

                var value = JpegBitReader.Extend(reader.ReadBits(size), size);

                component.Coefficients[offset + JpegImageData.ZigZagOrder[index]] = (short)value;

                index++;
            }
        }

        /// <summary>
        /// End-of-band run shared across blocks of a progressive AC scan.
        /// </summary>
        private int _eobRun;
    }
}
