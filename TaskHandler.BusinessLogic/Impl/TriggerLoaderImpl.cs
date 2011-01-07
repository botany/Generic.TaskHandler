using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using TaskHandler.Commons;

namespace TaskHandler.BusinessLogic.Impl
{
    public class TriggerLoaderImpl : ITriggerLoader
    {
        public IList<ITrigger> LoadTriggers()
        {
            ITrigger[] all = IoC.ResolveAll<ITrigger>();

            var list = new List<ITrigger>(all);

            return list;
        }
    }
}