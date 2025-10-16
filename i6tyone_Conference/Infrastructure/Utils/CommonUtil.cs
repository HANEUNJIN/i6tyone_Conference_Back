using eGhis_WebService_Core.Define;
using System.Runtime.CompilerServices;

namespace eGhis_WebService_Core.Infrastructure.Utils
{
    public static class CommonUtil
    {
        #region GENERAL STATIC METHOD AREA *************************************************
        public static DateTime UnixTimeToDatetime(long unixTime)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(unixTime);
            return dateTimeOffset.LocalDateTime;
        }

        public static void WriteLoggerString(
            LoggerLevel logLevel,
            ErrorStatusCode statusCode,
            string memo = "",
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            string className = Path.GetFileNameWithoutExtension(filePath);
            string location = $"{className}_{memberName}[{lineNumber}]";

            string message = logLevel == LoggerLevel.INFO ? memo :
                string.IsNullOrEmpty(memo)
                ? EnumUtil.GetDescription(statusCode)
                : $"{EnumUtil.GetDescription(statusCode)}, memo : {memo}";

            switch (logLevel)
            {
                case LoggerLevel.DEBUG:
                    LoggerUtil.Debug(statusCode.ToString(), location, message);
                    break;
                case LoggerLevel.INFO:
                    LoggerUtil.Info(statusCode.ToString(), location, message);
                    break;
                case LoggerLevel.WARN:
                    LoggerUtil.Warn(statusCode.ToString(), location, message);
                    break;
                case LoggerLevel.ERROR:
                    LoggerUtil.Error(statusCode.ToString(), location, message);
                    break;
                case LoggerLevel.FATAL:
                    LoggerUtil.Fatal(statusCode.ToString(), location, message);
                    break;
            }
        }
        #endregion
    }
}
