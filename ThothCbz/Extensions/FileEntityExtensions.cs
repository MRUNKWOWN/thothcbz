using AForge.Imaging.Filters;
using ImageMagick;
using SixLabors.ImageSharp;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using ThothCbz.Constants;
using ThothCbz.Entities;
using ThothCbz.Enumerators;
using ThothCbz.Properties;
using Windows.UI.ViewManagement;

namespace ThothCbz.Extensions
{
    internal static class FileEntityExtensions
    {
        internal static string _exeDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

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
                List<string> filesToGrayscale,
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

                filePath = Path.GetDirectoryName(entity.FilePath) + "\\" + Path.GetFileName(filePath);

                File.Move(
                        newfilePath,
                        filePath
                    );

                Directory.Delete(newDirectory, true);
            }

            SharpenAndSaveAs(
                    entity,
                    filePath: filePath
                );

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

            GC.Collect();
            GC.WaitForPendingFinalizers();

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
                    Thread.Sleep(500);
                }
            }

            imgRgb.SaveAs(
                    entity,
                    null
                );

            entity.IsGrayScaled = Settings.Default.EnableBrightnessAndContrastAdjustments && filesToGrayscale.Where(w => w == entity.FilePath).Any();

            return entity.IsGrayScaled
                    ? Grayscale.CommonAlgorithms.BT709.Apply(imgRgb)
                    : (Bitmap)imgRgb.Clone();
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

            GC.Collect();
            GC.WaitForPendingFinalizers();

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

            GC.Collect();
            GC.WaitForPendingFinalizers();

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
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = $"{_exeDirectory}\\jpeg2png_1.02_x64.exe",
                Arguments = $"\"{entity.FilePath}\" -o \"{newFilePath}\" -f -q",
                UseShellExecute = false, // Needed to redirect output
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using Process process = Process.Start(startInfo);

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
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = $"magick",
                Arguments = $"\"{filePath}\" -sharpen 0x3 \"{filePath}\"",
                UseShellExecute = false, // Needed to redirect output
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using Process process = Process.Start(startInfo);

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
    }
}
