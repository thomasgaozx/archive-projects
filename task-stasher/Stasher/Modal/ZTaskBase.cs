using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskStasher.Control.Core
{


    public abstract class ZTaskBase : ITask
    {

        #region Public Properties

        public string Title { get; set; } // required field
        
        public string Description { get; set; }

        #endregion

        #region Overrides

        public override string ToString()
        {
            return $"Title: {Title}\n"+(String.IsNullOrEmpty(Description)?"":$"Description:\n{Description}\n");
        }

        #endregion

    }
}
