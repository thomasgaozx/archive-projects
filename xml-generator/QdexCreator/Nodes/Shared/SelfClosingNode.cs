using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qdex.QdexCreator.Nodes
{
    public abstract class SelfClosingNode : BaseNode
    {

        public SelfClosingNode(XmlTextWriter writer, string tagName) : base(writer, tagName) { }
        
        public override void WriteNode()
        {
            writer.WriteStartElement(tagName);
            WriteAttributeList();
            writer.WriteEndElement();
        }
    }
}
