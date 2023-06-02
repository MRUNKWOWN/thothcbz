using ThothCbz.Enumerators;
using ThothCbz.Properties;

namespace ThothCbz.Extensions
{
    internal static class ExecutionStatusTypeExtensions
    {
        internal static string GetExecutionStatusText(
                this ExecutionStatusType value
            )
        {
            return value switch
            {
                ExecutionStatusType.Done => Resources.LblExecutionLogStatusDoneText,
                ExecutionStatusType.Error => Resources.LblExecutionLogStatusErrorText,
                ExecutionStatusType.NotRunning => Resources.LblExecutionLogStatusAnalyzedText,
                ExecutionStatusType.Queued => Resources.LblExecutionLogStatusQueueText,
                ExecutionStatusType.Running => Resources.LblExecutionLogStatusRunningText,
                ExecutionStatusType.Warning => Resources.LblExecutionLogStatusWarningText,
                _ => throw new NotImplementedException()
            };
        }

        internal static string GetExecutionStatusColorText(
                this ExecutionStatusType value
            )
        {
            var defaulRtfTag = $@"\cf";

            return value switch
            {
                ExecutionStatusType.Done => $@"{defaulRtfTag}4",
                ExecutionStatusType.Error => $@"{defaulRtfTag}5",
                ExecutionStatusType.NotRunning => $@"{defaulRtfTag}1",
                ExecutionStatusType.Queued => $@"{defaulRtfTag}2",
                ExecutionStatusType.Running => $@"{defaulRtfTag}3",
                ExecutionStatusType.Warning => $@"{defaulRtfTag}6",
                _ => throw new NotImplementedException()
            };
        }
    }
}
