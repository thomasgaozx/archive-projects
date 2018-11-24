using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskStasher.Control.Core
{
    public static class FileUtil
    {

        #region Private Fields and Methods

        public static readonly string CurrentLocation 
            = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static string GetDirectoryPath(string directoryName)
        {
            return CurrentLocation + "\\" + directoryName;
        }

        #endregion


    }
}
