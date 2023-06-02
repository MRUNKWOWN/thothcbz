using ThothCbz.Constants;
using ThothCbz.Entities;
using ThothCbz.Enumerators;
using ThothCbz.Extensions;
using ThothCbz.Properties;

using Image = System.Drawing.Image;
using Rectangle = System.Drawing.Rectangle;

namespace ThothCbz.Actions
{
    internal class Split
    {
        internal static void ExecuteSpliting(
                List<FileEntity> filesToAdjust
            )
        {
            foreach (var chapter in filesToAdjust
                .GroupBy(g => g.Chapter)
                .Select(s => new { s.Key, Items = s.Select(m => m).ToList() })
                .OrderBy(s => s.Key))
            {
                ModifyAndSave(
                        chapter.Items
                    );
            }
        }

        private static void ModifyAndSave(
                List<FileEntity> fileEntityList
            )
        {
            var uniqueIdentifier = Guid.NewGuid().ToString("N");

            foreach (var fileEntity in fileEntityList)
            {
                using var img = Image.FromFile(fileEntity.GetFilePathToJpgValue());

                var newWidth = Convert.ToInt32(img.Width / 2);
                var newHeight = img.Height;

                using var splitedImg01 = new Bitmap(newWidth, newHeight);
                splitedImg01.SetResolution(img.HorizontalResolution, img.VerticalResolution);

                using var splitedImg02 = new Bitmap(newWidth, newHeight);
                splitedImg02.SetResolution(img.HorizontalResolution, img.VerticalResolution);

                using var graphics1 = Graphics.FromImage(splitedImg01).SetDefaultQuality();
                graphics1.DrawImage(img, new Rectangle(0, 0, img.Width, newHeight));

                using var graphics2 = Graphics.FromImage(splitedImg02).SetDefaultQuality();
                graphics2.DrawImage(img, 0, 0, new Rectangle((img.Width - newWidth), 0, newWidth, newHeight), GraphicsUnit.Pixel);

                splitedImg01.SaveAsJpg(
                        fileEntity: fileEntity,
                        uniqueIdentifier: Settings.Default.ReadOrder == (int)ReadOrderTypes.RightToLeft
                                            ? GlobalConstants.DEFAULT_SPLITED_FILE_ORDER_02
                                            : GlobalConstants.DEFAULT_SPLITED_FILE_ORDER_01
                    );
                splitedImg02.SaveAsJpg(
                        fileEntity: fileEntity,
                        uniqueIdentifier: Settings.Default.ReadOrder == (int)ReadOrderTypes.RightToLeft
                                            ? GlobalConstants.DEFAULT_SPLITED_FILE_ORDER_01
                                            : GlobalConstants.DEFAULT_SPLITED_FILE_ORDER_02
                    );
                
                graphics1.Dispose();
                graphics2.Dispose();

                splitedImg01.Dispose();
                splitedImg02.Dispose();

                img.Dispose();

                GC.Collect();
                GC.WaitForPendingFinalizers();

                fileEntity.DeleteFile();

                ThothNotifyablePropertiesEntity.Default.SplittableFilesSetted.Add(1);
                ThothNotifyablePropertiesEntity.Default.ForceNotification(nameof(ThothNotifyablePropertiesEntity.Default.SplittableFilesSetted));

                fileEntity.FileWasSplit = true;
            }
        }
    }
}
