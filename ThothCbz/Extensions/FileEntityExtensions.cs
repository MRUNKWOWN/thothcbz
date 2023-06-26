using AForge.Imaging.Filters;

using System.Drawing.Imaging;
using System.Linq;

using ThothCbz.Constants;
using ThothCbz.Entities;
using ThothCbz.Properties;

namespace ThothCbz.Extensions
{
    internal static class FileEntityExtensions
    {
        internal static string GetFilePathToJpgValue(
                this FileEntity entity,
                string? uniqueIdentifier = null
            )
        {
            if(string.IsNullOrWhiteSpace(uniqueIdentifier) && entity.Extension == GlobalConstants.DEFAULT_JPG_EXTENSION) 
            {
                return entity.FilePath;
            }

            var splitNameChar = string.IsNullOrWhiteSpace(uniqueIdentifier)
                                    ? string.Empty
                                    : "-";

            return  entity.FilePath
                            .Replace($@"{entity.Name}{entity.Extension}", $@"{entity.Name}{splitNameChar}{uniqueIdentifier ?? string.Empty}{GlobalConstants.DEFAULT_JPG_EXTENSION}");
        }

        internal static void ReplaceOldFile(
                this FileEntity fileEntity,
                string uniqueIdentifier
            )
        {
            if (string.IsNullOrWhiteSpace(uniqueIdentifier) && fileEntity.Extension == GlobalConstants.DEFAULT_JPG_EXTENSION)
            {
                return;
            }

            var newFilePath = fileEntity.GetFilePathToJpgValue(uniqueIdentifier);

            if(!File.Exists(newFilePath))
            {
                throw new ArgumentException(newFilePath);
            }

            var wasDeleted = false;

            var currentPath = (fileEntity.Extension != GlobalConstants.DEFAULT_JPG_EXTENSION && fileEntity.FileWasAdjusted) || fileEntity.Extension == GlobalConstants.DEFAULT_WEBP_EXTENSION
                                ? fileEntity.GetFilePathToJpgValue()
                                : fileEntity.FilePath;

            while (!wasDeleted)
            {
                try
                {
                    if (File.Exists(currentPath))
                    {
                        File.Delete(currentPath);
                    }

                    if (File.Exists(fileEntity.GetFilePathToJpgValue()))
                    {
                        File.Delete(fileEntity.GetFilePathToJpgValue());
                    }

                    wasDeleted = true;
                }
                catch
                {
                    Thread.Sleep(500);
                }
            }

            File.Move(
                    fileEntity.GetFilePathToJpgValue(uniqueIdentifier),
                    fileEntity.GetFilePathToJpgValue()
                );
        }

        internal static void DeleteFile(
                this FileEntity fileEntity
            )
        {
            var wasDeleted = false;

            var currentPath = (fileEntity.Extension != GlobalConstants.DEFAULT_JPG_EXTENSION && fileEntity.FileWasAdjusted) || fileEntity.Extension == GlobalConstants.DEFAULT_WEBP_EXTENSION
                                ? fileEntity.GetFilePathToJpgValue()
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
            var filePath = entity.FilePath;

            if (entity.Extension == GlobalConstants.DEFAULT_WEBP_EXTENSION)
            {
                filePath = entity.GetFilePathToJpgValue();
                SaveWebpAsJpeg(
                            entity,
                            newFilePath: filePath
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

            GC.Collect();
            GC.WaitForPendingFinalizers();

            var wasDeleted = false;

            while (!wasDeleted)
            {
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

            imgRgb.SaveAsJpg(
                    entity,
                    null
                );

            entity.IsGrayScaled = Settings.Default.EnableBrightnessAndContrastAdjustments && filesToGrayscale.Where(w => w == entity.FilePath).Any();

            return entity.IsGrayScaled
                    ? Grayscale.CommonAlgorithms.BT709.Apply(imgRgb)
                    : (Bitmap)imgRgb.Clone();
        }

        internal static void SaveWebpAsJpeg(
                this FileEntity entity,
                string newFilePath
            )
        {
            using var img = SixLabors.ImageSharp.Image.Load(entity.FilePath);
            img.SaveAsJpeg(newFilePath);
            img.Dispose();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            var wasDeleted = false;

            while (!wasDeleted)
            {
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
    }
}
