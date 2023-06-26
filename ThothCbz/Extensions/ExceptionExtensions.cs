using ThothCbz.Constants;
using ThothCbz.Entities;
using ThothCbz.Properties;

namespace ThothCbz.Extensions
{
    internal static class ExceptionExtensions
    {
        internal static void InformAndSaveLog(
                this Exception exception
            )
        {
            var logFile = $@"{ThothNotifyablePropertiesEntity.Default.DirectoryPathToAnalyze.TrimEnd('\\')}\{GlobalConstants.DEFAULT_LOG_FILE_NAME}";

            File.AppendAllLines(
                    logFile,
                    new List<string>() {
                            Environment.NewLine,
                            $@"ERROR: {exception.Message}",
                            exception.StackTrace is null ? string.Empty : $@"STACKTRACE: {exception.StackTrace}"
                    });

            MessageBox.Show(string.Format(Resources.ProcessWithErrorTxt, logFile));
        }
    }
}
