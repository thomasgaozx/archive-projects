using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace qdex.QdexCreator.Nodes
{
    /// <summary>
    /// Only allows nodes as innercontent
    /// </summary>
    public class WrapperNode : BaseNode
    {
        protected List<BaseNode> InnerNodes;

        public WrapperNode(XmlTextWriter writer, string tagName) : base(writer, tagName) { }

        public virtual void AddToInnerNodes(BaseNode node)
        {
            if (InnerNodes == null)
            {
                InnerNodes = new List<BaseNode>(1);
            }
            InnerNodes.Add(node);
        }

        public void WriteInnerNodes()
        {
            if (InnerNodes!=null)
            {
                foreach (BaseNode node in InnerNodes)
                {
                    node.WriteNode();
                }
            }
        }

        public override void WriteNode()
        {
            writer.WriteStartElement(tagName);
            WriteAttributeList();
            WriteInnerNodes();
            writer.WriteFullEndElement();
        }


        #region RandUtil

        private static void Swap<T>(IList<T> list, int index1, int index2)
        {
            if (index1 != index2)
            {

                T temp = list[index1];
                list[index1] = list[index2];
                list[index2] = temp;
            }
        }

        /// <summary>
        /// A driver for <see cref="Swap{T}(IList{T}, int, int)"/>. Used to randomly shuffle 
        /// the nodes.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="random"></param>
        public void RandSwap(Random random)
        {
            int size = InnerNodes.Count;
            int lim = size / 2 + 1;
            int swapTimes = lim > 10 ? 10 : lim;

            for (int i = 0; i < swapTimes; i++)
            {
                Swap(InnerNodes, random.Next(0, size), random.Next(0, size));
            }
        }

        #endregion

    }
}
