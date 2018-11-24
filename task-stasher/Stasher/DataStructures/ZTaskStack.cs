using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskStasher.Control.Core
{
    /// <summary>
    /// ZTaskStack is a special type of stack used specifically for ITask objects. 
    /// The implementation of the stack is achieved 
    /// through singly linked list for insertion efficiency.
    /// </summary>
    public class ZTaskStack
    {

        #region Nested Type

        private class Node
        {
            public ITask task;
            public Node link;

            public Node()
            {
                task = null;
                link = null;
            }

            public Node(ITask task, Node link)
            {
                this.task = task;
                this.link = link;
            }
        }

        #endregion

        #region Private Field

        private Node head;

        #endregion

        #region Methods

        public bool Empty()
        {
            return head == null;
        }

        public ZTaskStack Push(ITask task)
        {
            head = new Node(task, head);
            return this;
        }

        public ZTaskStack PushAll(ICollection<ITask> tasks)
        {
            foreach(ITask task in tasks)
            {
                Push(task);
            }
            return this;
        }

        public ITask Pop()
        {
            CheckHeadNullity();

            ITask task = head.task;
            head = head.link;
            return task;
        }

        public ITask Peek()
        {
            CheckHeadNullity();
            return head.task;
        }

        public void DelayCurrentTaskBy(int limit)
        {
            if (limit>0)
            {
                CheckHeadLinkNullity();

                Node currentNode = head;
                head = head.link; // move head by one offset
                currentNode.link = null;

                Node position = head;
                for (int i = 1; i < limit && position.link != null; i++)
                {
                    position = position.link;
                }

                // Insertion
                Node temp = position.link;
                position.link = currentNode;
                currentNode.link = temp;

            }
        }

        public void DelayCurrentTaskToBottom()
        {
            CheckHeadLinkNullity();

            Node currentNode = head;
            head = head.link;
            currentNode.link = null;

            Node position = head;
            while (position.link != null)
            {
                position = position.link;
            }

            position.link = currentNode;
        }

        public List<ITask> PeekTasks(int max)
        {
            CheckHeadNullity();

            int i = 0;

            List<ITask> tasklist = new List<ITask>(max);
            Node position = head;

            while (position.link != null && i < max)
            {
                tasklist.Add(position.task);

                position = position.link;
                ++i;
            }

            return tasklist;
        }

        public List<ITask> PeekAll()
        {
            List<ITask> tasklist = new List<ITask>();
            if (!Empty())
            {
                Node position = head;

                while (position.link != null)
                {
                    tasklist.Add(position.task);
                    position = position.link;
                }
                tasklist.Add(position.task); // Adding the last entry
            }

            return tasklist;
        }

        public int Count()
        {
            if (head == null)
            {
                return 0;
            }
            else
            {
                int i = 1;
                Node position = head;
                while (position.link != null)
                {
                    ++i;
                    position = position.link;
                }
                return i;
            }
        }

        /// <summary>
        /// Removes the link of a node and return that removed link
        /// </summary>
        private static Node PopLink(Node node)
        {
            Node temp = node.link;
            node.link = node.link.link;
            temp.link = null;
            return temp;
        }

        /// <summary>
        /// Private shared code
        /// </summary>
        /// <param name="condition"></param>
        private void PullTasksWith(Func<ZScheduledTask,bool> condition)
        {
            if (!(head == null || head.link == null))
            {
                List<Node> urgentTasks = new List<Node>();

                Node position = head;
                while (position.link != null)
                {
                    ZScheduledTask task = position.link.task as ZScheduledTask;
                    if (task != null && condition(task))
                    {
                        urgentTasks.Add(PopLink(position));
                    }
                    else
                    {
                        position = position.link;
                    }
                }

                if (urgentTasks.Any())
                {
                    foreach (Node node in urgentTasks)
                    {
                        // stacking to the top
                        node.link = head;
                        head = node;
                    }
                }
            }
        }

        /// <summary>
        /// Pulls all urgent tasks and stack them to the top
        /// </summary>
        public void PullUrgentTasks()
        {
            PullTasksWith(task => task.IsUrgent());
        }

        /// <summary>
        /// Pulls all the danger tasks and stack them to the top
        /// </summary>
        public void PullDangerTasks()
        {
            PullTasksWith(task => task.IsInDanger());
        }

        #endregion

        #region Private Debug Helper

        private void CheckHeadNullity()
        {
            if (head==null)
            {
                throw new NullReferenceException("The task stack is empty!");
            }
        }

        /// <summary>
        /// Checks both the nullity of head and head's link
        /// </summary>
        private void CheckHeadLinkNullity()
        {
            CheckHeadNullity();
            if (head.link == null)
            {
                throw new Exception("There is only one task in the stack, cannot delay current task.");
            }
        }

        #endregion

        #region Life Cycle

        public ZTaskStack()
        {
            head = null;
        }

        #endregion
    }
}
