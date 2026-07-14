using ImageMagick;
using SixLabors.ImageSharp;
using System.Diagnostics;
using System.Reflection;
using ThothCbz.Constants;
using ThothCbz.Entities;
using ThothCbz.Enumerators;
using ThothCbz.Properties;

namespace ThothCbz.Extensions
{
    internal static class FileEntityExtensions
    {
        internal static string _exeDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly()!.Location)!;

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

            var currentPath = (fileEntity.Extension != Settings.Default.ImageOutputFileType.GetImageOutputFileTypeExtension() && fileEntity.FileWasAdjusted) || fileEntity.Extension == GlobalConstants.DEFAULT_WEBP_EXTENSION || fileEntity.Extension == GlobalConstants.DEFAULT_AVIF_EXTENSION
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

            var currentPath = (fileEntity.Extension != Settings.Default.ImageOutputFileType.GetImageOutputFileTypeExtension() && fileEntity.FileWasAdjusted) || fileEntity.Extension == GlobalConstants.DEFAULT_WEBP_EXTENSION || fileEntity.Extension == GlobalConstants.DEFAULT_AVIF_EXTENSION
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

            if (entity.Extension == GlobalConstants.DEFAULT_AVIF_EXTENSION)
            {
                filePath = entity.GetFilePathToImageOutputFileTypeValue();
                SaveAvifAsDefaultImageOutputFileType(
                            entity,
                            newFilePath: filePath
                        );
            }

            if(entity.Extension == GlobalConstants.DEFAULT_JPEG_EXTENSION || entity.Extension == GlobalConstants.DEFAULT_JPG_EXTENSION)
            {
                var serieDirectory = Directory.GetParent(entity.SeriePath.Replace("|", "\\"))!.FullName;
                var newDirectory = $@"{serieDirectory}\{uniqueDirectoryIdentifier}";

                var newEntity = entity.Move(
                        newDirectory
                    );

                var newfilePath = newEntity.GetFilePathToImageOutputFileTypeValue();

                RemoveJpegArtifactsAndSaveAsDefaultImageOutputFileType(
                        newEntity,
                        newFilePath: newfilePath
                    );

                filePath = Path.Combine(Path.GetDirectoryName(entity.FilePath)!, Path.GetFileName(filePath));

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

        internal static void SaveAvifAsDefaultImageOutputFileType(
                this FileEntity entity,
                string newFilePath
            )
        {
            using var img = new MagickImage(entity.FilePath);

            switch ((ImageOutputFileType)Settings.Default.ImageOutputFileType)
            {
                case ImageOutputFileType.JPG:
                    img.Format = MagickFormat.Jpg;
                    break;
                case ImageOutputFileType.PNG:
                    img.Format = MagickFormat.Png;
                    break;
                default:
                    throw new NotImplementedException();
            }

            img.Write(newFilePath);
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

        internal static void RemoveJpegArtifactsAndSaveAsDefaultImageOutputFileType(
                this FileEntity entity,
                string newFilePath
            )
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = $"{_exeDirectory}\\jpeg2png_1.02_x64.exe",
                Arguments = $"\"{entity.FilePath}\" -o \"{newFilePath}\" -f -q",
                UseShellExecute = false, // Needed to redirect output
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using Process process = Process.Start(startInfo)!;

            string error = process.StandardError.ReadToEnd();

            process.WaitForExit();

            if (!string.IsNullOrEmpty(error))
                throw new Exception($"Error converting JPEG to PNG: {error}");

            DeleteFileWithRetry(entity.FilePath);
        }

        internal static void SharpenAndSaveAs(
                this FileEntity entity,
                string filePath
            )
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = $"magick",
                Arguments = $"\"{filePath}\" -sharpen 0x3 \"{filePath}\"",
                UseShellExecute = false, // Needed to redirect output
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using Process process = Process.Start(startInfo)!;

            string error = process.StandardError.ReadToEnd();

            process.WaitForExit();

            if (!string.IsNullOrEmpty(error))
                throw new Exception($"Error sharpening image: {error}");
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




