using ThothCbz.Constants;
using ThothCbz.Enumerators;
using ThothCbz.Extensions;
using ThothCbz.Properties;

namespace ThothCbz.Entities
{
    public class FileEntity
    {
        private static readonly HashSet<string> AcceptableExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            GlobalConstants.DEFAULT_JPG_EXTENSION,
            GlobalConstants.DEFAULT_JPEG_EXTENSION,
            GlobalConstants.DEFAULT_IMG_EXTENSION,
            GlobalConstants.DEFAULT_GIF_EXTENSION,
            GlobalConstants.DEFAULT_PNG_EXTENSION,
            GlobalConstants.DEFAULT_WEBP_EXTENSION,
            GlobalConstants.DEFAULT_AVIF_EXTENSION
        };

        public FileEntity(
                string filePath,
                string selectedFolderPath,
                bool useSelectedFolderAsLevel,
                string? splitFolderDefaultName,
                string? unifyFolderDefaultName
            )
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            Name = Path.GetFileNameWithoutExtension(filePath);
            Extension = NormalizeExtension(filePath);
            FilePath = filePath;
            IsAcceptableFileType = AcceptableExtensions.Contains(Extension);
            NeedConversion = IsAcceptableFileType && !string.Equals(Extension, Settings.Default.ImageOutputFileType.GetImageOutputFileTypeExtension(), StringComparison.OrdinalIgnoreCase);
            FileWasAdjusted = false;
            ExtensionOutputFileType = GetImageOutputFileType(Extension);

            var folderLevelCount = 0;
            var seriesLevelFound = false;
            var currentPath = filePath;
            List<string> levelNames = new List<string>();
            List<string> paths = new List<string>();

            while (!seriesLevelFound)
            {
                var parent = Directory.GetParent(currentPath);
                if (parent is null)
                {
                    break;
                }

                currentPath = parent.FullName;
                var directoryName = new DirectoryInfo(currentPath).Name;
                var foundAdjustableFolder = false;

                if (string.IsNullOrWhiteSpace(directoryName))
                {
                    seriesLevelFound = true;
                    continue;
                }

                var normalizedDirectoryName = directoryName.Trim();
                var normalizedSplitFolderDefaultName = splitFolderDefaultName?.Trim();
                var normalizedUnifyFolderDefaultName = unifyFolderDefaultName?.Trim();

                if (string.Equals(normalizedDirectoryName, normalizedSplitFolderDefaultName, StringComparison.OrdinalIgnoreCase))
                {
                    foundAdjustableFolder = IsSplit = true;
                }

                if (string.Equals(normalizedDirectoryName, normalizedUnifyFolderDefaultName, StringComparison.OrdinalIgnoreCase))
                {
                    foundAdjustableFolder = IsUnify = true;
                }

                if (!foundAdjustableFolder)
                {
                    var normalizedSelectedFolderPath = string.IsNullOrWhiteSpace(selectedFolderPath)
                        ? string.Empty
                        : Path.GetFullPath(selectedFolderPath);
                    var normalizedCurrentPath = Path.GetFullPath(currentPath);

                    if (string.Equals(normalizedSelectedFolderPath, normalizedCurrentPath, StringComparison.OrdinalIgnoreCase))
                    {
                        if (useSelectedFolderAsLevel)
                        {
                            levelNames.Add(directoryName);
                            paths.Add(currentPath);
                            folderLevelCount++;
                        }

                        seriesLevelFound = true;
                        continue;
                    }

                    levelNames.Add(directoryName);
                    paths.Add(currentPath);
                    folderLevelCount++;
                    seriesLevelFound = folderLevelCount == 3;
                }
            }

            if (folderLevelCount == 0)
            {
                SeriePath = string.Empty;
                Serie = string.Empty;
                return;
            }

            SeriePath = paths[folderLevelCount - 1].Replace('\\', '|');
            Serie = levelNames[folderLevelCount - 1];
            Volume = (folderLevelCount - 2) >= 0
                        ? levelNames[folderLevelCount - 2]
                        : string.Empty;
            Chapter = (folderLevelCount - 3) >= 0
                        ? levelNames[folderLevelCount - 3]
                        : string.Empty;
        }

        public string Name { get; private set; } = string.Empty;
        public string Extension { get; private set; } = string.Empty;
        internal ImageOutputFileType ExtensionOutputFileType { get; private set; }
        public string FilePath { get; private set; } = string.Empty;
        public string SeriePath { get; private set; } = string.Empty;
        public string Serie { get; private set; } = string.Empty;
        public string? Volume { get; private set; }
        public string? Chapter { get; private set; }
        public bool IsAcceptableFileType { get; private set; }
        public bool NeedConversion { get; private set; }
        public bool IsSplit { get; private set; }
        public bool IsUnify { get; private set; }
        public bool IsGrayScaled { get; set; } = false;
        public bool FileWasAdjusted { get; set; }
        public bool FileWasSplit { get; set; }
        public bool FileWasUnified { get; set; }
        public bool FileWasRenamed { get; set; }
        public bool FileWasCompressed { get; set; }

        private static string NormalizeExtension(string filePath)
        {
            var extension = Path.GetExtension(filePath);
            return string.IsNullOrWhiteSpace(extension)
                ? string.Empty
                : extension.ToLowerInvariant();
        }

        private static ImageOutputFileType GetImageOutputFileType(string extension)
        {
            return extension.ToLowerInvariant() switch
            {
                GlobalConstants.DEFAULT_JPEG_EXTENSION => ImageOutputFileType.JPEG,
                GlobalConstants.DEFAULT_JPG_EXTENSION => ImageOutputFileType.JPG,
                GlobalConstants.DEFAULT_PNG_EXTENSION => ImageOutputFileType.PNG,
                GlobalConstants.DEFAULT_IMG_EXTENSION => ImageOutputFileType.IMG,
                GlobalConstants.DEFAULT_GIF_EXTENSION => ImageOutputFileType.GIF,
                GlobalConstants.DEFAULT_AVIF_EXTENSION => ImageOutputFileType.AVIF,
                GlobalConstants.DEFAULT_WEBP_EXTENSION => ImageOutputFileType.WEBP,
                _ => ImageOutputFileType.INVALID
            };
        }
    }
}
