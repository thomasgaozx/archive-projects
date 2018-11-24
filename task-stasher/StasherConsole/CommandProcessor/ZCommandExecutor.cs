using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskStasher.Control.Core;
using static TaskStasher.ZConsole.ConsoleSyntax.ZConsoleSyntax;
using static TaskStasher.ZConsole.VisualFlair.ConsoleFormat;
using static TaskStasher.ZConsole.ConsoleUtil;
using TaskStasher.ZConsole.CommandProcessor.Resources;

namespace TaskStasher.ZConsole
{
    /// <summary>
    /// Handles each individual command and its corresponding flags.
    /// </summary>
    public class ZCommandExecutor
    {

        #region Private Field 

        private ZTaskManager manager;
        private ZConsoleReader reader;
        private ZCommandMap cmdMap; // for help menu

        #endregion

        #region Methods

        public void PrintHelpMenu(string[] commands)
        {
            switch (commands.Length)
            {
                case 1:
                    cmdMap.PrintAllCommands();
                    break;
                case 2: // there exists a flag
                    if (commands[1].Equals(KeywordFlag))
                    {
                        var keys = cmdMap.GetAllCommands().Select(c => c.Key);
                        foreach (var key in keys)
                        {
                            Print(key);
                        }
                    }
                    else
                    {
                        var confused = cmdMap[commands[1]];
                        if (confused == null)
                        {
                            throw new Exception($"'{commands[1]}' is not a keyword");
                        }
                        cmdMap.PrintCommandNode(confused);
                    }
                    break;
                default:
                    throw new BadCommandException(commands);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commands">the whole command include the main command</param>
        /// <returns></returns>
        public ConsoleStatus AddNewTask(string[] commands)
        {
            // 1. Parse flags
            if (commands.Length > 3)
            {
                throw new BadCommandException(commands);
            }

            bool priority = false;
            bool scheduled = false;

            for (int i = 1; i < commands.Length; ++i)
            {
                switch (commands[i])
                {
                    case ScheduledFlag:
                        scheduled = true;
                        break;
                    case PriorityFlag:
                        priority = true;
                        break;
                    default:
                        throw new BadCommandException();
                }
            }

            // 2. Add task
            ConsoleStatus status = reader.GetTask(scheduled, priority);

            // 3. Print message and return
            switch (status)
            {
                case ConsoleStatus.Cancel:
                    PrintWarning("Task is cancelled");
                    break;
                case ConsoleStatus.OK:
                    Print("Task is added");
                    break;
                default:
                    break;
            }
            return status;

        }

        public void PrintCurrentTask()
        {
            PrintBright(FormatUtil.BreakLine);
            if (manager.CurrentTaskIsPrioritized)
            {
                PrintWarning(FormatUtil.PrioritizedLabel);
            }
            ZScheduledTask sCur = manager.CurrentTask as ZScheduledTask;
            if (sCur != null)
            {
                if (sCur.IsInDanger())
                {
                    PrintWarning(FormatUtil.UrgentLabel);
                }
                else if (sCur.IsInDanger())
                {
                    PrintWarning(FormatUtil.DangerLabel);
                }
            }
            PromptBright(manager.CurrentTask.ToString());
            PrintBright(FormatUtil.BreakLine);
        }

        public void PutOffCurrentTask(string[] commands)
        {
            string title = null;
            switch (commands.Length)
            {
                case 1:
                    title = manager.PutOffCurrentTask(new Random().Next(2, 4));
                    break;
                case 2:
                    string extent = commands[1];
                    if (extent.Equals(ToBottomFlag))
                    {
                        title = manager.PutOffCurrentTaskToBottom();
                    }
                    else
                    {
                        title = manager.PutOffCurrentTask(int.Parse(extent));
                    }
                    break;
                default:
                    throw new BadCommandException(commands);
            }
            Print($"Task '{title}' is successfully put off ...");
        }

        public void ArchiveCurrentTask(string[] commands)
        {
            if (commands.Length != 2)
            {
                throw new BadCommandException(commands);
            }

            string title; 
            switch (commands[1])
            {
                case Cat_Done:
                    title = manager.ArchiveCurrentTask(Category.Done);
                    break;
                case Cat_Cancelled:
                    title = manager.ArchiveCurrentTask(Category.Cancelled);
                    break;
                case Cat_Obsolete:
                    title = manager.ArchiveCurrentTask(Category.Obsolete);
                    break;
                default:
                    throw new BadCommandException(commands);
            }
            Print($"Task '{title}' is successfully archived ...");
        }

        /// <summary>
        /// Stretches the deadline of the current task. 
        /// Message: task '{taskName}' has been stretched from '{originalTime}'
        /// to '{newTime}' ...
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        public ConsoleStatus StretchCurrentTaskDeadline()
        {
            // Setting conditions for deadline-stretching
            if (manager.CurrentTaskNotLoaded)
                throw new Exception(ZTaskManager.YouHaveNotLoadedACurrentTask);
            else
            {
                ZScheduledTask scheduled = manager.CurrentTask as ZScheduledTask;
                if (scheduled == null)
                    throw new Exception($"Task '{manager.CurrentTask.Title}' is not scheduled!");

                // Gather information from the task
                string title = scheduled.Title;
                DateTime deadline_old = scheduled.Deadline;

                // Get the time from console and check status
                ConsoleStatus status = reader.GetTimeSpan(out TimeSpan? stretch, "Enter the time span that you wish to stretch (dd.hh:mm:ss:00:00): ");
                if (Bad(status))
                {
                    if (status == ConsoleStatus.Cancel)
                    {
                        Print("Operation Cancelled ...");
                    }
                    return status;
                }

                // Put off current task by the value, print message and return
                scheduled.PutOff(stretch.Value);
                Print($"Task '{title}' has been stretched from {deadline_old.ToString("o")} to {scheduled.Deadline.ToString("o")}");

                return ConsoleStatus.OK;
            }
        }

        /// <summary>
        /// Resets the deadline of the current task
        /// </summary>
        /// <returns>Console Task</returns>
        public ConsoleStatus ResetCurrentTaskDeadline()
        {
            // Setting conditions for deadline-resetting
            if (manager.CurrentTaskNotLoaded)
                throw new Exception(ZTaskManager.YouHaveNotLoadedACurrentTask);
            else
            {
                ZScheduledTask scheduled = manager.CurrentTask as ZScheduledTask;
                if (scheduled == null)
                    throw new Exception($"Task '{manager.CurrentTask.Title}' is not scheduled!");

                // Gather information from the task
                string title = scheduled.Title;
                DateTime deadline_old = scheduled.Deadline;

                // Read new deadline
                ConsoleStatus status = reader.GetDeadline(out DateTime? deadline);
                if (Bad(status))
                {
                    if (status == ConsoleStatus.Cancel)
                        Print("Operation Cancelled ...");
                    return status;
                }

                scheduled.ResetDeadline(deadline.Value);
                Print($"Task '{title}'s Deadline has been reset from {deadline_old.ToString("o")} to {scheduled.Deadline.ToString("o")}");

                return ConsoleStatus.OK;
            }
        }

        public ConsoleStatus ResetCurrentTaskBuffer()
        {
            // Setting conditions for buffer-resetting
            if (manager.CurrentTaskNotLoaded)
                throw new Exception(ZTaskManager.YouHaveNotLoadedACurrentTask);
            else
            {
                ZScheduledTask scheduled = manager.CurrentTask as ZScheduledTask;
                if (scheduled == null)
                    throw new Exception($"Task '{manager.CurrentTask.Title}' is not scheduled!");

                // Gather information from the task
                string title = scheduled.Title;
                TimeSpan buffer_old = scheduled.ZBuffer;

                // Read new buffer
                ConsoleStatus status = reader.GetTimeSpan(out TimeSpan? newBuffer, "Enter the new buffer (1.hh:mm:ss:00:00): ");
                if (Bad(status))
                {
                    if (status == ConsoleStatus.Cancel)
                        Print("Operation Cancelled ...");
                    return status;
                }

                // Reset Buffer, Print Message and Return OK
                scheduled.ResetBuffer(newBuffer.Value);
                Print($"Task '{title}' has been stretched from {buffer_old.ToString()} to {scheduled.ZBuffer.ToString()}");

                return ConsoleStatus.OK;

            }
        }

        public ConsoleStatus EditCurrentTaskTitle()
        {
            if (manager.CurrentTaskNotLoaded)
                throw new Exception(ZTaskManager.YouHaveNotLoadedACurrentTask);
            else
            {
                Print("Reset Task Title ...");

                // Gather old info
                string title_old = manager.CurrentTask.Title;

                // Read new Title
                ConsoleStatus status = reader.GetTitle(out string newTitle);
                if (Bad(status))
                {
                    if (IsCancel(status)) Print("Operation cancelled ...");
                    return status;
                }

                manager.CurrentTask.Title = newTitle;
                Print($"Task '{title_old}'s Title has been changed to '{newTitle}'");
                return ConsoleStatus.OK;
            }
        }

        public ConsoleStatus EditCurrentTaskDescription()
        {
            if (manager.CurrentTaskNotLoaded)
                throw new Exception(ZTaskManager.YouHaveNotLoadedACurrentTask);
            else
            {
                Print("Reset Task Description ...");

                // Read new Title
                ConsoleStatus status = reader.GetDescription(out string description_new);
                if (Bad(status))
                {
                    if (IsCancel(status)) Print("Operation cancelled ...");
                    return status;
                }

                manager.CurrentTask.Description = description_new;
                Print($"Task '{manager.CurrentTask.Title}'s description has updated ...");
                return ConsoleStatus.OK;
            }
        }

        public void View_AllTasks(string[] commands)
        {
            bool detail = false;

            switch (commands.Length)
            {
                case 1:
                    break;
                case 2:
                    if (commands[1].Equals(ShowDetail))
                    {
                        detail = true;
                    }
                    else
                    {
                        throw new BadCommandException(commands);
                    }
                    break;
                default:
                    throw new BadCommandException();
            }

            var tasks = manager.GetAllTasks();

            if (detail)
            {
                Print(FormatUtil.BreakLine);
                ConsoleUtil.WriteTasksInDetail(tasks);
                detail = false;
            }
            else
            {
                for (int i = 0; i < tasks.Count; ++i)
                {
                    Print(i + ". " + tasks[i].Title);
                }
            }
        }

        public void View_SelectedTasks(string[] commands)
        {
            // Arrange
            bool detail = false;
            TaskFlag selection = TaskFlag.None;

            // Collect selection flags
            for (int i = 1; i < commands.Length; ++i)
            {
                switch (commands[i])
                {
                    case PrioritizedTaskFlag:
                        selection = selection | TaskFlag.Prioritized;
                        break;
                    case NonPrioritizedTaskFlag:
                        selection = selection | TaskFlag.NonPrioritized;
                        break;
                    case UrgentTaskFlag:
                        selection = selection | TaskFlag.Urgent;
                        break;
                    case DangerTaskFlag:
                        selection = selection | TaskFlag.Danger;
                        break;
                    case ScheduledTaskFlag:
                        selection = selection | TaskFlag.Scheduled;
                        break;
                    case NonScheduledTaskFlag:
                        selection = selection | TaskFlag.NonScheduled;
                        break;
                    case ShowDetail:
                        detail = true;
                        break;
                    default:
                        throw new Exception("Bad selection command");
                }

            }

            // Select tasks and Print
            var selectedTasks = manager.SelectTasks(selection);

            if (selectedTasks.Any())
            {
                if (detail)
                {
                    Print(FormatUtil.BreakLine);
                    ConsoleUtil.WriteTasksInDetail(selectedTasks);
                    detail = false;
                }
                else
                {
                    for (int i = 0; i < selectedTasks.Count; ++i)
                    {
                        Print($"{i}. {selectedTasks[i].Title}");
                    }
                }
            }

        }

        private void WriteArchivesOnCategory(string catCmd)
        {
            switch (catCmd)
            {
                case Cat_Done:
                    var doneArcs = manager.GetArchives(Category.Done);
                    if (doneArcs.Any())
                    {
                        ConsoleUtil.WriteArchivesInDetail(doneArcs);
                    }
                    break;
                case Cat_Cancelled:
                    var cancelledArcs = manager.GetArchives(Category.Cancelled);
                    if (cancelledArcs.Any())
                    {
                        ConsoleUtil.WriteArchivesInDetail(cancelledArcs);
                    }
                    break;
                case Cat_Obsolete:
                    var obsoleteArcs = manager.GetArchives(Category.Obsolete);
                    if (obsoleteArcs.Any())
                    {
                        ConsoleUtil.WriteArchivesInDetail(obsoleteArcs);
                    }
                    break;
                default:
                    throw new BadCommandException();

            }
        }

        public void View_Archives(string[] commands)
        {
            switch (commands.Length)
            {
                case 1:
                    var archives = manager.GetArchives();
                    if (archives.Any())
                        ConsoleUtil.WriteArchivesInDetail(archives);
                    break;
                case 2:
                    WriteArchivesOnCategory(commands[1]);
                    break;
                default:
                    throw new BadCommandException(commands);
            }
        }

        public ConsoleStatus ScheduleCurrentTask()
        {
            if (manager.CurrentTaskNotLoaded)
            {
                throw new Exception(ZTaskManager.YouHaveNotLoadedACurrentTask);
            }
            else if (manager.CurrentTask is ZScheduledTask)
            {
                throw new Exception("The current task is already scheduled");
            }
            else
            {
                ConsoleStatus status;
                status=reader.GetDeadline(out DateTime? deadline);
                if (Bad(status)) return status;

                reader.GetBuffer(out TimeSpan? buffer);
                if (Bad(status)) return status;

                manager.ScheduleCurrentTask(deadline.Value, buffer.Value);
                return ConsoleStatus.OK;
            }
        }

        #endregion

        #region Life Cycle

        public ZCommandExecutor(ZTaskManager manager,ZConsoleReader reader)
        {
            this.manager = manager;
            this.reader = reader;
            cmdMap = new ZCommandMap();
        }

        #endregion

    }
}
