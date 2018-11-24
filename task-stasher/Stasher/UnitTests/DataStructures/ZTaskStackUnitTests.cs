using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskStasher.Control.Core.UnitTests
{
    [TestFixture]
    public class ZTaskStackUnitTests
    {
        private IList<ITask> tasks;

        [SetUp]
        public void SetUpTest()
        {
            tasks = new List<ITask>
            {
                new ZTask()
                {
                    Title=ZTestUtil.GenerateTestTitle(),
                    Description=ZTestUtil.GenerateTestDescription()
                },
                new ZTask()
                {
                    Title=ZTestUtil.GenerateTestTitle(),
                    Description=ZTestUtil.GenerateTestDescription()
                },
                new ZScheduledTask()
                {
                    Title=ZTestUtil.GenerateTestTitle(),
                    Description=ZTestUtil.GenerateTestDescription(),
                    Deadline = new DateTime(2020,10,8),
                    ZBuffer = TimeSpan.FromDays(6)
                },
                new ZTask()
                {
                    Title=ZTestUtil.GenerateTestTitle(),
                    Description=ZTestUtil.GenerateTestDescription()
                },
                new ZScheduledTask()
                {
                    Title=ZTestUtil.GenerateTestTitle(),
                    Description=ZTestUtil.GenerateTestDescription(),
                    Deadline = new DateTime(2007,8,9),
                    ZBuffer = TimeSpan.FromDays(180)
                },
                new ZScheduledTask()
                {
                    Title=ZTestUtil.GenerateTestTitle(),
                    Description=ZTestUtil.GenerateTestDescription(),
                    Deadline = new DateTime(1998,8,8),
                    ZBuffer = TimeSpan.FromDays(60)
                },
                new ZTask()
                {
                    Title=ZTestUtil.GenerateTestTitle(),
                    Description=ZTestUtil.GenerateTestDescription()
                },
                new ZTask()
                {
                    Title=ZTestUtil.GenerateTestTitle(),
                    Description=ZTestUtil.GenerateTestDescription()
                }
            };
        }

        [Test]
        public void ZTaskStack_PushAndPop()
        {
            // Arrange
            int[] pushSequence = { 1, 7, 0, 2, 4, 3, 5, 6 };
            int[] popSequence = pushSequence.Reverse().ToArray();
            ZTaskStack testStack = new ZTaskStack();

            // Assume
            Assume.That(tasks.Count, Is.GreaterThanOrEqualTo(8));
            Assume.That(testStack.Empty(), Is.True);

            // Act
            foreach (int i in pushSequence)
            {
                testStack.Push(tasks[i]);
            }

            // Assert
            foreach (int i in popSequence)
            {
                Assert.That(testStack.Pop(), Is.EqualTo(tasks[i]));
            }
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void ZTaskStack_DelayTaskBy(int i)
        {
            // Arrange
            ZTaskStack testStack = new ZTaskStack();
            testStack.PushAll(tasks);

            // Assume
            Assume.That(testStack.PeekAll(), Is.EquivalentTo(tasks));
            Assume.That(i, Is.LessThan(tasks.Count));

            // Act
            var curtask = testStack.Peek();
            testStack.DelayCurrentTaskBy(i);

            // Assert
            for (int j=0; j<i; ++j)
            {
                testStack.Pop();
            }
            Assert.That(testStack.Peek(), Is.EqualTo(curtask));
        }

        [Test]
        public void ZTaskStack_DelayTaskToBottom()
        {
            // Arrange
            ZTaskStack testStack = new ZTaskStack();
            testStack.PushAll(tasks);

            // Assume
            Assume.That(testStack.PeekAll(), Is.EquivalentTo(tasks));

            // Act
            var curTask = testStack.Peek();
            testStack.DelayCurrentTaskToBottom();

            // Assert
            int lim = testStack.Count() - 1;
            for (int i=0; i<lim; ++i)
            {
                testStack.Pop();
            }
            Assert.That(testStack.Peek(), Is.EqualTo(curTask));
            testStack.Pop();
            Assert.That(testStack.Empty(), Is.True);
        }

        [Test]
        public void ZTaskStack_PullUrgentTask()
        {
            // Arrange
            ZTaskStack testStack = new ZTaskStack();
            testStack.PushAll(tasks);
            IEnumerable<ITask> urgents = tasks.Where(t =>
            {
                ZScheduledTask st = t as ZScheduledTask;
                if (st!=null && st.IsUrgent())
                {
                    return true;
                }
                return false;
            });
            int numOfUrgents = urgents.Count();

            // Assume
            Assume.That(testStack.PeekAll(), Is.EquivalentTo(tasks));

            // Act
            testStack.PullUrgentTasks();

            // Assert
            var actual = testStack.PeekTasks(numOfUrgents);
            Assert.That(actual, Is.EquivalentTo(urgents));
        }

        [Test]
        public void ZTaskStack_PullDangerTasks()
        {
            // Arrange
            ZTaskStack testStack = new ZTaskStack();
            testStack.PushAll(tasks);
            IEnumerable<ITask> dangers = tasks.Where(t =>
            {
                ZScheduledTask st = t as ZScheduledTask;
                if (st != null && st.IsInDanger())
                {
                    return true;
                }
                return false;
            });
            int numOfDangers = dangers.Count();

            // Assume
            Assume.That(testStack.PeekAll(), Is.EquivalentTo(tasks));

            // Act
            testStack.PullDangerTasks();

            // Assert
            var actual = testStack.PeekTasks(numOfDangers);
            Assert.That(actual, Is.EquivalentTo(dangers));
        }
    }
}
