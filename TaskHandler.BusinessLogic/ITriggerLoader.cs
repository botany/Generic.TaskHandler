using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskHandler.BusinessLogic
{
    public interface ITriggerLoader
    {
        IList<ITrigger> LoadTriggers();
    }
}