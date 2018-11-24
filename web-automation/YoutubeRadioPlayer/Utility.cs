using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebAutomation.YoutubeAdSkipper.Utility
{
    public static class Util
    {
        public static readonly Regex SecondsPattern = new Regex(@"\d{1,2}$");
        public static readonly Regex MinutesPattern = new Regex(@"\d{1,2}(?=:\d{2}$)");
        public static readonly Regex HoursPattern = new Regex(@"\d{1,2}(?=:\d{2}:\d{2}$)");

        public static TimeSpan ParseTimeFrom(string youTubeFormattedString)
        {
            TimeSpan raw = TimeSpan.FromSeconds(Double.Parse(SecondsPattern.Match(youTubeFormattedString).Value));
            string min = MinutesPattern.Match(youTubeFormattedString).Value;
            string hour = HoursPattern.Match(youTubeFormattedString).Value;
            if (min!="")
            {
                raw+=TimeSpan.FromMinutes(Double.Parse(min));
            }
            if (hour!="")
            {
                raw+=TimeSpan.FromHours(Double.Parse(hour));
            }
            return raw;
        }
    }
}
