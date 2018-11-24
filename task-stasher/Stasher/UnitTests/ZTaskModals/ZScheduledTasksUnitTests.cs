using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskStasher.Control.Core.UnitTests
{
    [TestFixture]
    public class ZTaskModalUnitTests
    {
        protected ZScheduledTask task;

        [OneTimeSetUp]
        public void SetUpFixture()
        {
            task = new ZScheduledTask();
        }

        [Test]
        public void ZScheduledTask_IsInDanger()
        {
            // Arrange
            DateTime deadline = new DateTime(2018,5,6,12,0,0);
            TimeSpan buffer = TimeSpan.FromDays(1);
            task.Deadline = deadline;
            task.ZBuffer = buffer;

            // Assume
            Assume.That(task.GetUrgentDate(), Is.EqualTo(new DateTime(2018, 5, 5, 12, 0, 0)));

            // Assert
            Assert.That(task.IsUrgent, Is.True);
            Assert.That(task.IsInDanger(), Is.True);

        }
    }
}
