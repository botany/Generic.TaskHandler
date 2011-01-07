using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TaskHandler.Commons
{
    public class IocException : BaseException
    {
        public IocException()
        {
        }

        public IocException(string message) : base(message)
        {
        }

        public IocException(Exception innerException, string message) : base(innerException, message)
        {
        }

        public IocException(string message, params object[] parameters) : base(message, parameters)
        {
        }

        public IocException(Exception exception, string message, params object[] objects) : base(exception, message, objects)
        {
        }

        protected IocException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}