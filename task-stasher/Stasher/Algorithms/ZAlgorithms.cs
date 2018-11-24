using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskStasher.Control.Core
{
    public static class ZAlgorithms
    {
        private static void Swap<T>(this IList<T>list, int i, int j)
        {
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

        /// <summary>
        /// Modifies the list;
        /// </summary>
        public static void QuickSort<T>(IList<T> list, Comparison<T> comparison)
        {
            Action<int, int> Sort = null;
            Sort = (start, end) =>
            {
                if (start<end)
                {
                    int i = start, j = end;
                    int pIndex = j--;
                    T pivot = list[pIndex]; // pick the last one as pivot for efficiency

                    // Working towards the right position
                    while (i < j)
                    {
                        int compareResult = comparison.Invoke(pivot, list[i]);
                        if (compareResult > 0)
                        {
                            ++i;
                        }
                        else
                        {
                            list.Swap(i, j--);
                        }
                    }

                    // Insert the pivot
                    if (comparison.Invoke(pivot, list[i]) > 0)
                    {
                        if (++i!=pIndex)
                        {
                            list.Swap(pIndex, i);
                        }
                    }
                    else
                    {
                        list.Swap(pIndex, i);
                    }

                    // Now pivot shall be in the right position.
                    Sort(i+1, end);
                    Sort(start, i-1);
                }
            };

            Sort(0, list.Count - 1);
        }

        /// <summary>
        /// Returns the index of the target in the list, of target is not found, -1 is returned.
        /// e.g. search 5 in {1, 3, 5, 7, 9} would return 2;
        /// </summary>
        /// <typeparam name="T">The Type of which the objects in the list has</typeparam>
        /// <typeparam name="E">A type that can be produced by object  of type T through select function</typeparam>
        /// <param name="list">The list that contains objects of type T</param>
        /// <param name="select">The function that takes in a T object and returns a E object</param>
        /// <param name="comparison">A comparison of the E object</param>
        /// <returns></returns>
        public static int BinarySearch<T,E>(IList<T> list, Func<T,E> select, E target, Comparison<E> comparison)
        {
            int low = 0, high = list.Count-1;

            if (select(list[high]).Equals(target))
            {
                return high;
            }

            do
            {
                int mid = (low + high) / 2;
                E val = select(list[mid]);
                int comparedResult = comparison.Invoke(target,val);

                if (comparedResult == 0)
                {
                    return mid;
                }
                else if (comparedResult > 0)
                {
                    low = mid+1;
                }
                else
                {
                    high = mid;
                }
            } while (low < high);

            return -1;
        }
    }
}
