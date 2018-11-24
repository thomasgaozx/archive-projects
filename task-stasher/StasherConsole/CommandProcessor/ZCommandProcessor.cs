using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskStasher.Control.Core;
using TaskStasher.ZConsole.CommandProcessor.Resources;
using static TaskStasher.ZConsole.ConsoleSyntax.ZConsoleSyntax;
using static TaskStasher.ZConsole.VisualFlair.ConsoleFormat;
using static TaskStasher.ZConsole.ConsoleUtil;

namespace TaskStasher.ZConsole
{
    /// <summary>
    /// Responds to an array of commands
    /// </summary>
    public class ZCommandProcessor
    {
        private ZTaskManager manager;
        private ZConsoleReader reader;
        private ZCommandExecutor executor;

        #region Methods

        /// <summary>
        /// return true if the command is quit;
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        public bool ProcessCommand(string[] commands)
        {
            string mainCommand = commands[0];

            ConsoleStatus status = ConsoleStatus.OK;

            try
            {
                switch (mainCommand)
                {
                    case Quit: // Quit the program
                        return true;

                    case Save:
                        manager.Save();
                        Print("Tasks saved to disk ...");
                        break;

                    case HelpKey:
                        executor.PrintHelpMenu(commands);
                        break;

                    case Add: // Adding a new task
                        status = executor.AddNewTask(commands);
                        if (IsQuit(status))
                            return true;
                        break;

                    case CurTask:
                        executor.PrintCurrentTask();
                        break;

                    case PutOffCurrent:
                        executor.PutOffCurrentTask(commands);
                        break;

                    case ArchiveCurrent:
                        executor.ArchiveCurrentTask(commands);
                        break;

                    case Update:
                        manager.Update();
                        Print("Update Successful ...");
                        break;

                    case DeprioritizeCurrent:
                        manager.DeprioritizeCurrentTask();
                        Print($"Current Task '{manager.CurrentTask.Title}' Is Deprioritized ...");
                        break;

                    case PrioritizeCurrent:
                        manager.PrioritizeCurrentTask();
                        Print($"Current Task '{manager.CurrentTask.Title}' Is Prioritized ...");
                        break;

                    case UnscheduleCurrent:
                        manager.UnscheduleCurrentTask();
                        Print($"Current Task '{manager.CurrentTask.Title}' Is Unscheduled ...");
                        break;

                    case ScheduleCurrent:
                        status = executor.ScheduleCurrentTask();
                        if (IsQuit(status)) return true;
                        Print($"Current Task '{manager.CurrentTask.Title}' is successfully scheduled ...");
                        break;

                    case StretchCurrentDeadline:
                        status = executor.StretchCurrentTaskDeadline();
                        if (IsQuit(status)) return true;
                        break;

                    case ResetCurrentDeadline:
                        status = executor.ResetCurrentTaskDeadline();
                        if (IsQuit(status)) return true;
                        break;

                    case ResetCurrentBuffer:
                        status = executor.ResetCurrentTaskBuffer();
                        if (IsQuit(status)) return true;
                        break;

                    case EditCurrentTitle:
                        status = executor.EditCurrentTaskTitle();
                        if (IsQuit(status)) return true;
                        break;

                    case EditCurrentDescription:
                        status = executor.EditCurrentTaskDescription();
                        if (IsQuit(status)) return true;
                        break;

                    case ViewAllTasks:
                        executor.View_AllTasks(commands);
                        break;

                    case ViewSelect: // select certain tasks to view
                        executor.View_SelectedTasks(commands);
                        break;

                    case ViewArchive: // should implement -detail flag detector later
                        executor.View_Archives(commands);
                        break;

                    default:
                        throw new BadCommandException($"Command '{mainCommand}' is not recognized");
                }
            }
            catch (BadFileParsingException e)
            {
                PrintWarning(Help.CorruptFileInstruction);
                ZDebugUtil.PrintError(e);
            }
            catch (BadCommandException e)
            {
                PrintWarning(Help.BadCommandInstruction);
                ZDebugUtil.PrintError(e);
            }
            catch (Exception e)
            {
                ZDebugUtil.PrintError(e);
            }
            return false;
        }

        #endregion

        #region Life Cycle

        public ZCommandProcessor(ZTaskManager manager)
        {
            this.manager = manager;
            reader = new ZConsoleReader(manager);
            executor = new ZCommandExecutor(manager, reader);
        }

        #endregion
    }
}
