using AForge;
using AForge.Imaging.Filters;

using ThothCbz.Constants;
using ThothCbz.Entities;
using ThothCbz.Extensions;
using ThothCbz.Properties;

namespace ThothCbz.Actions
{
    internal static class Adjustments
    {
        internal static void ModifyAndSave(
                FileEntity fileEntity,
                List<string> filesToGrayscale,
                System.Drawing.Size? defaultSize = null
            )
        {
            if (!File.Exists(fileEntity.FilePath))
                return;

            var uniqueIdentifier = Guid.NewGuid().ToString("N");

            var color = fileEntity.Extension switch
            {
                GlobalConstants.DEFAULT_PNG_EXTENSION => Color.White,
                _ => System.Drawing.Color.Black,
            };

            using var img = fileEntity.GetImage(
                    backgroundColor: color,
                    filesToGrayscale: filesToGrayscale,
                    defaultSize: defaultSize
                );

            if (Settings.Default.EnableBrightnessAndContrastAdjustments)
            {
                if (fileEntity.IsGrayScaled)
                {
                    var levelsLinear = new LevelsLinear
                    {
                        InRed = new IntRange(30, 230),
                        InGreen = new IntRange(50, 240),
                        InBlue = new IntRange(10, 210)
                    };

                    levelsLinear.ApplyInPlace((Bitmap)img);
                }
                else
                {
                    var contrastCorrection = new ContrastCorrection();
                    contrastCorrection.ApplyInPlace((Bitmap)img);

                    var saturationCorrection = new SaturationCorrection(0.15f);
                    saturationCorrection.ApplyInPlace((Bitmap)img);
                }
            }

            float factor = (Settings.Default.EnableUpscale && img.Height < Settings.Default.MinimalImageHeight)
                        ? (float)Settings.Default.MinimalImageHeight / (float)img.Height
                        : 1;

            if (factor != 1)
            {
                using var newImg = img.Resize(
                                    resizeFactor: factor,
                                    backgroundColor: color
                                );

                newImg.SaveAs(
                                fileEntity,
                                uniqueIdentifier
                            );

                newImg.Dispose();

                fileEntity.SharpenAndSaveAs(
                        fileEntity.GetFilePathToImageOutputFileTypeValue(uniqueIdentifier)
                    );
            }
            else
            {
                ((Bitmap)img).SaveAs(
                            fileEntity,
                            uniqueIdentifier
                        );
            }

            img.Dispose();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            fileEntity.ReplaceOldFile(
                    uniqueIdentifier
                );

            ThothNotifyablePropertiesEntity.Default.FilesSetted.Add(1);
            ThothNotifyablePropertiesEntity.Default.ForceNotification(nameof(ThothNotifyablePropertiesEntity.Default.FilesSetted));

            if (fileEntity.NeedConversion) 
            {
                ThothNotifyablePropertiesEntity.Default.AdjustableFilesSetted.Add(1);
                ThothNotifyablePropertiesEntity.Default.ForceNotification(nameof(ThothNotifyablePropertiesEntity.Default.AdjustableFilesSetted));
            }

            fileEntity.FileWasAdjusted = true;
        }
    }
}
