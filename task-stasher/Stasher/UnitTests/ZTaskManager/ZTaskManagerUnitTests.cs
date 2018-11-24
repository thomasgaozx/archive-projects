using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskStasher.Control.Core.UnitTests
{
    [TestFixture]
    public class ZTaskManagerUnitTests
    {
        #region Private Field

        private ZTaskManager manager;

        #endregion

        #region Static Readonly Resources

        private static readonly IList<ITask> tasks = new List<ITask>
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

        private static readonly IList<ZScheduledTask> pTasks = new List<ZScheduledTask>()
        {
            new ZScheduledTask()
            {
                Title=ZTestUtil.GenerateTestTitle(),
                Description=ZTestUtil.GenerateTestDescription(),
                Deadline = DateTime.Today+TimeSpan.FromDays(15),
                ZBuffer = TimeSpan.FromDays(20)
            },
            new ZScheduledTask()
            {
                Title=ZTestUtil.GenerateTestTitle(),
                Description=ZTestUtil.GenerateTestDescription(),
                Deadline = new DateTime(1998,8,8),
                ZBuffer = TimeSpan.FromDays(60)
            },
            new ZScheduledTask()
            {
                Title=ZTestUtil.GenerateTestTitle(),
                Description=ZTestUtil.GenerateTestDescription(),
                Deadline = DateTime.Now+TimeSpan.FromMinutes(50),
                ZBuffer = TimeSpan.FromMinutes(15)
            },
            new ZScheduledTask()
            {
                Title=ZTestUtil.GenerateTestTitle(),
                Description=ZTestUtil.GenerateTestDescription(),
                Deadline = new DateTime(2018,5,18),
                ZBuffer = TimeSpan.FromDays(10)
            }
        };

        #endregion

        [SetUp]
        public void SetUpTest()
        {
            manager = new ZTaskManager();
        }

        [Test]
        public void ZTaskManager_GetAndUpdateCurrentTask()
        {
            // Arrange
            foreach (ITask task in tasks)
            {
                manager.PushTask(task);
            }

            ITask lastTask = tasks[tasks.Count - 1];
            ZScheduledTask mostUrgent = pTasks[0];
            
            // Select the most urgent task
            int priorityListLength = pTasks.Count;
            for (int i = 1; i < priorityListLength; ++i) 
            {
                if (pTasks[i].GetUrgentDate() < mostUrgent.GetUrgentDate())
                {
                    mostUrgent = pTasks[i];
                }
            }
            

            // Assume
            Assume.That(manager.CurrentTask, Is.EqualTo(lastTask));

            // Act I - push priority
            foreach(var task in pTasks) // tasks with priority
            {
                manager.PushTaskWithPriority(task);
            }

            // Assert I - Current Task is still stale
            Assert.That(manager.CurrentTask, Is.EqualTo(lastTask));

            // Act II - update Current Task
            manager.Update();

            // Assert II - Current Task is updated
            Assert.That(manager.CurrentTask, Is.EqualTo(mostUrgent));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void ZTaskManager_ArchiveCurrentTask(int num)
        {
            // Arrange
            foreach (var task in tasks)
                manager.PushTask(task);
            ITask lastTask = tasks[tasks.Count - 1];
            ITask nthLastTask = tasks[tasks.Count - 1 - num];

            // Assume
            Assume.That(manager.CurrentTask, Is.EqualTo(lastTask));
            
            // Act
            for (int i=0; i<num; ++i)
            {
                var t = manager.CurrentTask; // load current task
                manager.ArchiveCurrentTask(Category.Cancelled);
            }

            // Assert
            Assert.That(manager.CurrentTask, Is.EqualTo(nthLastTask));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void ZTaskManager_PutOffCurrentTask(int by)
        {
            // Arrange
            foreach (var task in tasks)
            {
                manager.PushTask(task);
            }
            ITask lastTask = tasks[tasks.Count - 1];

            // Assume
            Assume.That(manager.CurrentTask, Is.EqualTo(lastTask));
            Assume.That(tasks.Count, Is.GreaterThanOrEqualTo(by));

            // Act
            manager.PutOffCurrentTask(by);

            // Assert
            for (int i = 0; i < by; ++i)
            {
                var t = manager.CurrentTask;
                manager.ArchiveCurrentTask(Category.Done);
            }

            Assert.That(manager.CurrentTask, Is.EqualTo(lastTask));
        }

        [Test]
        public void ZTaskManager_PutOffCurrentTaskToBottom()
        {
            // Arrange
            foreach (var task in tasks)
            {
                manager.PushTask(task);
            }
            ITask lastTask = tasks[tasks.Count - 1];

            // Assume
            Assume.That(manager.CurrentTask, Is.EqualTo(lastTask));

            // Act
            manager.PutOffCurrentTaskToBottom();

            // Assert
            for (int i=0; i < tasks.Count - 1; ++i)
            {
                var t = manager.CurrentTask; // load current task
                manager.ArchiveCurrentTask(Category.Obsolete);
            }

            Assert.That(manager.CurrentTask, Is.EqualTo(lastTask));
        }

        [Test]
        public void ZTaskManager_GetAllTasks()
        {
            // Arrange I
            foreach (var task  in tasks)
            {
                manager.PushTask(task);
            }
            List<ITask> copy = new List<ITask>(tasks);

            // Assert I
            Assert.That(manager.GetAllTasks(), Is.EquivalentTo(tasks));

            // Arrange II
            var archivedTask = manager.CurrentTask;
            manager.ArchiveCurrentTask(Category.Obsolete);
            copy.Remove(archivedTask);

            // Assert II
            Assert.That(manager.GetAllTasks(), Is.EquivalentTo(copy));

            // Arrange III
            foreach (var task in pTasks)
            {
                manager.PushTaskWithPriority(task);
            }
            copy.AddRange(pTasks);

            // Assert III
            Assert.That(manager.GetAllTasks(), Is.EquivalentTo(copy));

            // Arrange IV
            archivedTask = manager.CurrentTask;
            manager.ArchiveCurrentTask(Category.Obsolete);
            copy.Remove(archivedTask);

            // Assert IV
            Assert.That(manager.GetAllTasks(), Is.EquivalentTo(copy));

        }

        private const TaskFlag AllTasks = TaskFlag.NonPrioritized | TaskFlag.Prioritized;
        private static readonly IEnumerable<ITask> tasks_all = tasks.Union(pTasks);
        private static readonly object[] selectTasksCases =
        {
            new object[] { TaskFlag.Prioritized, pTasks },
            new object[] { TaskFlag.NonPrioritized, tasks },
            new object[] { AllTasks, tasks_all},
            new object[] { AllTasks|TaskFlag.Scheduled, tasks_all.Where(t=>t is ZScheduledTask) },
            new object[] { AllTasks|TaskFlag.NonScheduled, tasks_all.Where(t=>!(t is ZScheduledTask)) },
            new object[] { AllTasks|TaskFlag.Urgent, tasks_all.Where(t=>
                { var u = t as ZScheduledTask; return u != null && u.IsUrgent(); }) },
            new object[] { AllTasks|TaskFlag.Danger, tasks_all.Where(t=>
                { var d = t as ZScheduledTask; return d != null && d.IsInDanger(); }) },
            new object[] { TaskFlag.NonScheduled|TaskFlag.Urgent, new List<ITask>(1) },
            new object[] { TaskFlag.Scheduled|TaskFlag.Prioritized, pTasks }

        };

        [TestCaseSource(nameof(selectTasksCases))]
        public void ZTaskManager_SelectTasks(TaskFlag flag, IEnumerable<ITask> expectedResult)
        {
            // Arrange
            foreach (var task in tasks)
            {
                manager.PushTask(task);
            }
            foreach (var task in pTasks)
            {
                manager.PushTaskWithPriority(task);
            }

            manager.Update();

            // Assert
            Assert.That(manager.SelectTasks(flag), Is.EquivalentTo(expectedResult));
        }

    }
}
