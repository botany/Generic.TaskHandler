using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TaskHandler.Commons
{
    public class TypeResolveException : IocException
    {
        public TypeResolveException()
        {
        }

        public TypeResolveException(string message) : base(message)
        {
        }

        public TypeResolveException(Exception innerException, string message) : base(innerException, message)
        {
        }

        public TypeResolveException(string message, params object[] parameters) : base(message, parameters)
        {
        }

        public TypeResolveException(Exception exception, string message, params object[] objects) : base(exception, message, objects)
        {
        }

        protected TypeResolveException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}