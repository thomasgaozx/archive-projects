using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qdex.QdexCreator.Nodes
{
    public class Attr
    {
        public string Key { get; private set; }

        public string Value { get; set; }

        public Attr(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
