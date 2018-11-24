using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskStasher.Control.Core;
using static TaskStasher.ZConsole.VisualFlair.ConsoleFormat;

namespace TaskStasher.ZConsole
{
    public static class ConsoleUtil
    {
        public static bool Bad(ConsoleStatus status)
        {
            return status != ConsoleStatus.OK;
        }

        public static bool IsQuit(ConsoleStatus status)
        {
            return status == ConsoleStatus.Quit;

        }

        public static bool IsCancel(ConsoleStatus status)
        {
            return status == ConsoleStatus.Cancel;
        }

        public static void WriteTasksInDetail(IEnumerable<ITask> tasks)
        {
            int i = 0;
            foreach (var task in tasks)
            {
                Print($"Task #{++i}\n{task.ToString()}{FormatUtil.BreakLine}");
            }
        }

        public static void WriteArchivesInDetail(IEnumerable<ZArchive> archives)
        {
            int i = 0;
            Print(FormatUtil.BreakLine);
            foreach (var archive in archives)
            {
                Print($"Archive #{++i}:\n{archive.Content.ToString()}" +
                    $"\nArchive Date: {archive.ArchiveDate}\nCategory: {Enum.GetName(typeof(Category), archive.Status)}" +
                    $"\n{FormatUtil.BreakLine}");
            }
        }
        
    }
}
