using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forest
{
    public class AvlTree<T> : ISearchTree<T> where T:IComparable
    {

        #region Nested Type

        private class AvlNode
        {
            public AvlNode Left { get; set; }

            public AvlNode Right { get; set; }

            public int Height { get; set; }

            public T Value { get; set; }

            public AvlNode (T val, AvlNode left = null, AvlNode right = null)
            {
                Value = val;
                Left = left;
                Right = right;
                Height = 0;
            }

        }

        #endregion

        #region Private Field

        private AvlNode root;

        #endregion

        #region Public Property

        /// <summary>
        /// The maximum difference between the height of the left node
        /// and the height of the right node before the node is considered
        /// unbalanced. MUST be greater than 1.
        /// </summary>
        public int Tolerance { get; private set; }

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// node1 and node2 MUST NOT be null!
        /// </summary>
        private static void Swap(AvlNode node1, AvlNode node2)
        {
            AvlNode temp = new AvlNode(node1.Value, node1.Left, node1.Right);

            node1.Value = node2.Value;
            node1.Left = node2.Left;
            node1.Right = node2.Right;

            node2.Value = temp.Value;
            node2.Left = temp.Left;
            node2.Right = temp.Right;
        }

        /// <summary>
        /// node must NOT be null!
        /// </summary>
        /// <param name="node"></param>
        private static int GetLeftHeight(AvlNode node)
        {
            return node.Left == null ? -1 : node.Left.Height;
        }

        /// <summary>
        /// node must NOT be null!
        /// </summary>
        private static int GetRightHeight(AvlNode node)
        {
            return node.Right == null ? -1 : node.Right.Height;
        }

        /// <summary>
        /// Re-set the height of the node.
        /// Node MUST NOT be null!
        /// </summary>
        private static void UpdateHeight(AvlNode node)
        {
            int leftHeight = GetLeftHeight(node);
            int rightHeight = GetRightHeight(node);

            node.Height = (leftHeight > rightHeight ? leftHeight : rightHeight) + 1;
        }

        /// <summary>
        /// Assumption: node is NOT null, node.Left is NOT null;
        /// Rotate the node with its left sub node, proper height is reset;
        /// </summary>
        private static void RotateLeft(AvlNode node)
        {
            AvlNode temp = new AvlNode(node.Value, node.Left.Right, node.Right);
            
            node.Value = node.Left.Value;
            node.Left = node.Left.Left;
            node.Right = temp;

            // reset height
            UpdateHeight(node.Right);
            UpdateHeight(node);
        }

        /// <summary>
        /// Assumption: node is NOT null, node.Right is NOT null;
        /// Rotate the node with its right sub node, proper height is reset.
        /// </summary>
        private static void RotateRight(AvlNode node)
        {
            AvlNode temp = new AvlNode(node.Value, node.Left, node.Right.Left);

            node.Value = node.Right.Value;
            node.Right = node.Right.Right;
            node.Left = temp;

            // Reset Height
            UpdateHeight(node.Left);
            UpdateHeight(node);
        }

        /// <summary>
        /// Call this method only when you are sure that target is contained by one of parent's child
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="target"></param>
        private static void RemoveChildTarget(AvlNode parent, T target)
        {
            if (target.CompareTo(parent.Value) > 0)
                parent.Right = null;
            else
                parent.Left = null;
        }

        #endregion

        #region Methods

        public bool Insert(T obj)
        {
            if (root == null)
            {
                root = new AvlNode(obj);
                return true;
            }

            Stack<AvlNode> stack = new Stack<AvlNode>();
            AvlNode pos = root;

            // Finding the right insertion position.
            while (pos!=null)
            {
                int result = obj.CompareTo(pos.Value);

                if (result > 0)
                {
                    stack.Push(pos);

                    if (pos.Right == null)
                    {
                        pos.Right = new AvlNode(obj);
                        pos = pos.Right; // will be null if pos.Right is called again, quit the loop
                    }
                    pos = pos.Right;
                }
                else if (result < 0)
                {
                    stack.Push(pos);

                    if (pos.Left == null)
                    {
                        pos.Left = new AvlNode(obj);
                        pos = pos.Left; // will be null if pos.Left is called again, quit the loop
                    }
                    pos = pos.Left;
                }
                else
                    return false;
            }

            // Trace back, reset height and rotate once (if necessary) to achieve balance
            bool toContinue = true;
            while (stack.Any() && toContinue)
            {
                pos = stack.Pop();

                int difference = GetLeftHeight(pos) - GetRightHeight(pos);

                // Q1: Is it necessary to reset the height of position? [RESOLVED by proof]
                // Q2: Is it necessary to perform a lot of null checks? [RESOLVED]

                if (difference > Tolerance) // Left node has greater height than tolerance (at least 2), and therefore pos.Left cannot be null
                { 
                    if (GetLeftHeight(pos.Left) > GetRightHeight(pos.Left)) 
                        RotateLeft(pos);
                    else // Left right node has greater height than left left node. Therefore pos.Left.Right cannot be null
                    {
                        RotateRight(pos.Left); // pos.Left.Right is not null
                        RotateLeft(pos);
                    }
                    toContinue = false; // no need to continue after rebalancing -- backed by proof
                }
                else if (difference < -Tolerance) // Right node has greater height, therefore pos.Right cannot be null
                {
                    if (GetRightHeight(pos.Right) > GetLeftHeight(pos.Right))
                        RotateRight(pos);
                    else
                    {
                        RotateLeft(pos.Right);
                        RotateRight(pos);
                    }
                    toContinue = false;
                }
                else
                {
                    int oldHeight = pos.Height;
                    UpdateHeight(pos);
                    
                    if (pos.Height.Equals(oldHeight))
                        toContinue = false; // no need to continue if the pos's height does not change -- backed by proof
                }
            }

            return true;

        }

        /// <summary>
        /// o_X ... EWW!!!
        /// </summary>
        public bool Remove(T obj) 
        {
            if (root == null)
                return false;

            AvlNode pos = root;
            Stack<AvlNode> stack = new Stack<AvlNode>();

            // Finding the target to be removed
            bool notFound = true;
            while (notFound)
            {
                int result = obj.CompareTo(pos.Value);

                if (result > 0)
                {
                    if (pos.Right == null)
                        return false;
                    stack.Push(pos);
                    pos = pos.Right;
                }
                else if (result < 0)
                {
                    if (pos.Left == null)
                        return false;
                    stack.Push(pos);
                    pos = pos.Left;
                }
                else
                    notFound = false; // position is now the value to be removed
            }
            AvlNode removeTarget = pos;
            
            // Remove Node: always push the parent node before the physically-removed node
            if (pos.Left==null && pos.Right==null)
            {
                AvlNode parent = stack.Pop();
                RemoveChildTarget(parent, pos.Value);
                stack.Push(parent); // not optimal but elegant enough
            }
            else if (pos.Left==null) // Right branch exists, which should only be a leaf, otherwise the original tree won't be balanced 
            {
                // remove pos and concat right branch
                pos.Value = pos.Right.Value;
                // pos.Left = pos.Right.Left is not necessary
                pos.Right = null; // because the original pos.Right is a leaf
                stack.Push(pos);
            }
            else if (pos.Right==null)
            {
                pos.Value = pos.Left.Value;
                pos.Left = null;
                stack.Push(pos);
            }
            else // Two branches exist x__x
            {
                stack.Push(pos);
                pos = pos.Left;

                while (pos.Right != null)
                {
                    stack.Push(pos);
                    pos = pos.Right;
                }

                removeTarget.Value = pos.Value;

                // remove node using parent
                AvlNode parent = stack.Pop();
                RemoveChildTarget(parent, pos.Value);
                stack.Push(parent);
            }

            // Re-setting height and rebalancing
            bool toContinue = true;
            while(stack.Any() && toContinue)
            {
                pos = stack.Pop();
                UpdateHeight(pos);

                int diff = GetLeftHeight(pos) - GetRightHeight(pos);

                if (diff>Tolerance) // more on left branch, pos.Left is not empty
                {
                    if (GetLeftHeight(pos.Left) > GetRightHeight(pos.Left))
                        RotateLeft(pos);
                    else // greater height on pos->left->right than pos->left->left, pos.Left.Right is not empty.
                    {
                        RotateRight(pos.Left);
                        RotateLeft(pos);
                    }
                    // cannot terminate balancing after rotations the height of the pos won't go back to the original height before removal
                }
                else if (diff < -Tolerance) // more on right branch, pos.Right is not empty
                {
                    if (GetRightHeight(pos.Right) > GetLeftHeight(pos.Right))
                        RotateRight(pos);
                    else
                    {
                        RotateLeft(pos.Right);
                        RotateRight(pos);
                    }
                }
                else
                {
                    int oldHeight = pos.Height; // update before rotation? [NO]
                    UpdateHeight(pos);
                    if (pos.Height.Equals(oldHeight))
                        toContinue = false; // backed by proof
                }
            }
            return true;

        }

        public bool Contains(T obj)
        {
            AvlNode pos = root;

            while (pos!=null)
            {
                int result = obj.CompareTo(pos.Value);

                if (result > 0)
                    pos = pos.Right;
                else if (result < 0)
                    pos = pos.Left;
                else
                    return true;
            }

            return false;
        }

        public bool Any()
        {
            return root != null;
        }

        public void Clear()
        {
            root = null;
        }

        public T Min()
        {
            if (root == null)
                throw new Exception("The tree is empty!");

            AvlNode pos = root;

            while (pos.Left != null)
                pos = pos.Left;

            return pos.Value;
        }

        public T Max()
        {
            if (root == null)
                throw new Exception("The tree is empty!");

            AvlNode pos = root;

            while (pos.Right != null)
                pos = pos.Right;

            return pos.Value;
        }

        #endregion

        #region Life Cycle 

        public AvlTree(int tolerance=0)
        {
            root = null;
            Tolerance = tolerance > 1 ? tolerance : 1;
        }

        #endregion
    }
}
