using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qdex.QdexCreator.Nodes
{
    public abstract class BaseNode : INode
    {
        protected string tagName;
        protected XmlTextWriter writer;

        private List<Attr> AttributeList;

        public BaseNode(XmlTextWriter writer, string tagName)
        {
            this.writer = writer;
            this.tagName = tagName;
        }

        public void AddAttr(string key, string val)
        {
            if (AttributeList==null)
            {
                AttributeList = new List<Attr>(1);
            }
            AttributeList.Add(new Attr(key, val));
        }

        public void AddAttr(Attr newAttribute)
        {
            if (AttributeList == null)
            {
                AttributeList = new List<Attr>(1);
            }
            AttributeList.Add(newAttribute);
        }

        public void WriteAttributeList()
        {
            if (AttributeList != null)
            {
                foreach (Attr attribute in AttributeList)
                {
                    if (!(attribute.Value==null || attribute.Value=="") )
                    {
                        writer.WriteAttributeString(attribute.Key, attribute.Value);
                    }
                }
            }
        }

        public abstract void WriteNode();

    }
}
