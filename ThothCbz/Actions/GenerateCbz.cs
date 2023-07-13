using System.IO;
using System.IO.Compression;

using ThothCbz.Constants;
using ThothCbz.Entities;
using ThothCbz.Extensions;

namespace ThothCbz.Actions
{
    internal static class GenerateCbz
    {
        internal static void ExecuteCbzGeneration(
                List<FileEntity> filesToAdjust
            )
        {
            if (!filesToAdjust.Any())
            {
                return;
            }

            var fileEntity = filesToAdjust.First(f => !f.IsUnify && !f.IsSplit);
            var hasVolume = !string.IsNullOrWhiteSpace(fileEntity.Volume);
            var hasChapter = !string.IsNullOrWhiteSpace(fileEntity.Chapter);

            var directoryToCompress = !hasVolume || !hasChapter
                                        ? Directory.GetParent(fileEntity.FilePath)!.FullName
                                        : Directory.GetParent(Directory.GetParent(fileEntity.FilePath)!.FullName)!.FullName;

            var directoryToSaveNewFile = hasVolume && hasChapter
                                        ? Directory.GetParent(Directory.GetParent(Directory.GetParent(fileEntity.FilePath)!.FullName)!.FullName)!.FullName
                                        : Directory.GetParent(Directory.GetParent(fileEntity.FilePath)!.FullName)!.FullName;

            var volumeName = string.Empty;

            if(hasVolume)
            {
                volumeName = int.TryParse(fileEntity.Volume, out var volume)
                                ? $@"-{volume.ToString().PadLeft(GlobalConstants.DEFAULT_CHARACTERS_AMOUNT_FOR_FOLDERS, GlobalConstants.DEFAULT_CHARACTER_FOR_PADDING_FOR_FOLDER)}" 
                                : $@"-{fileEntity.Volume}";
            }

            ModifyAndSave(
                    directoryToCompress,
                    directoryToSaveNewFile,
                    $@"{fileEntity.Serie}{volumeName}"
                );

            foreach(var file in filesToAdjust)
            {
                file.FileWasCompressed = true;
            }

            var wasDeleted = false;

            while (!wasDeleted)
            {
                try
                {
                    if (Directory.Exists(directoryToCompress))
                    {
                        try
                        {
                            Directory.Delete(directoryToCompress, true);
                        }
                        catch { }
                    }

                    wasDeleted = true;
                }
                catch
                {
                    Thread.Sleep(500);
                }
            }

            ThothNotifyablePropertiesEntity.Default.CbzFilesSetted.Add(filesToAdjust.Count);

            foreach (var file in filesToAdjust)
            {
                file.FileWasCompressed = true;
            }
        }

        private static void ModifyAndSave(
                string directoryToCompress,
                string directoryToSaveNewFile,
                string fileName
            )
        {
            var adjustableFiles = Directory.GetFiles(directoryToCompress, GlobalConstants.DEFAULT_FILES_TO_GRAYSCALE_FILE_NAME).ToList();

            if(adjustableFiles.Any())
            {
                while (adjustableFiles.Any())
                {
                    try
                    {
                        var filePath = adjustableFiles.First();

                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }

                        adjustableFiles.RemoveAt(0);
                    }
                    catch
                    {
                        Thread.Sleep(500);
                    }
                }
            }

            var dirs = Directory.GetDirectories(directoryToCompress);

            if (!dirs.Any())
            {
                ZipFile.CreateFromDirectory(
                            directoryToCompress,
                            $@"{directoryToSaveNewFile}\{fileName}.cbz",
                            CompressionLevel.NoCompression,
                            false
                        );

                return;
            }

            var dirTemp = $@"{directoryToCompress}-{Guid.NewGuid().ToString("N")}";

            Directory.CreateDirectory(dirTemp);

            var files = Directory.GetFiles(directoryToCompress, "*.*", SearchOption.AllDirectories).ToList();

            foreach ( var file in files )
            {
                File.Move( file, dirTemp );
            }

            Directory.Delete(dirTemp, true);
        }
    }
}
