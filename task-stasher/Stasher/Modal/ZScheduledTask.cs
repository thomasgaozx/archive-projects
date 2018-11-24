using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskStasher.Control.Core
{
    public class ZScheduledTask : ZTaskBase
    {
        public DateTime Deadline { get; set; }

        /// <summary>
        /// Buffer is the timespan before the deadline within which the user
        /// should be concerned about this unfinished task.
        /// </summary>
        public TimeSpan ZBuffer { get; set; }

        /// <summary>
        /// Gets the date time that the task will become urgent
        /// </summary>
        /// <returns></returns>
        public DateTime GetUrgentDate()
        {
            return Deadline - ZBuffer;
        }

        /// <summary>
        /// Checks if the deadline is passed
        /// </summary>
        /// <returns></returns>
        public bool IsInDanger()
        {
            return Deadline - DateTime.Now < TimeSpan.Zero;
        }

        /// <summary>
        /// Checks if the task is within the buffer time span
        /// </summary>
        /// <returns></returns>
        public bool IsUrgent()
        {
            return Deadline - DateTime.Now < ZBuffer;
        }

        public void PutOff(TimeSpan time)
        {
            Deadline += time;
        }

        public void ResetDeadline(DateTime newDeadline)
        {
            Deadline = newDeadline;
        }


        public void ResetBuffer(TimeSpan buffer)
        {
            this.ZBuffer = buffer;
        }

        #region Overrides

        public override string ToString()
        {

            return base.ToString()+$"Deadline: {Deadline.ToString("o")}\nBuffer: {ZBuffer.ToString()}\n";
        }

        #endregion
    }
}
