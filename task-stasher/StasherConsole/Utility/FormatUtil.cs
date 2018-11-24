using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskStasher.Control.Core;

namespace TaskStasher.ZConsole
{
    public static class FormatUtil
    {
        public static string Purify(this string command)
        {
            return command.Trim().ToLower();
        }

        public const string BreakLine 
            = "==================================================================";
        
        public const string PrioritizedLabel
            = "|---PRIORITIZED---|";

        public const string UrgentLabel
            = "|---URGENT---|";

        public const string DangerLabel
            = "|---Danger---|---Overdue---|";

    }
}
