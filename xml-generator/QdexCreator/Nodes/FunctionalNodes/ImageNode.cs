using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace qdex.QdexCreator.Nodes
{
    public class ImageNode : SelfClosingNode
    {
        private Attr srcAttr = new Attr("src", null);

        private Attr styleAttr = new Attr("style", null);

        private static readonly HashSet<string> ValidStyleSet
            = new HashSet<string>()
            {
                "xlarge",
                "large",
                "medium",
                "small",
                "tiny"
            };

        public string Src
        {
            get
            {
                return srcAttr.Value;
            }
            set
            {
                srcAttr.Value = value;
            }
        }

        public string Style
        {
            get
            {
                return styleAttr.Value;
            }
            set
            {
                string temp = value;
                if (ValidStyleSet.Contains(temp))
                {
                    styleAttr.Value = temp;
                }
                else
                {
                    throw new Exception("This is not a valid style.");
                }
            }
        }

        public ImageNode(XmlTextWriter writer) : base(writer, "image")
        {
            AddAttr(srcAttr);
            AddAttr(styleAttr);
        }
    }
}
