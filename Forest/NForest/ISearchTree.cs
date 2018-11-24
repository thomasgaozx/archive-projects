using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forest
{
    public interface ISearchTree<T>
    {

        /// <summary>
        /// Inserting an object to the search tree.
        /// </summary>
        /// <param name="obj">The object being inserted</param>
        /// <returns>Returns whether the insertion is successful or not</returns>
        bool Insert(T obj);

        /// <summary>
        /// Remove an object from the sarchTree
        /// </summary>
        /// <param name="obj">The object being removed</param>
        /// <returns>Returns whether the removal is successful or not</returns>
        bool Remove(T obj);

        /// <summary>
        /// Returns true if the tree contains the object, 
        /// false otherwise.
        /// </summary>
        /// <param name="obj">The object being searched</param>
        /// <returns>true if the tree contains the object, false otherwise</returns>
        bool Contains(T obj);

        /// <summary>
        /// Checks whether the binary tree contains any entries.
        /// True if the binary tree contains entries, false otherwise.
        /// </summary>
        /// <returns>true if the tree contains entries, false otherwise</returns>
        bool Any();

        /// <summary>
        /// Clears the tree.
        /// </summary>
        void Clear();

        /// <summary>
        /// Finds the minimum value of the tree entries
        /// </summary>
        /// <returns>The minimum value, null if not found</returns>
        T Min();

        /// <summary>
        /// Finds the maximum value of the tree entries
        /// </summary>
        /// <returns>The maximum value, null if not found</returns>
        T Max();


    }
}
