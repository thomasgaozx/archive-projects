using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskStasher.Control.Core;
using static TaskStasher.ZConsole.VisualFlair.ConsoleFormat;
using static TaskStasher.ZConsole.ConsoleSyntax.ZConsoleSyntax;
namespace TaskStasher.ZConsole.CommandProcessor.Resources
{
    public class ZCommandMap
    {

        /// <summary>
        /// A list of sorted commands from a to z.
        /// </summary>
        private readonly List<ZCommandNode> commands;

        public ZCommandNode this[string command]
        {
            get
            {
                int rightIndex = ZAlgorithms.BinarySearch(commands, c => c.Key, command, (a, b) => a.CompareTo(b));
                if (rightIndex.Equals(-1))
                {
                    return null;
                }
                return commands[rightIndex];
            }
        }

        public IEnumerable<string> GetAllCommandKeys()
        {
            return commands.Select(c => c.Key);
        }

        public IEnumerable<ZCommandNode> GetAllCommands()
        {
            return commands;
        }
        
        public void PrintAllCommands()
        {
            foreach (var command in commands)
            {
                PrintCommandNode(command);
            }
        }

        public void PrintCommandNode(ZCommandNode command)
        {
            Prompt($"'{command.Key}': ");
            Print(command.Description);

            var flags = command.GetAllFlags();
            if (flags!=null)
            {
                foreach (var flag in flags)
                {
                    Prompt($"\t'{flag.Key}'[FLAG]: ");
                    Print(flag.Description);
                }

            }
        }

        public ZCommandMap()
        {
            commands = new List<ZCommandNode>()
            {
                new ZCommandNode(Add, "Adding a new task.",
                    new ZFlagNode(ScheduledFlag, "The new task should be a scheduled task."),
                    new ZFlagNode(PriorityFlag, "The new task should be a prioritized task.")),
                new ZCommandNode(ViewArchive, "View the tasks that are archived.",
                    new ZFlagNode(Cat_Done, "Tasks that are done."),
                    new ZFlagNode(Cat_Cancelled, "Cancelled tasks."),
                    new ZFlagNode(Cat_Obsolete, "The obsolete tasks.")),
                new ZCommandNode(PutOffCurrent, "Put off the current task to the back of the stack.",
                    new ZFlagNode(ToBottomFlag, "Put of the current task to bottom."),
                    new ZFlagNode("Any integer number", "A number of stacks that the current task should be delayed to.")),
                new ZCommandNode(ArchiveCurrent, "Archive the current task. Must use a category flag.",
                    new ZFlagNode(Cat_Done, "Tasks is done."),
                    new ZFlagNode(Cat_Cancelled, "Task is cancelled."),
                    new ZFlagNode(Cat_Obsolete, "Task is obsolete.")),
                new ZCommandNode(DeprioritizeCurrent, "Strip the priority off the current task."),
                new ZCommandNode(PrioritizeCurrent, "Make current task prioritized."),
                new ZCommandNode(ScheduleCurrent, "If the current task is not scheduled, schedule the current task."),
                new ZCommandNode(UnscheduleCurrent, "If the current task is scheduled, make it not scheduled."),
                new ZCommandNode(StretchCurrentDeadline, "Stretch the deadline of current task, if the current task is scheduled."),
                new ZCommandNode(ResetCurrentDeadline, "Reset the deadline of current task, if the current task is scheduled."),
                new ZCommandNode(ResetCurrentBuffer, "Reset the deadline buffer (the time span within which the task becomes urgent)."),
                new ZCommandNode(EditCurrentTitle, "Edit the title of current task."),
                new ZCommandNode(EditCurrentDescription, "Edit the description of current task."),
                new ZCommandNode(Save, "Save all tasks to disk."),
                new ZCommandNode(CurTask, "View the current task."),
                new ZCommandNode(Update, "Update the the tasks to pull the urgent task onto the top of the stack."),
                new ZCommandNode(Quit, "Quits the program, can be called any time (even during a console workflow)"),
                new ZCommandNode(Cancel, "Cancels the current operation / console workflow."),
                new ZCommandNode(StartMarker, "Start a multi-line console input block."),
                new ZCommandNode(EndMarker, "Ends a multi-line console input block and submit the input."),
                new ZCommandNode(HelpKey, "Show the help session, type in any command keyword after it to see its detailed description.", 
                    new ZFlagNode(KeywordFlag, "view the keywords ONLY.")),
            };

            ZAlgorithms.QuickSort(commands, (c1, c2) => (c1.Key.CompareTo(c2.Key)));
        }

    }
}
