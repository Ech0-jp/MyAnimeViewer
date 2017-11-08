using System;

namespace MyAnimeViewer.Utility.Extensions
{
    public static class DateTimeExtentions
    {
        public static long ToUnixTime(this DateTime time) => Math.Max(0, (long)(time.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds);
    }
}
