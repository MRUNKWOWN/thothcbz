using SixLabors.ImageSharp;
using ThothCbz.Constants;
using ThothCbz.Entities;
using ThothCbz.Enumerators;
using ThothCbz.Imaging.Jpeg;
using ThothCbz.Properties;

namespace ThothCbz.Extensions
{
    internal static class FileEntityExtensions
    {
        internal static string GetFilePathToImageOutputFileTypeValue(
                this FileEntity entity,
                string? uniqueIdentifier = null
            )
        {
            if(string.IsNullOrWhiteSpace(uniqueIdentifier) && entity.Extension == Settings.Default.ImageOutputFileType.GetImageOutputFileTypeExtension()) 
            {
                return entity.FilePath;
            }

            var splitNameChar = string.IsNullOrWhiteSpace(uniqueIdentifier)
                                    ? string.Empty
                                    : "-";

            return  entity.FilePath
                            .Replace($@"{entity.Name}{entity.Extension}", $@"{entity.Name}{splitNameChar}{uniqueIdentifier ?? string.Empty}{Settings.Default.ImageOutputFileType.GetImageOutputFileTypeExtension()}");
        }

        internal static void ReplaceOldFile(
                this FileEntity fileEntity,
                string uniqueIdentifier
            )
        {
            if (string.IsNullOrWhiteSpace(uniqueIdentifier) && fileEntity.Extension == Settings.Default.ImageOutputFileType.GetImageOutputFileTypeExtension())
            {
                return;
            }

            var newFilePath = fileEntity.GetFilePathToImageOutputFileTypeValue(uniqueIdentifier);

            if(!File.Exists(newFilePath))
            {
                throw new ArgumentException(newFilePath);
            }

            var wasDeleted = false;

            var currentPath = (fileEntity.Extension != Settings.Default.ImageOutputFileType.GetImageOutputFileTypeExtension() && fileEntity.FileWasAdjusted) || fileEntity.Extension == GlobalConstants.DEFAULT_WEBP_EXTENSION
                                ? fileEntity.GetFilePathToImageOutputFileTypeValue()
                                : fileEntity.FilePath;

            while (!wasDeleted)
            {
                try
                {
                    if (File.Exists(currentPath))
                    {
                        File.Delete(currentPath);
                    }

                    if (File.Exists(fileEntity.GetFilePathToImageOutputFileTypeValue()))
                    {
                        File.Delete(fileEntity.GetFilePathToImageOutputFileTypeValue());
                    }

                    wasDeleted = true;
                }
                catch
                {
                    Thread.Sleep(500);
                }
            }

            File.Move(
                    fileEntity.GetFilePathToImageOutputFileTypeValue(uniqueIdentifier),
                    fileEntity.GetFilePathToImageOutputFileTypeValue()
                );
        }

        internal static void DeleteFile(
                this FileEntity fileEntity
            )
        {
            var wasDeleted = false;

            var currentPath = (fileEntity.Extension != Settings.Default.ImageOutputFileType.GetImageOutputFileTypeExtension() && fileEntity.FileWasAdjusted) || fileEntity.Extension == GlobalConstants.DEFAULT_WEBP_EXTENSION
                                ? fileEntity.GetFilePathToImageOutputFileTypeValue()
                                : fileEntity.FilePath;

            while (!wasDeleted)
            {
                try
                {
                    if (File.Exists(currentPath))
                    {
                        File.Delete(currentPath);
                    }

                    wasDeleted = true;
                }
                catch
                {
                    Thread.Sleep(500);
                }
            }
        }

        internal static System.Drawing.Image GetImage(
                this FileEntity entity,
                System.Drawing.Color backgroundColor,
                ISet<string> filesToGrayscale,
                System.Drawing.Size? defaultSize = null
            )
        {
            var uniqueDirectoryIdentifier = Guid.NewGuid().ToString("N");
            var filePath = entity.FilePath;

            if (entity.Extension == GlobalConstants.DEFAULT_WEBP_EXTENSION)
            {
                filePath = entity.GetFilePathToImageOutputFileTypeValue();
                SaveWebpAsDefaultImageOutputFileType(
                            entity,
                            newFilePath: filePath
                        );
            }

            if((entity.Extension == GlobalConstants.DEFAULT_JPEG_EXTENSION || entity.Extension == GlobalConstants.DEFAULT_JPG_EXTENSION) && IsJpegFile(entity.FilePath))
            {
                var serieDirectory = Directory.GetParent(entity.SeriePath.Replace("|", "\\"))!.FullName;
                var newDirectory = $@"{serieDirectory}\{uniqueDirectoryIdentifier}";

                var newEntity = entity.Move(
                        newDirectory
                    );

                // The artifact removal always emits PNG data, so it needs a destination
                // that is distinct from its own input file, otherwise it would overwrite
                // the source while reading it and the next run would receive PNG bytes.
                var newfilePath = $@"{newDirectory}\{newEntity.Name}{GlobalConstants.DEFAULT_PNG_EXTENSION}";

                RemoveJpegArtifactsAndSaveAsDefaultImageOutputFileType(
                        newEntity,
                        newFilePath: newfilePath
                    );

                filePath = Path.Combine(Path.GetDirectoryName(entity.FilePath)!, Path.GetFileName(filePath));

                DeleteFileWithRetry(filePath);

                File.Move(
                        newfilePath,
                        filePath
                    );

                Directory.Delete(newDirectory, true);
            }

            var needSharpen = true;

            if (Settings.Default.CancelImageAdjustsIfSizeAndExtensionAreOK && entity.ExtensionOutputFileType == (ImageOutputFileType)Settings.Default.ImageOutputFileType)
            {
                using var imgSize = System.Drawing.Image.FromFile(filePath);

                needSharpen = (Settings.Default.EnableUpscale && imgSize.Height < Settings.Default.MinimalImageHeight);

                imgSize.Dispose();
            }

            if (needSharpen)
            {
                SharpenAndSaveAs(
                    entity,
                    filePath: filePath
                );
            }

            using var img = System.Drawing.Image.FromFile(filePath);

            using var imgRgb = img.NewImage(
                                    width: img.Width,
                                    height: img.Height,
                                    horizontalResolution: img.HorizontalResolution,
                                    verticalResolution: img.VerticalResolution,
                                    backgroundColor: backgroundColor,
                                    defaultSize: defaultSize
                                );

            img.Dispose();

            

            var wasDeleted = false;

            while (!wasDeleted)
            {
                if(!File.Exists(entity.FilePath))
                {
                    wasDeleted = true;
                    continue;
                }


                try
                {
                    File.Delete(entity.FilePath);
                    wasDeleted = true;
                }
                catch
                {

                }
            }

            imgRgb.SaveAs(
                    entity,
                    null
                );

            entity.IsGrayScaled = Settings.Default.EnableBrightnessAndContrastAdjustments && filesToGrayscale.Contains(entity.FilePath);

            return entity.IsGrayScaled
                    ? imgRgb.ApplyGrayscale()
                    : (System.Drawing.Bitmap)imgRgb.Clone();
        }

        internal static void SaveWebpAsDefaultImageOutputFileType(
                this FileEntity entity,
                string newFilePath
            )
        {
            using var img = SixLabors.ImageSharp.Image.Load(entity.FilePath);

            switch ((ImageOutputFileType)Settings.Default.ImageOutputFileType)
            {
                case ImageOutputFileType.JPG:
                    img.SaveAsJpeg(newFilePath);
                    break;
                case ImageOutputFileType.PNG:
                    img.SaveAsPng(newFilePath);
                    break;
                default:
                    throw new NotImplementedException();
            }
            
            img.Dispose();

            

            DeleteFileWithRetry(entity.FilePath);
        }

        internal static string FilesToGrayScaleFilePath(
                this IEnumerable<FileEntity> entities
            )
        {
            if (!entities.Any())
            {
                return string.Empty;
            }

            return $@"{entities.First().SeriePath.Replace("|", "\\")}\{GlobalConstants.DEFAULT_FILES_TO_GRAYSCALE_FILE_NAME}";
        }

        internal static string GetDirectoryFolder(
                this IEnumerable<FileEntity> entities,
                bool hasChapters = false
            )
        {
            if (!entities.Any())
            {
                return string.Empty;
            }

            var filePath = entities
                                .Where(w => !w.IsUnify && !w.IsSplit)
                                .FirstOrDefault()
                                ?.FilePath 
                                ?? entities.First().FilePath;

            return !hasChapters
                        ? entities.Where(w => !w.IsUnify && !w.IsSplit).Any()
                            ? Directory.GetParent(filePath)!.FullName
                            : Directory.GetParent(Directory.GetParent(filePath)!.FullName)!.FullName
                        : entities.Where(w => !w.IsUnify && !w.IsSplit).Any()
                            ? Directory.GetParent(Directory.GetParent(filePath)!.FullName)!.FullName
                            : Directory.GetParent(Directory.GetParent(Directory.GetParent(filePath)!.FullName)!.FullName)!.FullName; ;
        }

        /// <summary>
        /// The coefficient decoder needs a real JPEG payload to work with.
        /// Files can legitimately carry a .jpg name while already holding PNG data, so the
        /// magic bytes are checked instead of trusting the extension.
        /// </summary>
        private static bool IsJpegFile(
                string filePath
            )
        {
            if (!File.Exists(filePath))
                return false;

            var header = new byte[3];

            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

            if (stream.Read(header, 0, header.Length) < header.Length)
                return false;

            return header[0] == 0xFF && header[1] == 0xD8 && header[2] == 0xFF;
        }

        internal static void RemoveJpegArtifactsAndSaveAsDefaultImageOutputFileType(
                this FileEntity entity,
                string newFilePath
            )
        {
            try
            {
                Jpeg2PngConverter.Convert(
                        sourceFilePath: entity.FilePath,
                        destinationFilePath: newFilePath
                    );
            }
            catch (Exception exception)
            {
                throw new Exception($"Error converting JPEG to PNG: {exception.Message}", exception);
            }

            if (!File.Exists(newFilePath))
                throw new Exception($"Error converting JPEG to PNG: {newFilePath} was not produced.");

            DeleteFileWithRetry(entity.FilePath);
        }

        internal static void SharpenAndSaveAs(
                this FileEntity entity,
                string filePath
            )
        {
            var temporaryFilePath = $"{filePath}.{Guid.NewGuid():N}.tmp";

            using (var buffer = new MemoryStream(File.ReadAllBytes(filePath), writable: false))
            using (var loaded = new System.Drawing.Bitmap(buffer))
            using (var image = loaded.EnsureSharpenableFormat())
            {
                image.SharpenInPlace(GlobalConstants.DEFAULT_SHARPEN_SIGMA);

                image.SaveAsByExtension(
                        filePath: temporaryFilePath
                    );
            }

            DeleteFileWithRetry(filePath);

            File.Move(
                    temporaryFilePath,
                    filePath
                );
        }

        internal static FileEntity Move(
                this FileEntity entity,
                string directoryPath
            )
        {
            if(!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            var newFilePath = $@"{directoryPath}\{entity.Name}{entity.Extension}";

            File.Move(
                    entity.FilePath,
                    newFilePath
                );

            return new FileEntity(
                    filePath: newFilePath,
                    selectedFolderPath: ThothNotifyablePropertiesEntity.Default.DirectoryPathToAnalyze,
                    useSelectedFolderAsLevel: Settings.Default.UseSelectedFolderAsPartOfTheFileStructure,
                    splitFolderDefaultName: Settings.Default.DefaultSplitFolderName,
                    unifyFolderDefaultName: Settings.Default.DefaultUnifyFolderName
                );
        }

        private static void DeleteFileWithRetry(
                string filePath
            )
        {
            var wasDeleted = false;

            if (!File.Exists(filePath))
                return;

            while (!wasDeleted)
            {
                try
                {
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }

                    wasDeleted = true;
                }
                catch
                {
                    Thread.Sleep(500);
                }
            }
        }

        private static System.Drawing.Bitmap CreateBitmapCopy(System.Drawing.Bitmap source)
        {
            return new System.Drawing.Bitmap(source);
        }

        internal static System.Drawing.Bitmap ApplyGrayscale(this System.Drawing.Bitmap bitmap)
        {
            var result = CreateBitmapCopy(bitmap);
            var rect = new System.Drawing.Rectangle(0, 0, result.Width, result.Height);
            var data = result.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            try
            {
                unsafe
                {
                    byte* ptr = (byte*)data.Scan0;
                    for (var y = 0; y < result.Height; y++)
                    {
                        byte* row = ptr + (y * data.Stride);
                        for (var x = 0; x < result.Width; x++)
                        {
                            var blue = row[0];
                            var green = row[1];
                            var red = row[2];
                            var intensity = (byte)(0.299f * red + 0.587f * green + 0.114f * blue);

                            row[0] = intensity;
                            row[1] = intensity;
                            row[2] = intensity;
                            row += 4;
                        }
                    }
                }
            }
            finally
            {
                result.UnlockBits(data);
            }

            return result;
        }

        internal static System.Drawing.Bitmap ApplyLevelsLinear(this System.Drawing.Bitmap bitmap, int inRedMin, int inRedMax, int inGreenMin, int inGreenMax, int inBlueMin, int inBlueMax)
        {
            var result = CreateBitmapCopy(bitmap);
            var rect = new System.Drawing.Rectangle(0, 0, result.Width, result.Height);
            var data = result.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            try
            {
                unsafe
                {
                    byte* ptr = (byte*)data.Scan0;
                    for (var y = 0; y < result.Height; y++)
                    {
                        byte* row = ptr + (y * data.Stride);
                        for (var x = 0; x < result.Width; x++)
                        {
                            row[0] = ScaleChannel(row[0], inBlueMin, inBlueMax);
                            row[1] = ScaleChannel(row[1], inGreenMin, inGreenMax);
                            row[2] = ScaleChannel(row[2], inRedMin, inRedMax);
                            row += 4;
                        }
                    }
                }
            }
            finally
            {
                result.UnlockBits(data);
            }

            return result;
        }

        internal static System.Drawing.Bitmap ApplyContrastAndSaturation(this System.Drawing.Bitmap bitmap, float contrastFactor, float saturationFactor)
        {
            var result = CreateBitmapCopy(bitmap);
            var rect = new System.Drawing.Rectangle(0, 0, result.Width, result.Height);
            var data = result.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            try
            {
                unsafe
                {
                    byte* ptr = (byte*)data.Scan0;
                    for (var y = 0; y < result.Height; y++)
                    {
                        byte* row = ptr + (y * data.Stride);
                        for (var x = 0; x < result.Width; x++)
                        {
                            var blue = row[0];
                            var green = row[1];
                            var red = row[2];
                            var grayscale = (byte)(0.299f * red + 0.587f * green + 0.114f * blue);

                            row[0] = (byte)Math.Clamp(grayscale + (blue - grayscale) * (1f + saturationFactor), 0, 255);
                            row[1] = (byte)Math.Clamp(grayscale + (green - grayscale) * (1f + saturationFactor), 0, 255);
                            row[2] = (byte)Math.Clamp(grayscale + (red - grayscale) * (1f + saturationFactor), 0, 255);

                            if (contrastFactor != 1f)
                            {
                                row[0] = AdjustValue(row[0], contrastFactor);
                                row[1] = AdjustValue(row[1], contrastFactor);
                                row[2] = AdjustValue(row[2], contrastFactor);
                            }

                            row += 4;
                        }
                    }
                }
            }
            finally
            {
                result.UnlockBits(data);
            }

            return result;
        }

        private static byte ScaleChannel(byte value, int inputMin, int inputMax)
        {
            var clampedValue = Math.Clamp(value, inputMin, inputMax);
            return (byte)Math.Round((clampedValue - inputMin) * 255d / (inputMax - inputMin));
        }

        private static byte AdjustValue(byte value, float factor)
        {
            var adjustedValue = ((value - 128) * factor) + 128;
            return (byte)Math.Clamp(adjustedValue, 0, 255);
        }
    }
}




