using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskHandler.BusinessLogic.Impl
{
    public class GenericTrigger : ITrigger
    {
        public Guid Id
        {
            get; set;
        }

        public DateTime? NextProcessTime
        {
            get; set;
        }

        public TimeSpan Interval
        {
            get; set;
        }

        public ITask Task
        {
            get; set;
        }
    }
}