using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using TaskHandler.BusinessLogic;
using TaskHandler.BusinessLogic.Impl;
using TaskHandler.Commons;

namespace TaskHandler
{
    public partial class TaskHandler : ServiceBase
    {
        public TaskHandler()
        {
            InitializeComponent();
        }

        private MainTaskProcessor _mainTaskProcessor;

        /// <summary>
        /// Set things in motion so your service can do its work.
        /// </summary>
        protected override void OnStart(string[] args)
        {
            Debugger.Break();

            LoggingHelper.LogNoError("Starting service");

            System.IO.Directory.SetCurrentDirectory(System.AppDomain.CurrentDomain.BaseDirectory);

            try
            {
                _mainTaskProcessor = new MainTaskProcessor(IoC.Resolve<ITriggerLoader>());

                _mainTaskProcessor.Start();
            }
            catch (Exception e)
            {
                throw new BaseException(e, "Failed to start TaskHandler");
            }

            LoggingHelper.LogNoError("Service started");
        }

        /// <summary>
        /// Stop this service.
        /// </summary>
        protected override void OnStop()
        {
            LoggingHelper.LogNoError("Stopping service");

            try
            {
                _mainTaskProcessor.Stop();

                LoggingHelper.LogNoError("Service stopped");
            }
            catch (Exception e)
            {
                LoggingHelper.Log(e, "Failure on stop TaskHandlerService");
            }
            finally
            {
                //Environment.Exit(1);
            }
        }
    }
}
