using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskStasher.Control.Core.UnitTests
{
    [TestFixture]
    public class ZCurrentTaskUnitTests
    {
        private static ZTask task;
        private static ZScheduledTask scheduledTask;
        private ZCurrentTask currentTask;

        [OneTimeSetUp]
        public void SetUpFixture()
        {
            task = new ZTask()
            {
                Title = ZTestUtil.TestTitle,
                Description = ZTestUtil.TestDescription
            };

            scheduledTask = new ZScheduledTask()
            {
                Title = ZTestUtil.TestTitle,
                Description = ZTestUtil.TestDescription,
                Deadline = DateTime.Now,
                ZBuffer = TimeSpan.FromMinutes(50)
            };
        }

        [Test]
        public void ZCurrentTask_DeprioritizePrioritizedScheduledTask()
        {
            // Arrange
            currentTask = ZCurrentTask.MakeCurrentTaskWithPriority(scheduledTask);

            // Assume
            Assume.That(currentTask.Priority, Is.True);

            // Act
            currentTask.Deprioritize();

            // Assert
            Assert.That(currentTask.Priority, Is.False);            
        }

        private static readonly object[] currentTaskSources = new object[]
        {
            new object[] {ZCurrentTask.MakeCurrentTask(task),"regular task"},
            new object[] {ZCurrentTask.MakeCurrentTask(scheduledTask),"scheduled task"}
        };

        [TestCaseSource(nameof(currentTaskSources))]
        public void ZCurrentTask_Deprioritize_Failures(ZCurrentTask curTask, string description)
        {
            // Arrange
            currentTask = curTask;
            
            Func<ZCurrentTask, bool> TryDeprioritize = c =>
            {
                try
                {
                    c.Deprioritize();
                }
                catch (ZCurrentTask.DeprioritizationException)
                {
                    return false;
                }
                return true;
            };

            // Assume
            Assume.That(currentTask.Priority, Is.False);

            // Act
            bool isSuccessful = TryDeprioritize(currentTask);

            // Assert
            Assert.That(currentTask.Priority, Is.False);
        }

    }
}
