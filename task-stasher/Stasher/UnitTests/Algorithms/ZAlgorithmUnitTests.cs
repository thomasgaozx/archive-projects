using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskStasher.Control.Core.UnitTests
{
    [TestFixture]
    public class ZAlgorithmUnitTests
    {
        private static List<int> integerList;
        private static List<DateTime> dateList;
        private static List<string> stringList;
        private static List<double> doubleList;
        

        [OneTimeSetUp]
        public void SetUpFixture()
        {
            integerList = new List<int> { 1, 3, 2, 4, 8, 10, 25, 43, 32, 26, 90, 108, 22 };
            dateList = new List<DateTime>
            {
                new DateTime(1998,10,21,17,45,0),
                new DateTime(1998,10,23,15,45,0),
                new DateTime(2098,10,22,14,45,0),
                new DateTime(1999,12,24,13,45,0),
                new DateTime(1996,10,25,15,45,0),
                new DateTime(1992,10,29,14,45,0),
                new DateTime(1994,11,27,15,45,0),
                new DateTime(1993,10,26,16,45,0),
                new DateTime(1990,10,20,11,45,0),
            };
            stringList = new List<string> { "safds", "sfddf", "safsdfsda", "safdsfadsf", "fdewfd", "423fsafd", "gregewg", "dffeger", "sdfasdfdsf", "sdafdsfdasf", "dsafsdfsad" };
            doubleList = new List<double>
            {
                1.23,2.42,3.94, 4.42,4.32, 9.98, 9.99, 10.29, 2.43, 1.02, 6.62, 6.63, 6.75, 8.89, 9.99, 9.10,3.34, 10.10, 2.19
            };
        }

        /// <summary>
        /// Have to use a driver function because NUnit does not support
        /// Generics yet
        /// </summary>
        /// <param name="typeOfList"></param>
        [TestCase("string")]
        [TestCase("int")]
        [TestCase("DateTime")]
        [TestCase("double")]
        public void QuickSortTest(string typeOfList)
        {
            switch (typeOfList) {
                case "string":
                    QuickSortTest_Actual(stringList);
                    break;
                case "int":
                    QuickSortTest_Actual(integerList);
                    break;
                case "DateTime":
                    QuickSortTest_Actual(dateList);
                    break;
                case "double":
                    QuickSortTest_Actual(doubleList);
                    break;
                default:
                    throw new Exception("The typename is not recognized: try 'string' 'int' 'DateTime'");
            }
        }
        public void QuickSortTest_Actual<T>(List<T> list)
            where T:IComparable
        {
            // Arrange
            List<T> objects = list;
            List<T> expected = new List<T>(objects);
            expected.Sort();

            // Assume
            Assume.That(objects.Count, Is.EqualTo(expected.Count));

            // Act
            ZAlgorithms.QuickSort(objects, (x, y) => x.CompareTo(y));

            // Assert
            for (int i=0; i<objects.Count;i++)
            {
                Assert.That(objects[i], Is.EqualTo(expected[i]));
            }
        }
    }
}
