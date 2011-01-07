using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TaskHandler.Commons
{
    public class BaseException : Exception
    {
        public BaseException()
        {
        }

        public BaseException(string message) : base(message)
        {
            LoggingHelper.Log(message);
        }

        public BaseException(Exception innerException, string message) : base(message, innerException)
        {
            LoggingHelper.Log(innerException, message);
        }

        public BaseException(string message, params object[] parameters)
        {
            LoggingHelper.Log(message, parameters);
        }

        public BaseException(Exception exception, string message, params object[] objects)
        {
            LoggingHelper.Log(exception, message, objects);
        }

        protected BaseException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}