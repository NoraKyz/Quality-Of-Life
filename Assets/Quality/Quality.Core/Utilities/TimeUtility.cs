using System;

namespace Quality.Core.Utilities
{
    public class TimeUtility
    {
        private const string DATE_TIME_FORMAT = "HH:mm:ss dd/MM/yyyy";
        
        public static string GetCurrentTime()
        {
            return DateTime.UtcNow.ToString(DATE_TIME_FORMAT);
        }

        public static long GetCurrentTimestamp()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }
    }
}
