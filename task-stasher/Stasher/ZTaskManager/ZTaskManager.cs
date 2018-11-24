using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskStasher.Control.Core
{
    [Flags]
    public enum TaskFlag
    {
        None=0,
        Prioritized=1,
        NonPrioritized=1<<1,
        Urgent=1<<2,
        Danger=1<<3,
        Scheduled=1<<4,
        NonScheduled=1<<5
    }

    public class ZTaskManager
    {

        #region Private Fields

        private static readonly Random random = new Random();
        private ZCurrentTask currentTask;

        private ZPriorityList priorityList;
        private ZTaskStack taskStack;
        private List<ZArchive> archiveList = new List<ZArchive>();

        private const string mainDir = @"\main";
        private const string copyDir = @"\copy";

        private const string stackFileName = @"\stack.json";
        private const string priorityFileName = @"\priority.json";
        private const string archiveFileName = @"\archive.json";

        public const string NoCurrentTaskWarning = "Warning: no current task has been loaded. A current task is auto-loaded to perform this action";
        public const string YouHaveNotLoadedACurrentTask = "You have not loaded the current task yet!";
        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Fetches the current task
        /// </summary>
        private void LoadTask()
        {
            bool taskStackIsEmpty = taskStack.Empty();
            if (priorityList.Any())
            {
                if (priorityList.Peek().IsUrgent()||taskStackIsEmpty)
                {
                    currentTask = ZCurrentTask.MakeCurrentTaskWithPriority(priorityList.Pop());
                }
            }
            else if (taskStackIsEmpty)
            {
                throw new Exception("No tasks are pushed");
            }
            else
            {
                currentTask = ZCurrentTask.MakeCurrentTask(taskStack.Pop());
            }
        }

        private void CheckPriority()
        {
            if (currentTask.Priority)
            {
                throw new Exception("Cannot put off tasks with priority. Consider remove task priority, put off task, or reset the deadline buffer for the task.");
            }
        }

        private void RePushCurrentTask()
        {
            if (currentTask != null)
            {
                if (CurrentTaskIsPrioritized)
                {
                    priorityList.Push(CurrentTask as ZScheduledTask);
                }
                else
                {
                    taskStack.Push(CurrentTask);
                }
                currentTask = null;
            }
        }

        #endregion

        #region Public Properties

        public ITask CurrentTask
        {
            get
            {
                if (currentTask==null)
                {
                    LoadTask();
                }
                return currentTask.Content;
            }
        }

        public bool CurrentTaskIsPrioritized
        {
            get
            {
                if (currentTask==null)
                {
                    LoadTask();
                }
                return currentTask.Priority;
            }
        }

        public bool CurrentTaskNotLoaded => currentTask == null;
        
        #endregion

        #region Methods

        public void PushTask(ITask task)
        {
            taskStack.Push(task);
        }

        public void PushTaskWithPriority(ZScheduledTask task)
        {
            priorityList.Push(task);
        }

        [Obsolete]
        public void PushTask(ZScheduledTask task, bool priority)
        {
            if (priority)
            {
                priorityList.Push(task);
            }
            else
            {
                taskStack.Push(task);
            }
        }

        /// <summary>
        /// Should only call when in current task scope. Returns teh title of the task being archived
        /// </summary>
        /// <param name="cat"></param>
        /// <returns>the title of the task being archived</returns>
        public string ArchiveCurrentTask(Category cat)
        {
            if (currentTask == null)
                throw new Exception(YouHaveNotLoadedACurrentTask);

            string title = CurrentTask.Title;
            archiveList.Add(currentTask.SendToArchive(cat));
            currentTask = null;
            return title;
        }

        /// <summary>
        /// Refresh the stack and the list and re-get the CurrentTask
        /// </summary>
        public void Update()
        {
            // Push the current story back and reshuffle
            RePushCurrentTask();

            taskStack.PullUrgentTasks();
            taskStack.PullDangerTasks();

            LoadTask();
        }
        
        /// <summary>
        /// Put off the current task by a number of positions.
        /// </summary>
        /// <param name="by">number of positions to put off</param>
        /// <returns>the name of the task being put off</returns>
        public string PutOffCurrentTask(int by)
        {
            // Load task if not already (with warning)
            if (currentTask==null)
            {
                LoadTask();
                Console.WriteLine("Warning: current task is not loaded, a task is auto-pushed as current task.");
            }

            // Make sure that the task is not prioritized, (Exception thrown otherwise)
            CheckPriority();

            // Push the current task back into the stack and delay it
            string title = CurrentTask.Title; //return the title later on
            taskStack.Push(CurrentTask);
            currentTask = null;
            taskStack.DelayCurrentTaskBy(by);

            return title;
        }
        /// <summary>
        /// Returns title of the task being delayed.
        /// </summary>
        /// <returns>title of the delayed task</returns>
        public string PutOffCurrentTaskToBottom()
        {
            // Load Task and Check Priority
            if (currentTask == null)
            {
                LoadTask();
                Console.WriteLine("Warning: current task is not loaded, a task is auto-pushed as current task.");
            }
            CheckPriority();

            // Reserve title for return
            string title = CurrentTask.Title;

            // Push task back to stack and delay
            taskStack.Push(CurrentTask);
            currentTask = null;
            taskStack.DelayCurrentTaskToBottom();

            return title;
        }

        public List<ITask> GetAllTasks()
        {
            var tasks = new List<ITask>
            {
                CurrentTask
            };

            tasks.AddRange(taskStack.PeekAll());
            tasks.Reverse();

            tasks.AddRange(priorityList.PeekAll());
            return tasks;
        }

        /// <summary>
        /// <see cref="TaskFlag.Prioritized"/> or <see cref="TaskFlag.NonPrioritized"/> is 
        /// a must. Based on which the user may add additional constraints such as
        /// whether the task is scheduled or not, or if the task is urgent or not.
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        public List<ITask> SelectTasks(TaskFlag flag)
        {
            var from = new List<ITask>();

            if ((flag&TaskFlag.Prioritized)==TaskFlag.Prioritized)
            {
                if (CurrentTaskIsPrioritized)
                {
                    from.Add(CurrentTask);
                }
                from.AddRange(priorityList.PeekAll());
            }
            if ((flag&TaskFlag.NonPrioritized)==TaskFlag.NonPrioritized)
            {
                if (!CurrentTaskIsPrioritized)
                {
                    from.Add(CurrentTask);
                }
                from.AddRange(taskStack.PeekAll());
            }

            List<Func<ITask,bool>> selectFunc = new List<Func<ITask,bool>>();
            if ((flag & TaskFlag.Scheduled)==TaskFlag.Scheduled)
            {
                selectFunc.Add(task => task is ZScheduledTask);
            }
            if ((flag & TaskFlag.NonScheduled) == TaskFlag.NonScheduled)
            {
                selectFunc.Add(task => !(task is ZScheduledTask));
            }
            
            if ((flag & TaskFlag.Urgent) == TaskFlag.Urgent)
            {
                selectFunc.Add(task =>
                {
                    var scheduled = task as ZScheduledTask;
                    return scheduled != null && scheduled.IsUrgent();
                });
            }
            if ((flag & TaskFlag.Danger) == TaskFlag.Danger)
            {
                selectFunc.Add(task =>
                {
                    var scheduled = task as ZScheduledTask;
                    return scheduled != null && scheduled.IsInDanger();
                });
            }
            
            if (selectFunc.Any())
            {
                return from.Where(task =>
                {
                    foreach (Func<ITask, bool> func in selectFunc)
                    {
                        if (!func(task))
                        {
                            return false;
                        }
                    }
                    return true;
                }).ToList();
            }
            else
            {
                return from;
            }
        }

        public void DeprioritizeCurrentTask()
        {
            if (currentTask == null)
                throw new Exception(YouHaveNotLoadedACurrentTask);
            currentTask.Deprioritize();
        }

        public void PrioritizeCurrentTask()
        {
            if (currentTask == null)
                throw new Exception(YouHaveNotLoadedACurrentTask);
            if (CurrentTask is ZScheduledTask)
                currentTask.Prioritize();
            else
                throw new Exception("Current task is not scheduled and therefore cannot be prioritized, consider schedule the current task first");
        }

        public void ScheduleCurrentTask(DateTime deadline, TimeSpan buffer)
        {
            if (currentTask==null)
                throw new Exception(YouHaveNotLoadedACurrentTask);
            currentTask.Schedule(deadline,buffer);
        }

        public void UnscheduleCurrentTask()
        {
            if (currentTask == null)
            {
                throw new Exception(YouHaveNotLoadedACurrentTask);
            }
            if (CurrentTask is ZScheduledTask && !CurrentTaskIsPrioritized)
            {
                currentTask.Unschedule();
            }
            else
            {
                throw new Exception("The current task is either not scheduled to begin with, or is prioritized.");
            }
        }

        public IEnumerable<ZArchive> GetArchives()
        {
            return archiveList;
        }

        public IEnumerable<ZArchive> GetArchives(Category cat)
        {
            return archiveList.Where(a => a.Status == cat);
        }


        public void Save()
        {
            // Check to make sure that manager is not empty
            if (!(archiveList.Any()|| GetAllTasks().Any()))
            {
                throw new Exception("Cannot save when there is no task.");
            }

            // Set Up Directories
            DirectoryInfo main = new DirectoryInfo(FileUtil.CurrentLocation+mainDir);
            DirectoryInfo copy = new DirectoryInfo(FileUtil.CurrentLocation+copyDir);

            // Initialize Directory
            if (!main.Exists)
            {
                main.Create();
            }
            if (!copy.Exists)
            {
                copy.Create();
            }

            // Clear Copy Directory if not empty
            var files_copy = copy.EnumerateFiles();
            if (files_copy.Any())
            {
                foreach (var file in files_copy)
                {
                    file.Delete();
                }
            }

            // Copy entries from main to copy, then delete entries in main
            var files_main = main.EnumerateFiles();
            if (files_main.Any())
            {
                foreach (var file in files_main)
                {
                    file.CopyTo(copy.FullName + "\\" + file.Name);
                }

                foreach (var file in files_main)
                {
                    file.Delete();
                }
            }

            // Save files to main
            RePushCurrentTask();
            var tasks_np = taskStack.PeekAll();
            if (tasks_np.Any())
            {
                FileInfo stack_saveto = new FileInfo(main.FullName + stackFileName);

                using (StreamWriter writer = new StreamWriter(stack_saveto.FullName))
                {
                    foreach (var task_np in tasks_np)
                    {
                        writer.WriteLine(JsonConvert.SerializeObject(task_np));
                    }
                }
                
            }

            var tasks_p = priorityList.PeekAll();
            if (tasks_p.Any())
            {
                FileInfo stack_saveto = new FileInfo(main.FullName + priorityFileName);

                using (StreamWriter writer = new StreamWriter(stack_saveto.FullName))
                {
                    foreach (var task_p in tasks_p)
                    {
                        writer.WriteLine(JsonConvert.SerializeObject(task_p));
                    }
                }
            }

            if (archiveList.Any())
            {
                FileInfo archive_saveto = new FileInfo(main.FullName + archiveFileName);

                using (StreamWriter writer = new StreamWriter(archive_saveto.FullName))
                {
                    foreach(var archive in archiveList)
                    {
                        writer.WriteLine($"{archive.ArchiveDate.ToString("o")}:::{(int)archive.Status}:::{JsonConvert.SerializeObject(archive.Content)}");
                    }
                }
            }

        }

        /// <summary>
        /// Should only be called once in Init();
        /// </summary>
        public void LoadFromDisk()
        {
            // Make sure that there is currently no tasks being pushed.
            if (currentTask!=null || !taskStack.Empty() || priorityList.Any())
            {
                throw new Exception("Cannot load from disk when tasks are not cleared!");
            }

            DirectoryInfo main = new DirectoryInfo(FileUtil.CurrentLocation + mainDir);
            FileInfo stack_file = new FileInfo(main.FullName + stackFileName);
            FileInfo priority_file = new FileInfo(main.FullName + priorityFileName);
            FileInfo archive_file = new FileInfo(main.FullName + archiveFileName);

            if (main.Exists)
            {
                if (stack_file.Exists)
                {
                    string[] taskJsons = File.ReadAllLines(stack_file.FullName);
                    int total = taskJsons.Length;
                    ITask[] tasks = new ITask[total];
                    for (int i=0; i < total; ++i)
                    {
                        tasks[i]=JsonConvert.DeserializeObject<ZTask>(taskJsons[total-i-1]);
                    }
                    foreach (ITask task in tasks)
                    {
                        taskStack.Push(task);
                    }
                }
                if (priority_file.Exists)
                {
                    string[] ptaskJsons = File.ReadAllLines(priority_file.FullName);
                    foreach (var ptaskJson in ptaskJsons)
                    {
                        var t = JsonConvert.DeserializeObject<ZScheduledTask>(ptaskJson);
                        if (t == null)
                        {
                            throw new BadFileParsingException("Json parsing failure!");
                        }
                        priorityList.Push(t);
                    }
                }
                if (archive_file.Exists)
                {
                    string[] ataskInfos = File.ReadAllLines(archive_file.FullName);
                    foreach (string ataskInfo in ataskInfos)
                    {
                        if (!String.IsNullOrWhiteSpace(ataskInfo))
                        {
                            string[] content = ataskInfo.Split(new[] { ":::" }, StringSplitOptions.None);
                            if (content.Length!=3)
                            {
                                throw new BadFileParsingException("archive task parsing error!");
                            }

                            DateTime date = DateTime.Parse(content[0]);
                            Category cat = (Category)int.Parse(content[1]);
                            var t = JsonConvert.DeserializeObject<ZTask>(content[2]);

                            if (t==null)
                            {
                                throw new BadFileParsingException("Parsing archive json failure!");
                            }

                            archiveList.Add(new ZArchive(t, cat, date));
                        }

                    }
                }
            }



        }

        #endregion

        #region Life Cycle

        public ZTaskManager()
        {
            priorityList = new ZPriorityList();
            taskStack = new ZTaskStack();
        }

        #endregion

    }
}
