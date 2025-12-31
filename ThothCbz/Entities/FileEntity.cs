using ThothCbz.Constants;
using ThothCbz.Extensions;
using ThothCbz.Properties;

namespace ThothCbz.Entities
{
    public class FileEntity
    {
        private List<string> _acceptableExtensions = new List<string>() { 
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
            if(string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            Name = Path.GetFileNameWithoutExtension(filePath);
            Extension = Path.GetExtension(filePath).ToLower();
            FilePath = filePath;
            IsAcceptableFileType = _acceptableExtensions.Contains(Extension);
            NeedConversion = _acceptableExtensions.Contains(Extension) && Extension != Settings.Default.ImageOutputFileType.GetImageOutputFileTypeExtension();
            FileWasAdjusted = false;

            var iCountFolderLevel = 0;
            var SeriesLevelFound = false;
            var currentPath = filePath;
            List<string> levelsName = new List<string>();
            List<string> paths = new List<string>();

            while (!SeriesLevelFound)
            {
                currentPath = Directory.GetParent(currentPath)!.FullName;
                var directoryName = new DirectoryInfo(currentPath).Name;
                var foundAdjustableFolder = false;

                if(string.IsNullOrWhiteSpace(directoryName))
                {
                    SeriesLevelFound = true;
                    continue;
                }

                if (directoryName.ToLower().Trim() == splitFolderDefaultName)
                {
                    foundAdjustableFolder = IsSplit = true;
                }
                
                if (directoryName.ToLower().Trim() == unifyFolderDefaultName)
                {
                    foundAdjustableFolder = IsUnify = true;
                }

                if(!foundAdjustableFolder) 
                {
                    if(selectedFolderPath.Trim().ToLower() == currentPath.ToLower())
                    {
                        if (useSelectedFolderAsLevel)
                        {
                            levelsName.Add(directoryName);
                            paths.Add(currentPath);
                            iCountFolderLevel++;
                        }

                        SeriesLevelFound = true;
                        continue;
                    }

                    levelsName.Add(directoryName);
                    paths.Add(currentPath);
                    iCountFolderLevel++;
                    SeriesLevelFound = iCountFolderLevel == 3;
                }
            }

            if (iCountFolderLevel == 0)
            {
                SeriePath = Serie = string.Empty;
                return;
            }

            SeriePath = paths[iCountFolderLevel - 1].Replace('\\', '|');
            Serie = levelsName[iCountFolderLevel - 1];
            Volume = (iCountFolderLevel - 2) >= 0 
                        ? levelsName[iCountFolderLevel - 2]
                        : string.Empty;
            Chapter = (iCountFolderLevel - 3) >= 0 
                        ? levelsName[iCountFolderLevel - 3]
                        : string.Empty;
        }

        public string Name { get; private set; }
        public string Extension { get; private set; }
        public string FilePath { get; private set; }
        public string SeriePath { get; private set; }
        public string Serie { get; private set; }
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
    }
}
