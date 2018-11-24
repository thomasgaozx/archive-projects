using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskStasher.ZConsole.CommandProcessor.Resources
{
    public abstract class ZBaseCommandNode
    {
        public string Key { get; private set; }

        public string Description { get; private set; }

        public ZBaseCommandNode(string key, string description)
        {
            Key = key;
            Description = description;
        }
    }
}
