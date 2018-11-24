using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qdex.QdexCreator.Nodes
{
    public interface INode
    {
        void AddAttr(string key, string val);

        void WriteAttributeList();

        void WriteNode();
    }
}
