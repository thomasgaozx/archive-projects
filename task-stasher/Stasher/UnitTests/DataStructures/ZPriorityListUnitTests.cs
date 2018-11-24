using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskStasher.Control.Core.UnitTests
{
    [TestFixture]
    public class ZPriorityListUnitTests
    {
        private IList<ZScheduledTask> pTasks;

        [SetUp]
        public void SetUpTest()
        {
            pTasks = new List<ZScheduledTask>
            {
                new ZScheduledTask()
                {
                    Title=ZTestUtil.GenerateTestTitle(),
                    Description = ZTestUtil.GenerateTestDescription(),
                    Deadline = new DateTime(2008,10,23),
                    ZBuffer = TimeSpan.FromDays(5)
                },
                new ZScheduledTask()
                {
                    Title=ZTestUtil.GenerateTestTitle(),
                    Description = ZTestUtil.GenerateTestDescription(),
                    Deadline = new DateTime(2018,12,20),
                    ZBuffer = TimeSpan.FromDays(15)
                },

                new ZScheduledTask()
                {
                    Title=ZTestUtil.GenerateTestTitle(),
                    Description = ZTestUtil.GenerateTestDescription(),
                    Deadline = new DateTime(2004,9,20),
                    ZBuffer = TimeSpan.FromDays(25)
                },

                new ZScheduledTask()
                {
                    Title=ZTestUtil.GenerateTestTitle(),
                    Description = ZTestUtil.GenerateTestDescription(),
                    Deadline = new DateTime(2016,10,20),
                    ZBuffer = TimeSpan.FromDays(35)
                },

                new ZScheduledTask()
                {
                    Title=ZTestUtil.GenerateTestTitle(),
                    Description = ZTestUtil.GenerateTestDescription(),
                    Deadline = new DateTime(2019,10,20),
                    ZBuffer = TimeSpan.FromDays(69)
                }
            };
        }

        private static void PushAllTasks(ZPriorityList pList, IList<ZScheduledTask> pTasks)
        {
            foreach (var task in pTasks)
            {
                pList.Push(task);
            }
        }

        [Test]
        public void BuildPriorityList()
        {
            // Arrange
            ZPriorityList pList = new ZPriorityList();

            // Act
            PushAllTasks(pList, pTasks);

            // Assert
            Assert.That(pList.PeekAll(), Is.EquivalentTo(pTasks));
        }

        private static Func<ZPriorityList,ZScheduledTask> SelectViewMethod(string method)
        {
            switch (method)
            {
                case ("pop"):
                    return (pList) => pList.Pop();
                case ("peek"):
                    return (pList) => pList.Peek();
                default:
                    throw new Exception("Cannot recognize! try 'pop' or 'peek'");
            }
        }

        [TestCase("pop")]
        [TestCase("peek")]
        public void PriorityList_GetMostUrgentTask(string method)
        {
            // Arrange
            var getFunc = SelectViewMethod(method);
            ZPriorityList pList = new ZPriorityList();
            ZScheduledTask mostUrgent = pTasks[0];
            for (int i=1; i < pTasks.Count; i++)
            {
                if (pTasks[i].GetUrgentDate() < mostUrgent.GetUrgentDate())
                {
                    mostUrgent = pTasks[i];
                }
            }

            // Assume
            Assume.That(mostUrgent, Is.EqualTo(pTasks[2])); // manually selected

            // Act
            PushAllTasks(pList, pTasks);

            // Assert
            Assert.That(getFunc(pList), Is.EqualTo(mostUrgent));
        }
    }

    
}
