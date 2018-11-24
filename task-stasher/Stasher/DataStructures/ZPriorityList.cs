using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskStasher.Control.Core
{
    /// <summary>
    /// A sorted list of scheduled tasks. Everytime pop() is called, the most urgent task will be popped.
    /// </summary>
    public class ZPriorityList
    {

        #region Private Fields

        /*The smaller the urgent date is, the more urgent that task is. 
         Thus the smaller urgent date shall be placed to the end*/
        private bool sorted = false;
        private List<ZScheduledTask> priorityList = new List<ZScheduledTask>(); // urgent tasks placed towards the end for efficiency

        #endregion

        #region Private Helper Methods

        private void SortIfNotAlready()
        {
            if (!sorted)
            {
                // Quick Sort
                ZAlgorithms.QuickSort(priorityList, (x, y) => (y.GetUrgentDate().CompareTo(x.GetUrgentDate())));
                // Done this way because I want the most urgent date to be at the end

                sorted = true;
            }
        }

        #endregion

        #region Methods

        public bool Any()
        {
            return priorityList.Any();
        }

        /// <summary>
        /// The reason that insertion sort is not used here is because of the fact insertion takes
        /// linear time in addition to the logrithmic time taken by binary search
        /// </summary>
        public ZPriorityList Push(ZScheduledTask task)
        {
            sorted = false;
            priorityList.Add(task);
            return this;
        }

        public int Length => priorityList.Count;

        /// <summary>
        /// Returns the most urgent task and removes it from the list;
        /// Using quick sort before popping. 
        /// </summary>
        /// <returns></returns>
        public ZScheduledTask Pop()
        {

            SortIfNotAlready();
            ZScheduledTask item = priorityList[priorityList.Count - 1];
            priorityList.RemoveAt(priorityList.Count - 1);
            return item;
        }

        /// <summary>
        /// Returns the most urgent task but not remove it from the list;
        /// </summary>
        /// <returns></returns>
        public ZScheduledTask Peek()
        {
            SortIfNotAlready();
            return priorityList[priorityList.Count - 1];
        }


        public List<ZScheduledTask> PeekAll()
        {
            SortIfNotAlready();

            return priorityList;
        }

        #endregion

    }
}
