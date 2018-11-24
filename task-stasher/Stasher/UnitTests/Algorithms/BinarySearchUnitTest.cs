using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskStasher.Control.Core.UnitTests.Algorithms
{
    [TestFixture]
    public class BinarySearchUnitTest
    {

        private static readonly object[] bs_int_case = new object[]
        {
            new object[] { new int[] { 1, 2, 3, 4, 5, 7, 9, 10 }, 4, 3 },
            new object[] { new int[] { 1, 2, 3, 4, 5, 6 ,8 }, 1, 0 },
            new object[] { new int[] { 1, 2, 3, 4, 5, 7, 10 }, 10, 6 },
            new object[] { new int[] { 1, 2, 3, 4, 5, 8, 11, 14 }, 14, 7 },
            new object[] { new int[] { 1, 3, 4, 5 }, 4, 2 },
            new object[] { new int[] { 1, 2 }, 1, 0 },
            new object[] { new int[] { 1, 2 }, 2, 1 },
            new object[] { new int[] { 1, 2 }, 0, -1 },
            new object[] { new int[] { 1, 2 }, 3, -1 }
        };


        [TestCaseSource(nameof(bs_int_case))]
        public void BinarySearch(int[] list,int target, int expected)
        {
            // Act
            int actual = ZAlgorithms.BinarySearch(list, i => i, target, new Comparison<int>((a, b) => a.CompareTo(b)));

            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        } 

    }
}
