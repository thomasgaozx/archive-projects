using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace qdex.QdexCreator.Nodes
{
    public sealed class DocumentRoot : WrapperNode
    {
        #region Private Fields

        private Node metadata;

        #endregion

        #region Public Properties

        public string Title { get; set; }
        public string Creator { get; set; }
        public string Abstract { get; set; }
        //public string CoverImageUrl { get; set; } To be implemented

        #endregion

        #region Methods
        public SectionNode Section(int index)
        {
            if (index>InnerNodes.Count())
            {
                throw new Exception("Section does not exist");
            }
            else
            {
                return (SectionNode)InnerNodes[index];
            }
        }

        public DocumentRoot AddSection(string title = null)
        {
            SectionNode newSection = new SectionNode(writer, this)
            {
                Title = title
            };
            return this;
        }


        public void WriteSectionNodes()
        {
            if (InnerNodes != null)
            {
                int listLen = InnerNodes.Count();
                for (int i=0; i<listLen; i++)
                {
                    SectionNode node = (SectionNode)InnerNodes[i];
                    if (node.Title==null)
                    {
                        writer.WriteComment(" Section ");
                    }
                    else
                    {
                        writer.WriteComment($" Section {i} ");
                    }
                    node.WriteNode();
                }
            }
        }

        #endregion

        #region Life Cycle

        public DocumentRoot(XmlTextWriter writer) : base(writer,"document")
        {
            // Default values for instances - to be removed
            Title = "Test Module";
            Creator = "Thomas Gao";
            Abstract = "DSFJHJDKSHFLJDKSHFJFLJDSHFJKSDHFL JDHFS DHSFK DSHDSDLSFDLALSSSFJDLSHFJDSHFJDSLHFJDSLHFJDSLHFJDSHLFJDSHFJDSKFLJDSHFKDSJFHSDJ";

            // Set document root attributes
            AddAttr("id", "G"+Guid.NewGuid().ToString().Replace("-", "_").ToUpper());
            AddAttr("name", "myDocument");
            AddAttr("xmlns", "http://resources.qdexapps.com/schema/v1/QDocument.xsd");

            // Set metadata
            metadata = new Node(writer, nameof(metadata));
            metadata.AppendTextNode(nameof(Title).ToLower(), Title);
            metadata.AppendTextNode(nameof(Creator).ToLower(), Creator);
            metadata.AppendTextNode(nameof(Abstract).ToLower(), Abstract);
        }

        public DocumentRoot(XmlTextWriter writer, string title, string creator, string _abstract) : base(writer, "document")
        {
            // Default values for instances
            Title = title;
            Creator = creator;
            Abstract = _abstract;

            // Set document root attributes
            AddAttr("id", "G"+Guid.NewGuid().ToString().Replace("-","_").ToUpper());
            AddAttr("name", "myDocument");
            AddAttr("xmlns", "http://resources.qdexapps.com/schema/v1/QDocument.xsd");

            // Set metadata
            metadata = new Node(writer, nameof(metadata));
            metadata.AppendTextNode(nameof(Title).ToLower(), Title);
            metadata.AppendTextNode(nameof(Creator).ToLower(), Creator);
            metadata.AppendTextNode(nameof(Abstract).ToLower(), Abstract);
        }

        public override void WriteNode()
        {
            // Set XmlTextWriter Format
            writer.WriteStartDocument();
            writer.Formatting = Formatting.Indented;
            writer.Indentation = 2;

            // document tag
            writer.WriteStartElement(tagName);
            WriteAttributeList();

            // Write Metadata
            metadata.WriteNode();

            WriteSectionNodes();

            writer.WriteEndElement();

            writer.WriteEndDocument();
        }

        #endregion

    }
}
