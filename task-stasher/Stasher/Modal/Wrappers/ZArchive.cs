using System;
using TaskStasher.Control.Core.Util;

namespace TaskStasher.Control.Core
{
    public enum Category { Done, Cancelled, Obsolete }

    public class ZArchive : TaskWrapperBase
    {

        #region Public Properties

        public Category Status { get; private set; }

        public DateTime ArchiveDate { get; private set; }

        #endregion

        #region Methods

        public string Extract()
        {
            return $"{Content.Title} : {Status} : {ArchiveDate.ToFormattedString()}";
        }

        public string ExtractDetail()
        {
            string taskString = $"Title: {Content.Title}\n";
            if (!String.IsNullOrWhiteSpace(Content.Description))
            {
                taskString += $"Description: {Content.Description}\n";
            }
            if (Content is ZScheduledTask)
            {
                ZScheduledTask task = Content as ZScheduledTask;
                taskString += $"Deadline: {task.Deadline.ToFormattedString()}\nDeadline Buffer: {task.ZBuffer.ToString()}\n";
            }
            taskString += $"Status: {Enum.GetName(typeof(Category), Status)}\n" +
                $"Archive Date: {ArchiveDate.ToFormattedString()}\n\n";
            return taskString;
        }

        #endregion

        #region Life Cycle

        public ZArchive(ITask task, Category status) : base(task)
        {
            Status = status;
            ArchiveDate = DateTime.Now;
        }

        public ZArchive(ITask task, Category status, DateTime archiveDate) : base(task)
        {
            Status = status;
            ArchiveDate = archiveDate;
        }

        #endregion

    }
}
