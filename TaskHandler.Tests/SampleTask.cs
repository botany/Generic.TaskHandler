using System;
using TaskHandler.BusinessLogic;

namespace TaskHandler.Tests
{
    public class SampleTask : ITask
    {
        public void Execute()
        {
            Console.Out.WriteLine("Sample Task Executed");
        }
    }
}