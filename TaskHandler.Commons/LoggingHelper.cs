using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace TaskHandler.Commons
{
    public class LoggingHelper
    {
        public static void Log(string message)
        {
            Log(TraceEventType.Error, message, null);
        }

        public static void LogNoError(string message)
        {
            LogEntry logEntry = new LogEntry
            {
                Priority = 10,
                Title = "Information from TaskHandler Component!",
                Severity = TraceEventType.Information,
                Message = message
            };

            Logger.Write(logEntry);
        }

        public static void LogNoError(string message, params object[] parameters)
        {
            LogEntry logEntry = new LogEntry
            {
                Priority = 10,
                Title = "Information from TaskHandler Component!",
                Severity = TraceEventType.Information,
                Message = parameters == null ? message : string.Format(message, parameters)
            };

            Logger.Write(logEntry);
        }

        public static void Log(Exception exp, string message, params object[] parameters)
        {
            LogEntry logEntry = new LogEntry
                                    {
                                        Priority = 10,
                                        Title = parameters == null ? message : string.Format(message, parameters),
                                        Severity = TraceEventType.Error,
                                        Message = string.Format("\r\n exception : {0} \r\n {1}", exp.Message, exp.StackTrace)
                                    };
            Logger.Write(logEntry);
        }

        public static void Log(TraceEventType type, string message, params object[] parameters)
        {
            LogEntry logEntry = new LogEntry
                                    {
                                        Priority = 10,
                                        Severity = type,
                                        Title = "Error in TaskHandler component!",
                                        Message = (parameters == null || parameters.Length == 0) ? message : string.Format(message, parameters),
                                    };

            Logger.Write(logEntry);
        }

        public static void Log(string message, params object[] parameters)
        {
            LogEntry logEntry = new LogEntry
                                    {
                                        Priority = 10,
                                        Title = "Error in TaskHandler Component!",
                                        Severity = TraceEventType.Error,
                                        Message = parameters == null ? message : string.Format(message, parameters)
                                    };

            Logger.Write(logEntry);
        }

        public static void Log(Exception exp, string message)
        {
            Log(exp, message, null);
        }
    }
}