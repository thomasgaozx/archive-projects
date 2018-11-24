using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskStasher.ZConsole.ConsoleSyntax
{
    /// <summary>
    /// Manages the keywords of the shell
    /// </summary>
    public static class ZConsoleSyntax
    {

        #region Add Task

        public const string Add = "add";
        public const string ScheduledFlag = "-s";
        public const string PriorityFlag = "-p";

        #endregion

        #region View Tasks

        /// <summary>
        /// Use the same flags as ArchiveCurrent.
        /// Without flags show everything.
        /// </summary>
        public const string ViewArchive = "view-archive";

        public const string ViewAllTasks = "view-all";

        public const string ViewSelect = "view-select";
        public const string PrioritizedTaskFlag = "-p";
        public const string NonPrioritizedTaskFlag = "-np";
        public const string UrgentTaskFlag = "-u";
        public const string DangerTaskFlag = "-d";
        public const string ScheduledTaskFlag = "-s";
        public const string NonScheduledTaskFlag = "-ns";

        public const string ShowDetail = "-detail";

        #endregion

        #region Current Task

        /// <summary>
        /// Viewing current task. Priority, urgency is indicated.
        /// </summary>
        public const string CurTask = "current";

        /// <summary>
        /// With number -> put off n times
        /// With flag -> put off to bottom
        /// Without anything -> randomly put off between 2-6 times.
        /// </summary>
        public const string PutOffCurrent = "put-off-current";

        public const string ToBottomFlag = "-bottom";

        public const string ArchiveCurrent = "archive-current";

        public const string Cat_Done = "-done";

        public const string Cat_Cancelled = "-cancelled";

        public const string Cat_Obsolete = "-obsolete";

        public const string DeprioritizeCurrent = "deprioritize-current";

        public const string PrioritizeCurrent = "prioritize-current";

        public const string UnscheduleCurrent = "unschedule-current";

        public const string ScheduleCurrent = "schedule-current";

        public const string StretchCurrentDeadline = "stretch-deadline";

        public const string ResetCurrentDeadline = "reset-deadline";

        public const string ResetCurrentBuffer = "reset-buffer";

        public const string EditCurrentTitle = "edit-title";

        public const string EditCurrentDescription = "edit-description";

        #endregion

        #region Save or Update

        public const string Save = "save";

        public const string Update = "update";

        #endregion

        #region Keywords

        public const string Quit = "::quit";

        /// <summary>
        /// Quit current procedure, quit current scope and go back to main.
        /// </summary>
        public const string Cancel = "::cancel";

        /// <summary>
        /// Enter a new scope
        /// </summary>
        public const string StartMarker = "::start";

        /// <summary>
        /// Quit scope
        /// </summary>
        public const string EndMarker = "::end";

        #endregion

        #region

        public const string HelpKey = "help";
        public const string KeywordFlag = "-keyword";

        #endregion

    }
}
