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
                ISet<string> filesToGrayscale,
                System.Drawing.Size? defaultSize = null
            )
        {
            if (!File.Exists(fileEntity.FilePath))
                return;

            var uniqueIdentifier = Guid.NewGuid().ToString("N");

            var color = fileEntity.Extension switch
            {
                GlobalConstants.DEFAULT_PNG_EXTENSION => System.Drawing.Color.White,
                _ => System.Drawing.Color.Black,
            };

            using var img = (System.Drawing.Bitmap)fileEntity.GetImage(
                    backgroundColor: color,
                    filesToGrayscale: filesToGrayscale,
                    defaultSize: defaultSize
                );

            if (Settings.Default.EnableBrightnessAndContrastAdjustments)
            {
                using var adjustedImage = fileEntity.IsGrayScaled
                    ? img.ApplyLevelsLinear(30, 230, 50, 240, 10, 210)
                    : img.ApplyContrastAndSaturation(1.15f, 0.15f);

                float factor = (Settings.Default.EnableUpscale && adjustedImage.Height < Settings.Default.MinimalImageHeight)
                            ? (float)Settings.Default.MinimalImageHeight / (float)adjustedImage.Height
                            : 1;

                if (factor != 1)
                {
                    using var newImg = adjustedImage.Resize(
                                        resizeFactor: factor,
                                        backgroundColor: color
                                    );

                    newImg.SaveAs(
                                    fileEntity,
                                    uniqueIdentifier
                                );

                    fileEntity.SharpenAndSaveAs(
                            fileEntity.GetFilePathToImageOutputFileTypeValue(uniqueIdentifier)
                        );
                }
                else
                {
                    adjustedImage.SaveAs(
                                fileEntity,
                                uniqueIdentifier
                            );
                }

                adjustedImage.Dispose();
            }
            else
            {
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

                    fileEntity.SharpenAndSaveAs(
                            fileEntity.GetFilePathToImageOutputFileTypeValue(uniqueIdentifier)
                        );
                }
                else
                {
                    img.SaveAs(
                                fileEntity,
                                uniqueIdentifier
                            );
                }
            }

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
