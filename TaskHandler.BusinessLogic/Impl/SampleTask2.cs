using TaskHandler.Commons;

namespace TaskHandler.BusinessLogic.Impl
{
    public class SampleTask2 : ITask
    {
        public void Execute()
        {
            LoggingHelper.Log("SAMPLE TASK 2 EXECUTED");
        }
    }
}