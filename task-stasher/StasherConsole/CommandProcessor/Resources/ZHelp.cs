using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static TaskStasher.ZConsole.ConsoleSyntax.ZConsoleSyntax;

namespace TaskStasher.ZConsole.CommandProcessor.Resources
{
    public static class ZHelp
    {
        public static Dictionary<String, String> commands = new Dictionary<string, string>
        {
            { Add, "" }
        };
    }
}
