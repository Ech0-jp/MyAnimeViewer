using System;

/// MyAnimeViewer > Utilities > Helper
/// 
/// Description:
///     A Simple helper to convert DateTime to Unix Time.
/// 
/// @Author Robert Andrew Gray

namespace MyAnimeViewer.Utility
{
    public static class Helper
    {
        public static long ToUnixTime(this DateTime time)
        {
            var total = (long)(time.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            return total < 0 ? 0 : total;
        }
    }
}
