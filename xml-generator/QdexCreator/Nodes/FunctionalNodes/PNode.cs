using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace qdex.QdexCreator.Nodes
{
    public class PNode : Node
    {
        public string List;

        public PNode(XmlTextWriter writer) : base(writer, "p") { }
    }
}
