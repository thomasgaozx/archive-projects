using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace qdex.QdexCreator.Nodes
{
    public class StackNode : WrapperNode
    {
        public StackNode(XmlTextWriter writer) : base(writer, "stack") { }
    }
}
