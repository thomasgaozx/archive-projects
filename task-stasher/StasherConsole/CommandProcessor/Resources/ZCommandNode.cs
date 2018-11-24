using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskStasher.ZConsole.CommandProcessor.Resources
{
    public class ZCommandNode : ZBaseCommandNode
    {
        private List<ZFlagNode> flags;

        public void AddFlags(params ZFlagNode[] flags)
        {
            this.flags.AddRange(flags);
        }

        public List<ZFlagNode> GetAllFlags()
        {
            return flags;
        }

        public ZCommandNode(string key, string description) : base(key, description)
        {
            List<ZFlagNode> nodes = new List<ZFlagNode>(2);
        }

        public ZCommandNode(string key, string description, params ZFlagNode[] zFlags) : base(key, description)
        {
            flags = zFlags.ToList();
        }
    }
}
