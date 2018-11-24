using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TaskStasher.ZConsole.VisualFlair.ConsoleFormat;

namespace TaskStasher.ZConsole
{
    public static class ZDebugUtil
    {
        public static void PrintError(Exception e)
        {
            PrintDanger(e.Message);
            PrintDanger($"{e.Source}\n{e.StackTrace}\n{e.InnerException}");
        }
    }
}
