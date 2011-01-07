using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskHandler.Commons;

namespace TaskHandler.BusinessLogic.Impl
{
    public class SampleTask1 : ITask
    {
        public void Execute()
        {
            LoggingHelper.Log("SAMPLE TASK 1 EXECUTED");
        }
    }
}
