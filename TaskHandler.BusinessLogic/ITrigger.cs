using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskHandler.BusinessLogic
{
    public interface ITrigger
    {
        Guid Id { get; set; }

        DateTime? NextProcessTime { get; set; }

        TimeSpan Interval { get; set;}

        ITask Task { get; set;}
    }
}