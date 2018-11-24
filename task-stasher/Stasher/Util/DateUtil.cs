using System;
using System.Globalization;

namespace TaskStasher.Control.Core.Util
{
    public static class DateUtil
    {
        public static string ToFormattedString(this DateTime time)
        {
            return time.ToString("g", CultureInfo.CreateSpecificCulture("en-us"));
        }
    }
}
