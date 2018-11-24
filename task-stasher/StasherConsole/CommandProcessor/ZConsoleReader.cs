using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskStasher.Control.Core;
using static TaskStasher.ZConsole.ConsoleSyntax.ZConsoleSyntax;
using static TaskStasher.ZConsole.VisualFlair.ConsoleFormat;
using static TaskStasher.ZConsole.ConsoleUtil;

namespace TaskStasher.ZConsole
{

    [Flags]
    public enum ConsoleStatus { OK, Cancel, Quit }

    /// <summary>
    /// Interacts with the console and actually gets lines
    /// </summary>
    public class ZConsoleReader
    {

        #region Private Field and Life Cycle

        private ZTaskManager manager;

        public ZConsoleReader(ZTaskManager manager)
        {
            this.manager = manager;
        }

        #endregion

        private static ConsoleStatus GetLine(out string content, bool reserveCap = false)
        {
            string copy = Console.ReadLine();
            string line = copy.Purify();

            switch (line)
            {
                case Cancel:
                    content = null;
                    return ConsoleStatus.Cancel;
                case Quit:
                    content = null;
                    return ConsoleStatus.Quit;
                default:
                    content = reserveCap?copy:line;
                    return ConsoleStatus.OK;
            }
            
        }

        /// <summary>
        /// Starting a new scope
        /// </summary>
        private static ConsoleStatus GetBlock(out string content, bool reserveCap=false)
        {
            string copy = Console.ReadLine();
            string main = copy.Purify();

            switch (main)
            {
                case "":
                    content = null;
                    return ConsoleStatus.OK;
                case Cancel:
                    content = null;
                    return ConsoleStatus.Cancel;
                case Quit:
                    content = null;
                    return ConsoleStatus.Quit;
                case StartMarker:
                    content = "";
                    break;
                default:
                    content = reserveCap?copy:main;
                    return ConsoleStatus.OK;
            }

            while (true)
            {
                copy = Console.ReadLine();
                string line = copy.Purify();

                switch (line)
                {
                    case Cancel:
                        content = null;
                        return ConsoleStatus.Cancel;
                    case Quit:
                        content = null;
                        return ConsoleStatus.Quit;
                    case EndMarker:
                        content = content.Trim();
                        return ConsoleStatus.OK;
                    default:
                        content += (reserveCap ? copy : line) + "\n";
                        break;
                }

            }
            
        }

        public ConsoleStatus GetTitle(out string title)
        {
            ConsoleStatus status;
            title = null;
            Prompt("Enter Title: ");
            status = GetLine(out title, true);
            if (Bad(status)) return status;

            while (String.IsNullOrWhiteSpace(title))
            {
                Prompt("You must enter a title: ");
                status = GetLine(out title, true);
                if (Bad(status)) return status;
            }

            return ConsoleStatus.OK;
        } 

        public ConsoleStatus GetDescription(out string description)
        {
            Print("Enter Description Below (press Enter to skip): ");
            return GetBlock(out description, true);
        }

        public ConsoleStatus GetDeadline(out DateTime? deadline)
        {
            bool success = false;
            ConsoleStatus status;
            deadline=null;
            DateTime temp = new DateTime();
            do
            {
                Prompt("Enter Deadline (yyyy-mm-dd hh:mm): ");
                status = GetLine(out string rawdate);
                if (Bad(status)) return status;
                success = DateTime.TryParse(rawdate, out temp);
            } while (!success);
            deadline = temp;
            return ConsoleStatus.OK;
        }

        public ConsoleStatus GetTimeSpan(out TimeSpan? time, string promptMessage)
        {
            ConsoleStatus status;
            bool success = false;
            time = null;
            TimeSpan temp = TimeSpan.Zero;
            do
            {
                Prompt(promptMessage);
                status = GetLine(out string rawbuffer);
                if (Bad(status)) return status;
                success = TimeSpan.TryParse(rawbuffer, out temp);
            } while (!success);

            time = temp;
            return ConsoleStatus.OK;
        }

        public ConsoleStatus GetBuffer(out TimeSpan? buffer)
        {
            return GetTimeSpan(out buffer, "Enter deadline buffer (dd.hh:mm:00:00): ");
        }

        /// <summary>
        /// Reasd a task from console, and if successful pushes
        /// the task to the manager.
        /// </summary>
        /// <param name="scheduled"></param>
        /// <param name="priority"></param>
        /// <returns></returns>
        public ConsoleStatus GetTask(bool scheduled, bool priority)
        {
            ConsoleStatus status;

            status = GetTitle(out string title);
            if (Bad(status)) return status;

            status = GetDescription(out string description);
            if (Bad(status)) return status;

            if (scheduled)
            {
                status = GetDeadline(out DateTime? deadline_wrapper);
                if (Bad(status)) return status;

                status = GetBuffer(out TimeSpan? buffer_wrapper);
                if (Bad(status)) return status;

                // Creating and pushing a scheduled task
                var task_scheduled = new ZScheduledTask()
                {
                    Title = title,
                    Description = description,
                    Deadline = deadline_wrapper.Value,
                    ZBuffer = buffer_wrapper.Value
                };

                if (priority)
                    manager.PushTaskWithPriority(task_scheduled);
                else
                    manager.PushTask(task_scheduled);
               
            }
            else
            {
                manager.PushTask(new ZTask()
                {
                    Title = title,
                    Description = description,
                });
            }

            return status;
        }

    }
}
