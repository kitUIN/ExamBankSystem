using System;
namespace ExamBankSystem.Helpers
{
    /// <summary>
    /// 时间转换帮助类
    /// </summary>
    public static class DateTimeHelper
    {
        /// <summary>
        /// 时间戳(13位)(毫秒)转DateTime
        /// </summary>
        public static DateTime ToDateTime(long timestamp)
        {
            return new DateTime(new DateTime(1970, 1, 1, 8, 0, 0).Ticks + timestamp * 10000);
        }
        /// <summary>
        /// 获取当前时间戳(13位)(毫秒)
        /// </summary>
        public static long GetTimeStamp()
        {
            return Convert.ToInt64((DateTime.Now - new DateTime(1970, 1, 1, 8, 0, 0, 0)).TotalMilliseconds);
        }
        /// <summary>
        /// DateTime转时间戳(13位)(毫秒)
        /// </summary>
        public static long ToTimeStamp(DateTime datetime)
        {
            return Convert.ToInt64((datetime - new DateTime(1970, 1, 1, 8, 0, 0, 0)).TotalMilliseconds);
        }
    }
}
