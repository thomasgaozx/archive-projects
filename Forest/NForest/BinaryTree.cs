using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forest
{
    public class BinaryTree<T> : ISearchTree<T> where T:IComparable
    {

        #region Nested Type 

        private class BinaryNode
        {
            public T Value { get; set; }

            public BinaryNode Left { get; set; }

            public BinaryNode Right { get; set; }

            public BinaryNode(T val, BinaryNode left=null, BinaryNode right=null)
            {
                Value = val;
                Left = left;
                Right = right;
            }
        }

        #endregion

        #region Private Field

        private BinaryNode root;

        #endregion

        public bool Any()
        {
            return root != null;
        }

        public void Clear()
        {
            root = null;
        }

        public bool Contains(T obj)
        {
            BinaryNode position = root;

            while (position!=null)
            {
                int result = obj.CompareTo(position.Value);

                if (result > 0)
                    position = position.Right;
                else if (result < 0)
                    position = position.Left;
                else
                    return true;
            }

            return false;
        }

        public bool Insert(T obj)
        {

            BinaryNode position = root;
            BinaryNode newNode = new BinaryNode(obj);

            if (position==null)
            {
                root = newNode;
                return true;
            }

            int result=0;
            do
            {
                result = obj.CompareTo(position.Value);

                if (result > 0)
                {
                    if (position.Right == null)
                    {
                        position.Right = newNode;
                        return true;
                    }
                    position = position.Right;
                }
                else if (result < 0)
                {
                    if (position.Left == null)
                    {
                        position.Left = newNode;
                        return true;
                    }
                    position = position.Left;
                }

            } while (result != 0);

            return false;

        }

        public T Max()
        {
            if (root == null)
                throw new Exception("Tree is empty!");

            BinaryNode pos = root;

            while (pos.Right != null)
                pos = pos.Right;

            return pos.Value;
        }

        public T Min()
        {
            if (root == null)
                throw new Exception("Tree is empty!");

            BinaryNode pos = root;

            while (pos.Left != null)
                pos = pos.Left;

            return pos.Value;
        }

        /// <summary>
        /// Assume that node is not leaf! Need to do additional 
        /// check before calling this method.
        /// </summary>
        private static void RemoveNode(BinaryNode node)
        {
            if (node.Left == null) // only right branch exists
            {
                node.Value = node.Right.Value;
                node.Left = node.Right.Left;
                node.Right = node.Right.Right;
            }
            else if (node.Right == null) // only left branch exists
            {
                node.Value = node.Left.Value;
                node.Right = node.Left.Right;
                node.Left = node.Left.Left;
            }
            else // both branches exists
            {
                BinaryNode parent = null;
                BinaryNode position = node.Left;

                while (position.Right!=null)
                {
                    parent = position;
                    position = position.Right;
                }

                if (parent == null) // the left node of node is the largest value of the left branch
                {
                    node.Value = node.Left.Value;
                    node.Left = node.Left.Left;
                    return;
                }

                node.Value = position.Value;
                parent.Right = position.Left;
            }
        }

        private static bool IsLeaf(BinaryNode node)
        {
            return node.Left == null && node.Right == null;
        }

        public bool Remove(T obj)
        {
            if (root == null)
                return false;
            else if (obj.Equals(root.Value)) {
                root = null;
                return true;
            }
            else
            {
                BinaryNode position = root;
                BinaryNode parent = null;

                while (position!=null)
                {
                    int result = obj.CompareTo(position.Value);

                    if (result > 0)
                    {
                        parent = position;
                        position = position.Right;
                    }
                    else if (result < 0)
                    {
                        parent = position;
                        position = position.Left;
                    }
                    else if (IsLeaf(position))
                    {
                        if (parent.Left!=null && parent.Left.Value.Equals(position.Value))
                            parent.Left = null;
                        else
                            parent.Right = null;
                        return true;
                    }
                    else
                    {
                        RemoveNode(position);
                        return true;
                    }
                }

                return false;
                
            }
        }

        #region Life Cycle

        public BinaryTree()
        {
            root = null;
        }

        #endregion
    }
}
