namespace ThothCbz.Imaging.Jpeg
{
    /// <summary>
    /// Raised when a JPEG payload uses a feature the managed coefficient decoder
    /// cannot handle (arithmetic coding, 12-bit samples, hierarchical mode, ...).
    /// Callers are expected to fall back to a plain decode.
    /// </summary>
    internal sealed class UnsupportedJpegException : Exception
    {
        internal UnsupportedJpegException(string message) : base(message)
        {
        }
    }

    /// <summary>
    /// Sequentially walks the marker structure of a JPEG file. This only deals with
    /// the segment framing; entropy coded data is handed over to
    /// <see cref="JpegCoefficientDecoder"/>.
    /// </summary>
    internal sealed class JpegMarkerReader
    {
        internal const byte MARKER_PREFIX = 0xFF;
        internal const byte MARKER_SOF0 = 0xC0;
        internal const byte MARKER_SOF1 = 0xC1;
        internal const byte MARKER_SOF2 = 0xC2;
        internal const byte MARKER_SOF3 = 0xC3;
        internal const byte MARKER_DHT = 0xC4;
        internal const byte MARKER_SOF5 = 0xC5;
        internal const byte MARKER_SOF9 = 0xC9;
        internal const byte MARKER_SOF10 = 0xCA;
        internal const byte MARKER_SOI = 0xD8;
        internal const byte MARKER_EOI = 0xD9;
        internal const byte MARKER_SOS = 0xDA;
        internal const byte MARKER_DQT = 0xDB;
        internal const byte MARKER_DRI = 0xDD;
        internal const byte MARKER_APP0 = 0xE0;
        internal const byte MARKER_APP14 = 0xEE;
        internal const byte MARKER_COM = 0xFE;

        private readonly byte[] _data;
        private int _position;

        internal JpegMarkerReader(
                byte[] data
            )
        {
            _data = data;
            _position = 0;
        }

        internal byte[] Data => _data;

        internal int Position
        {
            get => _position;
            set => _position = value;
        }

        internal bool TryReadStartOfImage()
        {
            if (_data.Length < 2 || _data[0] != MARKER_PREFIX || _data[1] != MARKER_SOI)
                return false;

            _position = 2;

            return true;
        }

        /// <summary>
        /// Advances to the next marker, skipping any fill bytes. Returns false when the
        /// stream is exhausted.
        /// </summary>
        internal bool TryReadMarker(
                out byte marker
            )
        {
            marker = 0;

            while (_position < _data.Length && _data[_position] != MARKER_PREFIX)
                _position++;

            while (_position < _data.Length && _data[_position] == MARKER_PREFIX)
                _position++;

            if (_position >= _data.Length)
                return false;

            marker = _data[_position];
            _position++;

            return true;
        }

        /// <summary>
        /// Reads the length-prefixed payload of the current segment, leaving the
        /// position just after it.
        /// </summary>
        internal ReadOnlySpan<byte> ReadSegment()
        {
            if (_position + 2 > _data.Length)
                throw new UnsupportedJpegException("Truncated JPEG segment header.");

            var length = (_data[_position] << 8) | _data[_position + 1];

            if (length < 2 || _position + length > _data.Length)
                throw new UnsupportedJpegException("Invalid JPEG segment length.");

            var payload = new ReadOnlySpan<byte>(_data, _position + 2, length - 2);

            _position += length;

            return payload;
        }

        internal void SkipSegment()
        {
            ReadSegment();
        }

        internal static bool IsStartOfFrame(
                byte marker
            )
        {
            return marker is MARKER_SOF0 or MARKER_SOF1 or MARKER_SOF2 or MARKER_SOF3
                    or MARKER_SOF5 or 0xC6 or 0xC7
                    or MARKER_SOF9 or MARKER_SOF10 or 0xCB or 0xCD or 0xCE or 0xCF;
        }

        internal static bool IsRestartMarker(
                byte marker
            )
        {
            return marker >= 0xD0 && marker <= 0xD7;
        }

        /// <summary>
        /// Rejects frame types the coefficient decoder does not implement. Only
        /// baseline (SOF0), extended sequential (SOF1) and progressive (SOF2)
        /// Huffman-coded frames are supported.
        /// </summary>
        internal static void EnsureSupportedFrame(
                byte marker
            )
        {
            switch (marker)
            {
                case MARKER_SOF0:
                case MARKER_SOF1:
                case MARKER_SOF2:
                    return;

                case MARKER_SOF3:
                case MARKER_SOF5:
                case 0xC6:
                case 0xC7:
                    throw new UnsupportedJpegException("Lossless or hierarchical JPEG is not supported.");

                default:
                    throw new UnsupportedJpegException($"Unsupported JPEG frame type 0x{marker:X2} (arithmetic coding).");
            }
        }
    }
}
