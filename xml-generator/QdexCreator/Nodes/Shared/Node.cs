using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qdex.QdexCreator.Nodes
{
    public class Node : BaseNode
    {
        /// <summary>
        /// Should either be string or inner nodes;
        /// </summary>
        private List<object> InnerContent;

        public Node(XmlTextWriter writer, string tagName) : base(writer, tagName) { }

        public void AddToInnerContent(BaseNode node)
        {
            if (InnerContent==null)
            {
                InnerContent = new List<object>(1);
            }
            InnerContent.Add(node);
        }

        public void AddToInnerContent(string stringContent)
        {
            if (InnerContent == null)
            {
                InnerContent = new List<object>(1);
            }
            InnerContent.Add(stringContent);
        }

        /// <summary>
        /// A quick way to add a simple text node
        /// </summary>
        /// <param name="tagName"></param>
        /// <param name="content"></param>
        public void AddTextNodeToInnerContent(string tagName, string content)
        {
            AddToInnerContent(new TextNode(writer, tagName, content));
        }

        public void WriteInnerContent()
        {
            if (InnerContent!=null)
            {
                foreach (object o in InnerContent)
                {
                    if (o is INode)
                    {
                        (o as INode).WriteNode();
                    }
                    else if (o is string)
                    {
                        writer.WriteString(o as string);
                    }
                }
            }
        }

        public override void WriteNode()
        {
            writer.WriteStartElement(tagName);
            WriteAttributeList();
            WriteInnerContent();
            writer.WriteFullEndElement();
        }
    }
}
