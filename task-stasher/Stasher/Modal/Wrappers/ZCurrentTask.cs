using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TaskStasher.Control.Core
{
    public class ZCurrentTask : TaskWrapperBase
    {

        #region Nested Type

        public class DeprioritizationException : Exception
        {
            public DeprioritizationException() { }

            public DeprioritizationException(string message) : base(message) { }

            public DeprioritizationException(string message, Exception innerException) : base(message, innerException) { }

            protected DeprioritizationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        }

        #endregion

        public bool Priority { get; private set; }

        public void Deprioritize()
        {
            if (!Priority)
            {
                throw new DeprioritizationException("Cannot deprioritize a non-prioritized task");
            }
            else
            {
                Priority = false;
            }
        }

        public void Prioritize()
        {
            if (Priority)
            {
                throw new Exception("The task is already prioritized");
            }
            else
            {
                Priority = true;
            }
        }

        public void Schedule(DateTime deadline, TimeSpan buffer)
        {
            if (Content is ZScheduledTask)
            {
                throw new Exception("Task is already scheduled, consider reschedule task.");
            }
            Content = new ZScheduledTask
            {
                Title = Content.Title,
                Description = Content.Description,
                Deadline = deadline,
                ZBuffer = buffer
            };
        }

        public void Unschedule()
        {
            if (!(Content is ZScheduledTask))
            {
                throw new Exception("Task is not scheduled");
            }
            Content = new ZTask
            {
                Title = Content.Title,
                Description = Content.Description
            };
        }

        public ZArchive SendToArchive(Category cat)
        {
            ITask task = Content;
            Content = null;
            return new ZArchive(task, cat);
        }

        public static ZCurrentTask MakeCurrentTaskWithPriority(ZScheduledTask task)
        {
            return new ZCurrentTask(task, true);
        }

        public static ZCurrentTask MakeCurrentTask(ITask task)
        {
            return new ZCurrentTask(task);
        }

        private ZCurrentTask(ZScheduledTask task, bool prioritized) : base(task)
        {
            Priority = prioritized;
        }

        private ZCurrentTask(ITask task) : base(task)
        {
            Priority = false;
        }
    }
}
