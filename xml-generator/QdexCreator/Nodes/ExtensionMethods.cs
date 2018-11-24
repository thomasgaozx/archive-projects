using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qdex.QdexCreator.Nodes
{
    public static class ExtensionMethods
    {

        /// <summary>
        /// Delegate for <see cref="WrapperNode.AddToInnerNodes(BaseNode)"/>. Added for syntactical fluidity
        /// </summary>
        /// <typeparam name="TNode">A WrapperNode Type</typeparam>
        /// <param name="wrapper">A WrapperNode object</param>
        /// <param name="node">A BaseNode object</param>
        /// <returns>The WrapperNode itself</returns>
        public static TNode AppendNode<TNode>(this TNode wrapper, BaseNode node) where TNode:WrapperNode
        {
            if (!(wrapper is DocumentRoot) || (node is SectionNode))
            {
                wrapper.AddToInnerNodes(node);
            }
            else
            {
                Console.WriteLine("Appending failed because the node appended is not sectionNode");
            }

            return wrapper;
        }

        /// <summary>
        /// Adding multiple nodes to a wrapper node
        /// </summary>
        /// <typeparam name="TNode">A Node of type <see cref="WrapperNode"/></typeparam>
        /// <param name="wrapper">Wrapper Node object</param>
        /// <param name="nodes">A number of BaseNode objects</param>
        /// <returns>The TNode object itself</returns>
        public static TNode AppendNodes<TNode>(this TNode wrapper, params BaseNode[] nodes) where TNode:WrapperNode
        {
            if (wrapper is DocumentRoot)
            {
                Console.WriteLine("DocumentRoot is not eligible for this method.");
            }
            else
            {
                foreach(var node in nodes)
                {
                    if (node is SectionNode)
                    {
                        Console.WriteLine("SectionNode cannot be added to random nodes");
                    }
                    else
                    {
                        wrapper.AddToInnerNodes(node);
                    }
                }
            }
            return wrapper;
        }

        /// <summary>
        /// Append Content to a <see cref="Node"/> object. (Can be either string or a node)
        /// </summary>
        /// <typeparam name="TNode">A <see cref="Node"/> type</typeparam>
        /// <param name="node">A <see cref="Node"/> object</param>
        /// <param name="other">Another baseNode object</param>
        /// <returns>The TNode object itself</returns>
        public static TNode AppendContent<TNode>(this TNode node, BaseNode other)
            where TNode : Node
        {
            node.AddToInnerContent(other);
            return node;
        }

        /// <summary>
        /// Append Content to a <see cref="Node"/> object. (Can be either string or a node)
        /// </summary>
        /// <typeparam name="TNode">A <see cref="Node"/> type</typeparam>
        /// <param name="node">A <see cref="Node"/> object</param>
        /// <param name="text">A string object</param>
        /// <returns>The TNode object itself</returns>
        public static TNode AppendContent<TNode>(this TNode node, string text)
            where TNode : Node
        {
            node.AddToInnerContent(text);
            return node;
        }

        /// <summary>
        /// A shortcut method for appending a textnode to a <see cref="Node"/> object
        /// </summary>
        /// <typeparam name="TNode">A <see cref="Node"/> type</typeparam>
        /// <param name="node">A <see cref="Node"/> object</param>
        /// <param name="tagName">A string object representing the tagname of the node</param>
        /// <param name="text">A string object representing the innerText of the node</param>
        /// <returns>The TNode object itself</returns>
        public static TNode AppendTextNode<TNode>(this TNode node, string tagName, string text)
            where TNode : Node
        {
            node.AddTextNodeToInnerContent(tagName, text);
            return node;
        }

    }
}
