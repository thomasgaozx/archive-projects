using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskStasher.Control.Core;
using static TaskStasher.ZConsole.ConsoleSyntax.ZConsoleSyntax;
using static TaskStasher.ZConsole.VisualFlair.ConsoleFormat;
namespace TaskStasher.ZConsole
{
    public class Shell
    {
        #region Init

        /// <summary>
        /// Set up the program witht he proper configuration
        /// </summary>
        public static void Init(ZTaskManager manager, ZCommandProcessor processor)
        {
            try // use a try here such that if the file reading fails the program won't break
            {
                Print(File.ReadAllText("introduction.txt"));
            }
            catch (Exception e)
            {
                ZDebugUtil.PrintError(e);
            }

            Print("Loading ...");

            manager.LoadFromDisk();
        }

        #endregion

        #region Shell

        public static void Main(String[] args)
        {
            ZTaskManager manager= new ZTaskManager();
            ZCommandProcessor processor= new ZCommandProcessor(manager);
            Init(manager,processor);
            Print("Loading Complete ...\n");

            bool quit = false;
            while (!quit)
            {
                Prompt("> ");
                string command = Console.ReadLine().Purify();
                if (!String.IsNullOrEmpty(command))
                {
                    var commands = command.Split(null);
                    quit = processor.ProcessCommand(commands);
                }
            }

            Print("Program ends ...");
            Thread.Sleep(3000);
        }

        #endregion

    }
}
