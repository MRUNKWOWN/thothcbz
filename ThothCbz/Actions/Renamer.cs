using System.Drawing.Imaging;

using ThothCbz.Constants;
using ThothCbz.Entities;
using ThothCbz.Enumerators;
using ThothCbz.Extensions;
using ThothCbz.Properties;

namespace ThothCbz.Actions
{
    internal static class Renamer
    {
        internal static void ExecuteRenamingAndMove(
                List<FileEntity> filesToAdjust,
                string? customBlankFilePath
            )
        {
            if(!filesToAdjust.Any())
            {
                return;
            }

            var defaultCharactersAmountForFilesName = filesToAdjust.Count().ToString().Length;
            var dictItensPerKey = new Dictionary<string, List<FileEntity>>();

            var keys = filesToAdjust
                .Where(w => !string.IsNullOrWhiteSpace(w.Chapter))
                .GroupBy(g => g.Chapter)
                .Select(s => s.Key)
                .ToList();

            if(!keys.Any())
            {
                MoveFiles(
                        chapterName: string.Empty,
                        fileEntityList: filesToAdjust,
                        defaultCharactersAmountForFilesName
                    );

                DeleteUnifyableSplittableFolders(filesToAdjust);

                return;
            }

            foreach ( var key in keys ) 
            {
                var splittableDecimalChar = key!.Contains(".")
                                                ? '.'
                                                : ',';

                var newKey = int.TryParse(key!, out var number)
                                ? number.ToString().PadLeft(GlobalConstants.DEFAULT_CHARACTERS_AMOUNT_FOR_FOLDERS, GlobalConstants.DEFAULT_CHARACTER_FOR_PADDING_FOR_FOLDER)
                                : double.TryParse(key!, out var number_with_decimal)
                                    ? Settings.Default.UnifySplittedChaptersFolder || number_with_decimal.ToString().Split(splittableDecimalChar).Length == 1
                                        ? number_with_decimal.ToString().Split(splittableDecimalChar)[0].PadLeft(GlobalConstants.DEFAULT_CHARACTERS_AMOUNT_FOR_FOLDERS, GlobalConstants.DEFAULT_CHARACTER_FOR_PADDING_FOR_FOLDER)
                                        : $@"{number_with_decimal.ToString().Split(splittableDecimalChar)[0].PadLeft(GlobalConstants.DEFAULT_CHARACTERS_AMOUNT_FOR_FOLDERS, GlobalConstants.DEFAULT_CHARACTER_FOR_PADDING_FOR_FOLDER)}-{number_with_decimal.ToString().Split(splittableDecimalChar)[1].PadLeft(GlobalConstants.DEFAULT_CHARACTERS_AMOUNT_FOR_FOLDERS, GlobalConstants.DEFAULT_CHARACTER_FOR_PADDING_FOR_FOLDER)}"
                                    : key!;

                if (dictItensPerKey.Keys.Contains(newKey))
                {
                    dictItensPerKey[newKey].AddRange(
                            filesToAdjust
                            .Where(w => w.Chapter == key)
                            .Select(s => s)
                            .ToList()
                        );
                }
                else
                {
                    dictItensPerKey.Add(
                            newKey,
                            filesToAdjust
                                .Where(w => w.Chapter == key)
                                .Select(s => s)
                                .ToList()
                        );
                }
            }

            var countKeys = 0;

            foreach (var key in dictItensPerKey.Keys.OrderBy(o => o).ToList())
            {
                countKeys++;
                MoveFiles(
                        chapterName: key,
                        dictItensPerKey[key],
                        defaultCharactersAmountForFilesName
                    );

                if(Settings.Default.EnableBlankPageBetweenChapters && countKeys < dictItensPerKey.Keys.Count) 
                {
                    var newBlankFilePath = $@"{dictItensPerKey[key].GetDirectoryFolder(true)}\\{key}-{(dictItensPerKey[key].Count + 1).ToString().PadLeft(defaultCharactersAmountForFilesName, GlobalConstants.DEFAULT_CHARACTER_FOR_PADDING_FOR_FILES)}.jpg";

                    if (((BlackPageType)Settings.Default.BlackPageType) == BlackPageType.Custom && !string.IsNullOrWhiteSpace(customBlankFilePath))
                    {
                        File.Copy(customBlankFilePath, newBlankFilePath);
                    }
                    else
                    {
                        var firstFile = Directory
                        .GetFiles(dictItensPerKey[key].GetDirectoryFolder(true), "*.jpg")
                        .OrderBy(o => o)
                        .FirstOrDefault();

                        if (!string.IsNullOrWhiteSpace(firstFile))
                        {
                            using var img = new Bitmap(firstFile);

                            using var newImg = new Bitmap(img.Width, img.Height, PixelFormat.Format32bppRgb);

                            newImg.SetResolution(img.HorizontalResolution, img.VerticalResolution);

                            using var graphics = Graphics
                                                    .FromImage(newImg)
                                                    .SetDefaultQuality(
                                                            backgroundColor: ((BlackPageType)Settings.Default.BlackPageType) == BlackPageType.White
                                                                                ? System.Drawing.Color.White
                                                                                : System.Drawing.Color.Black
                                                        );

                            newImg.SaveAsJpg(
                                    filePath: newBlankFilePath
                                );
                        }
                    }
                }

                foreach (var fileEntity in dictItensPerKey[key].GroupBy(g => g.Chapter).Select(s => s.Where(w2 => w2 is { }).First()).ToList())
                {
                    var directoryPath = (fileEntity.IsUnify || fileEntity.IsSplit)
                                            ? Directory.GetParent(Directory.GetParent(fileEntity.FilePath)!.FullName)!.FullName
                                            : Directory.GetParent(fileEntity.FilePath)!.FullName;
                    try
                    {
                        Directory.Delete(
                                    directoryPath,
                                    true
                                );
                    }
                    catch { }
                }
            }
        }

        private static void MoveFiles(
                string chapterName,
                List<FileEntity> fileEntityList,
                int defaultCharactersAmountForFilesName
            )
        {
            int pageNumber = 0;

            var filePath = fileEntityList
                                .Where(w => !w.IsUnify && !w.IsSplit)
                                .FirstOrDefault()
                                ?.FilePath ??
                                fileEntityList.First().FilePath;

            var destinyFolder = fileEntityList.GetDirectoryFolder(
                                        !string.IsNullOrWhiteSpace(chapterName)
                                    );

            var itensRenamedList = fileEntityList
                        .Select(s => new {
                            newName = int.TryParse(s.Name, out var number)
                                ? number.ToString().PadLeft(defaultCharactersAmountForFilesName, GlobalConstants.DEFAULT_CHARACTER_FOR_PADDING_FOR_FILES)
                                : s.Name!,
                            newFolder = string.IsNullOrWhiteSpace(s.Chapter)
                                            ? string.Empty
                                            : double.TryParse(s.Chapter, out var number_with_decimal)
                                                ? number_with_decimal.ToString().Split(s.Chapter.Contains(".") ? '.' : ',').Length == 1
                                                    ? $@"{number_with_decimal.ToString().Split(s.Chapter.Contains(".") ? '.' : ',')[0].PadLeft(GlobalConstants.DEFAULT_CHARACTERS_AMOUNT_FOR_FOLDERS, GlobalConstants.DEFAULT_CHARACTER_FOR_PADDING_FOR_FOLDER)}-{"0".PadLeft(GlobalConstants.DEFAULT_CHARACTERS_AMOUNT_FOR_FOLDERS, GlobalConstants.DEFAULT_CHARACTER_FOR_PADDING_FOR_FOLDER)}"
                                                    : $@"{number_with_decimal.ToString().Split(s.Chapter.Contains(".") ? '.' : ',')[0].PadLeft(GlobalConstants.DEFAULT_CHARACTERS_AMOUNT_FOR_FOLDERS, GlobalConstants.DEFAULT_CHARACTER_FOR_PADDING_FOR_FOLDER)}-{number_with_decimal.ToString().Split(s.Chapter.Contains(".") ? '.' : ',')[1].PadLeft(GlobalConstants.DEFAULT_CHARACTERS_AMOUNT_FOR_FOLDERS, GlobalConstants.DEFAULT_CHARACTER_FOR_PADDING_FOR_FOLDER)}"
                                                : s.Chapter,
                            item = s
                            }).ToList();

            foreach ( var file in itensRenamedList.OrderBy(o => o.newFolder).ThenBy(t => t.newName) ) 
            {
                pageNumber++;

                ThothNotifyablePropertiesEntity.Default.SeriesDictionary[file.item.Serie].First(w => w.FilePath == file.item.FilePath).FileWasRenamed = true;

                if (file.item.IsSplit && file.item.FileWasSplit)
                {
                    if (File.Exists(file.item.GetFilePathToJpgValue(GlobalConstants.DEFAULT_SPLITED_FILE_ORDER_01)))
                    {
                        ValidateAndMoveFile(
                                destinyFolder: destinyFolder,
                                file.item.GetFilePathToJpgValue(GlobalConstants.DEFAULT_SPLITED_FILE_ORDER_01),
                                GetNewFileName(pageNumber, chapterName, defaultCharactersAmountForFilesName)
                            );

                        pageNumber++;
                    }

                    if (File.Exists(file.item.GetFilePathToJpgValue(GlobalConstants.DEFAULT_SPLITED_FILE_ORDER_02)))
                    {
                        ValidateAndMoveFile(
                                destinyFolder: destinyFolder,
                                file.item.GetFilePathToJpgValue(GlobalConstants.DEFAULT_SPLITED_FILE_ORDER_02),
                                GetNewFileName(pageNumber, chapterName, defaultCharactersAmountForFilesName)
                            );
                    }

                    continue;
                }

                if (file.item.IsUnify && file.item.FileWasUnified && !File.Exists(file.item.GetFilePathToJpgValue()))
                {
                    pageNumber--;
                    continue;
                }

                ValidateAndMoveFile(
                            destinyFolder: destinyFolder,
                            file.item.GetFilePathToJpgValue(),
                            GetNewFileName(pageNumber, chapterName, defaultCharactersAmountForFilesName)
                        );
            }
        }

        private static void ValidateAndMoveFile(
                string destinyFolder,
                string oldFilePath,
                string newFileName
            )
        {
            var newFilePath = $@"{destinyFolder}\{newFileName}.jpg";

            if(newFilePath.ToLower() == oldFilePath.ToLower())
            {
                return;
            }

            if(oldFilePath == newFilePath) 
            { 
                return; 
            }

            if (File.Exists(newFilePath))
            {
                File.Delete(newFilePath);
            }

            File.Move(
                    oldFilePath, 
                    newFilePath
                );
        }

        private static string GetNewFileName(
                int pageNumber,
                string chapterName,
                int defaultCharactersAmountForFilesName
            )
        {
            var uniqueFileName = pageNumber.ToString().PadLeft(defaultCharactersAmountForFilesName, GlobalConstants.DEFAULT_CHARACTER_FOR_PADDING_FOR_FILES);

            return string.IsNullOrWhiteSpace(chapterName)
                    ? uniqueFileName
                    : $@"{chapterName}-{uniqueFileName}";
        }

        private static void DeleteUnifyableSplittableFolders(
                List<FileEntity> filesToAdjust
            )
        {
            var unifyable = filesToAdjust.Where(w => w.IsUnify).FirstOrDefault();
            var splittable = filesToAdjust.Where(w => w.IsSplit).FirstOrDefault();

            if (unifyable is { } && Directory.Exists(Directory.GetParent(unifyable.FilePath)!.FullName))
            {
                Directory.Delete(
                        Directory.GetParent(unifyable.FilePath)!.FullName,
                        true
                    );
            }

            if (splittable is { } && Directory.Exists(Directory.GetParent(splittable.FilePath)!.FullName))
            {
                Directory.Delete(
                        Directory.GetParent(splittable.FilePath)!.FullName,
                        true
                    );
            }
        }
    }
}
