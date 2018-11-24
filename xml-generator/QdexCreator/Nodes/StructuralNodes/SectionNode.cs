using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace qdex.QdexCreator.Nodes
{
    public sealed class SectionNode : WrapperNode
    {
        private TextNode titleNode;

        /// <summary>
        /// The Title of the section.
        /// The reason this property is special is that it controls the tag name of the sectionNode.
        /// </summary>
        public string Title
        {
            get
            {
                return titleNode?.Text;
            }
            set // setting the content of titleNode
            {
                string content = value;
                if (content==null)
                {
                    titleNode = null; //set to null
                } 
                else if (titleNode==null)
                {
                    titleNode = new TextNode(writer, "title", content); //adding title
                }
                else
                {
                    titleNode.Text = content; //changing title content
                }
            }
        }

        #region Life Cycle

        /// <summary>
        /// The tagname of this class is determined at run time and is thus set to null.
        /// The section is auto-appended to the root.
        /// </summary>
        /// <param name="writer"></param>
        public SectionNode(XmlTextWriter writer, DocumentRoot root) : base(writer, null)
        {
            root.AppendNode(this);
        }

        public override void WriteNode()
        {
            bool titleExists = titleNode != null;

            writer.WriteStartElement(titleExists?"section":"sectionNoTitle");
            WriteAttributeList();

            if (titleExists)
            {
                titleNode.WriteNode();
            }

            WriteInnerNodes();
            writer.WriteFullEndElement();
        }

        #endregion
    }
}
