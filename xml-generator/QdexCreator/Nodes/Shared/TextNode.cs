using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace qdex.QdexCreator.Nodes
{
    public class TextNode : BaseNode
    {
        public string Text { get; set; }
        
        public TextNode(XmlTextWriter writer, string tagName) : base(writer, tagName) { }

        public TextNode(XmlTextWriter writer, string tagName, string text) : base(writer, tagName)
        {
            Text = text;
        }

        public override void WriteNode()
        {
            writer.WriteStartElement(tagName);
            WriteAttributeList();
            writer.WriteString(Text);
            writer.WriteFullEndElement();
        }
    }
}
