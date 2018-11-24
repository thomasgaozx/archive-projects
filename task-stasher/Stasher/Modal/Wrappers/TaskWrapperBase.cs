using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskStasher.Control.Core
{
    public abstract class TaskWrapperBase
    {

        public ITask Content { get; protected set; }

        public TaskWrapperBase(ITask task)
        {
            Content = task;
        }

    }
}
