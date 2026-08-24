namespace ThothCbz.Imaging.Jpeg
{
    /// <summary>
    /// Canonical Huffman decoding table built from a DHT segment.
    /// </summary>
    internal sealed class HuffmanTable
    {
        private readonly int[] _minCode = new int[17];
        private readonly int[] _maxCode = new int[17];
        private readonly int[] _valuePointer = new int[17];
        private readonly byte[] _values;

        private HuffmanTable(
                byte[] values
            )
        {
            _values = values;
        }

        /// <summary>
        /// Builds a table from the 16 code-length counts followed by the symbol values,
        /// following the canonical construction in the JPEG specification (F.2.2.3).
        /// </summary>
        internal static HuffmanTable Build(
                ReadOnlySpan<byte> counts,
                byte[] values
            )
        {
            var table = new HuffmanTable(values);

            Span<int> code = stackalloc int[17];

            var currentCode = 0;
            var valueIndex = 0;

            for (var length = 1; length <= 16; length++)
            {
                var count = counts[length - 1];

                table._valuePointer[length] = valueIndex;
                code[length] = currentCode;

                if (count == 0)
                {
                    // No codes of this length: mark the range as empty.
                    table._minCode[length] = 1;
                    table._maxCode[length] = 0;
                }
                else
                {
                    table._minCode[length] = currentCode;
                    table._maxCode[length] = currentCode + count - 1;

                    currentCode += count;
                    valueIndex += count;
                }

                currentCode <<= 1;
            }

            return table;
        }

        /// <summary>
        /// Decodes a single symbol by growing the code one bit at a time until it falls
        /// inside the range for that length.
        /// </summary>
        internal byte Decode(
                JpegBitReader reader
            )
        {
            var code = reader.ReadBit();

            for (var length = 1; length <= 16; length++)
            {
                if (_maxCode[length] >= _minCode[length] && code <= _maxCode[length] && code >= _minCode[length])
                {
                    var index = _valuePointer[length] + (code - _minCode[length]);

                    if (index < 0 || index >= _values.Length)
                        throw new UnsupportedJpegException("Corrupt Huffman table index.");

                    return _values[index];
                }

                code = (code << 1) | reader.ReadBit();
            }

            throw new UnsupportedJpegException("Invalid Huffman code encountered.");
        }
    }

    /// <summary>
    /// MSB-first bit reader over entropy coded JPEG data. Handles 0xFF00 byte stuffing
    /// and stops at markers, which lets the scan decoder detect restart intervals.
    /// </summary>
    internal sealed class JpegBitReader
    {
        private readonly byte[] _data;
        private int _position;
        private int _bitBuffer;
        private int _bitCount;
        private bool _markerReached;

        internal JpegBitReader(
                byte[] data,
                int position
            )
        {
            _data = data;
            _position = position;
        }

        internal int Position => _position;
        internal bool MarkerReached => _markerReached;

        internal int ReadBit()
        {
            if (_bitCount == 0)
            {
                if (!FillByte())
                    return 0;
            }

            _bitCount--;

            return (_bitBuffer >> _bitCount) & 1;
        }

        internal int ReadBits(
                int count
            )
        {
            var result = 0;

            for (var i = 0; i < count; i++)
                result = (result << 1) | ReadBit();

            return result;
        }

        private bool FillByte()
        {
            if (_position >= _data.Length)
            {
                _markerReached = true;

                return false;
            }

            var value = _data[_position];

            if (value == JpegMarkerReader.MARKER_PREFIX)
            {
                var next = _position + 1 < _data.Length ? _data[_position + 1] : (byte)JpegMarkerReader.MARKER_EOI;

                if (next == 0x00)
                {
                    // Stuffed byte: the 0x00 is padding and must be discarded.
                    _position += 2;
                }
                else if (next == JpegMarkerReader.MARKER_PREFIX)
                {
                    _position++;

                    return FillByte();
                }
                else
                {
                    _markerReached = true;

                    return false;
                }
            }
            else
            {
                _position++;
            }

            _bitBuffer = value;
            _bitCount = 8;

            return true;
        }

        /// <summary>
        /// Drops any partially consumed byte so the next read starts on a byte boundary,
        /// as required after a restart marker.
        /// </summary>
        internal void AlignToByte()
        {
            _bitCount = 0;
        }

        /// <summary>
        /// Skips an expected RSTn marker at the current position.
        /// </summary>
        internal void SkipRestartMarker()
        {
            AlignToByte();

            while (_position + 1 < _data.Length)
            {
                if (_data[_position] == JpegMarkerReader.MARKER_PREFIX && JpegMarkerReader.IsRestartMarker(_data[_position + 1]))
                {
                    _position += 2;
                    _markerReached = false;

                    return;
                }

                _position++;
            }

            _markerReached = true;
        }

        /// <summary>
        /// Extends a raw magnitude-coded value to its signed representation (JPEG F.2.2.1).
        /// </summary>
        internal static int Extend(
                int value,
                int magnitude
            )
        {
            if (magnitude == 0)
                return 0;

            return value < (1 << (magnitude - 1))
                        ? value - (1 << magnitude) + 1
                        : value;
        }
    }
}
