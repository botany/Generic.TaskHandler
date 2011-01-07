using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using CustomThreadPoolImpl;
using TaskHandler.Commons;

namespace TaskHandler.BusinessLogic.Impl
{
    public class MainTaskProcessor
    {
        private static readonly object Sync = new object();
        // needed to stop processing
        private static bool _halted;

        // jon skeet custom thread pool
        private static readonly CustomThreadPool CustomThreadPool = new CustomThreadPool("myThreadPool");

        // collection of recurrent tasks
        private static readonly Dictionary<Guid, ITrigger> _workerToTaskList = new Dictionary<Guid, ITrigger>();

        // maximum amount of threads
        private static readonly int MaxThreads = 16;

        // time to wait
        private static readonly TimeSpan IdleTime = TimeSpan.FromSeconds(5);

        // thread used to host main loop
        private static Thread MainLoopThread;


        static MainTaskProcessor()
        {
            // define pool restrictions
            CustomThreadPool.SetMinMaxThreads(1, MaxThreads);

            // initialize threads
            CustomThreadPool.StartMinThreads();
        }

        // interface, used to load all tasks from config or db
        private ITriggerLoader _triggerLoader;

        public MainTaskProcessor(ITriggerLoader triggerLoader)
        {
            _triggerLoader = triggerLoader;
        }

        // starts the execution
        public void Start()
        {
            var triggers = _triggerLoader.LoadTriggers();

            // exception handling
            CustomThreadPool.WorkerException += ((CustomThreadPool pool, ThreadPoolWorkItem item, Exception exception, ref bool handled) =>
                                                     {
                                                         var task = item.Parameters[0];
                                                         LoggingHelper.Log(exception, "Error while processing Task {0}", task.GetType().FullName);
                                                     });


            foreach (var trigger in triggers)
            {
                // generate uniqueId for the task item
                Guid workItemId = Guid.NewGuid();

                if (trigger.NextProcessTime == null)
                {
                    trigger.NextProcessTime = DateTime.UtcNow;
                }

                trigger.Id = workItemId;

                // add workitem to the dict, main loop will handle it
                _workerToTaskList.Add(workItemId, trigger);
            }

            // start main loop in a separate thread
            var ts = new ThreadStart(Run);

            MainLoopThread = new Thread(ts);

            MainLoopThread.Start();
        }

        /// <summary>
        /// Main processing loop
        /// </summary>
        public void Run()
        {
            while (!Halted)
            {
                // check if we have free threads
                if (CustomThreadPool.WorkingThreads < MaxThreads)
                {
                    var nextForProcess = GetNextForProcess(DateTime.UtcNow.Add(IdleTime));

                    if (nextForProcess == null)
                    {
                        lock (Sync)
                        {
                            try
                            {
                                // wait some time to see if something is ready for processing
                                Monitor.Wait(Sync, 2000);
                            }
                            catch (ThreadInterruptedException)
                            {
                            }
                        }
                    }
                    else
                    {
                        var triggerTime = nextForProcess.NextProcessTime.Value;

                        TimeSpan timeUntilTrigger = triggerTime - DateTime.UtcNow;

                        while (timeUntilTrigger > TimeSpan.Zero)
                        {
                            lock (Sync)
                            {
                                try
                                {
                                    // we must recompute if lock took some time
                                    timeUntilTrigger = triggerTime - DateTime.UtcNow;

                                    if (timeUntilTrigger.TotalMilliseconds >= 1)
                                    {
                                        Monitor.Wait(Sync, timeUntilTrigger);
                                    }
                                }
                                catch (ThreadInterruptedException)
                                {
                                }
                            }

                            timeUntilTrigger = triggerTime - DateTime.UtcNow;
                        }

                        bool goAhead;

                        lock (Sync)
                        {
                            goAhead = !Halted;
                        }

                        if (goAhead)
                        {
                            // some thread will pickup this item and process it
                            CustomThreadPool.AddWorkItem(new ThreadPoolWorkItem(nextForProcess.Id, true, true, 1,
                                                                                new Action<ITrigger>(item =>
                                                                                                         {
                                                                                                             try
                                                                                                             {
                                                                                                                 item.Task.Execute();
                                                                                                             }
                                                                                                             catch (Exception exp)
                                                                                                             {
                                                                                                                 LoggingHelper.Log(exp, "Error on task processing");
                                                                                                             }
                                                                                                         }), nextForProcess));

                            nextForProcess.NextProcessTime = DateTime.UtcNow + nextForProcess.Interval;
                        }
                    }
                }
                else
                {
                    // if we don't have threads, wait for some time to get a free one
                    // normally, should not occur
                    lock (Sync)
                    {
                        try
                        {
                            Monitor.Wait(Sync, 2000);
                        }
                        catch (ThreadInterruptedException)
                        {
                        }
                    }
                }
            }
        }

        // will pick-up first ready-to-process item
        private ITrigger GetNextForProcess(DateTime date)
        {
            var key = _workerToTaskList.Values.FirstOrDefault(taskWorkItem => taskWorkItem.NextProcessTime < date);

            if(key != null)
            {
                return _workerToTaskList[key.Id];
            }
            else
            {
                return null;
            }
        }

        public void Stop()
        {
            // Main loop should exit
            Halt();

            // give tasks some time to finish
            Thread.Sleep(1000);

            // cancel everything which is in the queue
            CustomThreadPool.CancelAllWorkItems();

            // give main loop 5 seconds to finish work
            MainLoopThread.Join(5000);

            // finish it for sure
            MainLoopThread.Abort();
        }

        /// <summary>
        /// Signals the main processing loop to pause at the next possible point.
        /// </summary>
        internal void Halt()
        {
            lock (Sync)
            {
                _halted = true;

                Monitor.PulseAll(Sync);
            }
        }

        private bool Halted
        {
            get
            {
                lock (Sync)
                {
                    return _halted;
                }
            }
            set
            {
                lock (Sync)
                {
                    _halted = value;
                }
            }
        }
    }
}