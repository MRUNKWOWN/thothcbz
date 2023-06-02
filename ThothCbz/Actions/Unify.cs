using ThothCbz.Extensions;
using ThothCbz.Entities;
using ThothCbz.Enumerators;
using ThothCbz.Properties;

using Image = System.Drawing.Image;
using Color = System.Drawing.Color;
using Rectangle = System.Drawing.Rectangle;

namespace ThothCbz.Actions
{
    internal static class Unify
    {
        internal static void ExecuteUnification(
                List<FileEntity> filesToAdjust
            )
        {
            foreach(var chapter in filesToAdjust
                .GroupBy(g => g.Chapter)
                .Select(s => new { s.Key, Items = s.Select(m => m).ToList() })
                .OrderBy(s => s.Key))
            {
                ModifyAndSave(
                        chapter.Items.OrderBy(o => o.Name).ToList()
                    );
            }
        }

        private static void ModifyAndSave(
                List<FileEntity> fileEntityList
            )
        {
            var uniqueIdentifier = Guid.NewGuid().ToString("N");

            for (int i = 0; i < fileEntityList.Count();)
            {
                var idxImagem0 = Settings.Default.ReadOrder == (int)ReadOrderTypes.LeftToRight ? 0 : 1;
                var idxImagem1 = Settings.Default.ReadOrder == (int)ReadOrderTypes.LeftToRight ? 1 : 0;

                using var img1 = Image.FromFile(fileEntityList[i + idxImagem0].GetFilePathToJpgValue())!;
                using var img2 = Image.FromFile(fileEntityList[i + idxImagem1].GetFilePathToJpgValue())!;

                if (img1 is null || img2 is null)
                {
                    throw new ArgumentNullException(nameof(FileEntity.FilePath));
                }

                var height = Math.Max(img1.Height, img2.Height);

                using var imgResized1 = img1.Resize(
                       resizeFactor: img1.Height < height
                                        ? (float)height / (float)img1!.Height
                                        : 1,
                       backgroundColor: Color.Black
                    );

                using var imgResized2 = img2.Resize(
                       resizeFactor: img2.Height < height
                                        ? (float)height / (float)img2.Height
                                        : 1,
                       backgroundColor: Color.Black
                    );

                var space = Settings.Default.EnableSpaceInUnifyablePages
                                ? Math.Max(imgResized1.Width, imgResized2.Width) - Math.Min(imgResized1.Width, imgResized2.Width)
                                : 0;

                using var newUnifiedImage = new Bitmap(Convert.ToInt32(imgResized1.Width + space + imgResized2.Width), height);
                newUnifiedImage.SetResolution(imgResized1.HorizontalResolution, imgResized1.VerticalResolution);

                using var graphics = Graphics.FromImage(newUnifiedImage).SetDefaultQuality(backgroundColor: Color.Black);

                graphics.DrawImage(imgResized1, new Rectangle(0, 0, imgResized1.Width, height));
                graphics.DrawImage(imgResized2, new Rectangle(space + imgResized1.Width, 0, imgResized2.Width, height));

                newUnifiedImage.SaveAsJpg(
                            fileEntityList[i],
                            uniqueIdentifier
                        );

                graphics.Dispose();
                newUnifiedImage.Dispose();

                imgResized1.Dispose();
                imgResized2.Dispose();

                img1.Dispose();
                img2.Dispose();

                GC.Collect();
                GC.WaitForPendingFinalizers();

                fileEntityList[i].ReplaceOldFile(
                        uniqueIdentifier
                    );

                fileEntityList[i + 1].DeleteFile();

                ThothNotifyablePropertiesEntity.Default.UnifyableFilesSetted.Add(2);
                ThothNotifyablePropertiesEntity.Default.ForceNotification(nameof(ThothNotifyablePropertiesEntity.Default.UnifyableFilesSetted));

                fileEntityList[i].FileWasUnified = true;
                fileEntityList[i + 1].FileWasUnified = true;

                i += 2;
            }
        }
    }
}
