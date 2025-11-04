namespace ThothCbz.Constants
{
    public static class GlobalConstants
    {
        public static System.Drawing.Color DEFAULT_BACKGROUND_COLOR { get { return System.Drawing.Color.FromArgb(49, 59, 68); } }
        public static System.Drawing.Color DEFAULT_ENABLED_TEXT_COLOR { get { return System.Drawing.Color.FromArgb(250, 179, 14); } }
        public static System.Drawing.Color DEFAULT_DISABLED_TEXT_COLOR { get { return System.Drawing.Color.FromArgb(99, 109, 118); } }
        public static System.Drawing.Color DEFAULT_LOG_BACKGROUND_COLOR { get { return System.Drawing.Color.FromArgb(66, 75, 83); } }
        public static System.Drawing.Color DEFAULT_LOG_ERRO_TEXT_COLOR { get { return System.Drawing.Color.FromArgb(255, 0, 0); } }
        public static System.Drawing.Color DEFAULT_LOG_RUNNING_TEXT_COLOR { get { return System.Drawing.Color.FromArgb(255, 246, 0); } }
        public static System.Drawing.Color DEFAULT_LOG_DONE_TEXT_COLOR { get { return System.Drawing.Color.FromArgb(12, 255, 0); } }
        public static System.Drawing.Color DEFAULT_LOG_QUEUE_TEXT_COLOR { get { return System.Drawing.Color.FromArgb(154, 154, 154); } }
        public static System.Drawing.Color DEFAULT_LOG_WARNING_TEXT_COLOR { get { return System.Drawing.Color.FromArgb(255, 154, 154); } }

        public const string DEFAULT_EMPTY_STATISTIC_VALUE = "-- / --";
        public const string DEFAULT_JPEG_MIME_TYPE = "image/jpeg";
        public const string DEFAULT_WEBP_EXTENSION = ".webp";
        public const string DEFAULT_PNG_EXTENSION = ".png";
        public const string DEFAULT_JPG_EXTENSION = ".jpg";
        public const string DEFAULT_JPEG_EXTENSION = ".jpeg";
        public const string DEFAULT_IMG_EXTENSION = ".img";
        public const string DEFAULT_GIF_EXTENSION = ".gif";
        public const string DEFAULT_SPLITED_FILE_ORDER_01 = "1";
        public const string DEFAULT_SPLITED_FILE_ORDER_02 = "2";
        public const string DEFAULT_FILES_TO_GRAYSCALE_FILE_NAME = "file_to_grayscale.txt";
        public const string DEFAULT_BLANK_FILE_NAME = "_blank";
        public const string DEFAULT_TEMPLATE_FILE_NAME = "_template";
        public const string DEFAULT_LOG_FILE_NAME = "_log.txt";

        public const char DEFAULT_CHARACTER_FOR_PADDING_FOR_FOLDER = '0';
        public const char DEFAULT_CHARACTER_FOR_PADDING_FOR_FILES = '0';

        public const int DEFAULT_CHARACTERS_AMOUNT_FOR_FOLDERS = 3;
        public const int PIXEL_FORMAT_32BPP_CMYK = 0x200F;
        public const int MAXIMUM_DISTINCT_COLORS_FOR_GRAYSCALE = 32768;
    }
}
