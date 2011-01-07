using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TaskHandler.Commons
{
    public class TypeResolveDuplicateObjects : IocException
    {
        public TypeResolveDuplicateObjects()
        {
        }

        public TypeResolveDuplicateObjects(string message) : base(message)
        {
        }

        public TypeResolveDuplicateObjects(Exception innerException, string message) : base(innerException, message)
        {
        }

        public TypeResolveDuplicateObjects(string message, params object[] parameters) : base(message, parameters)
        {
        }

        public TypeResolveDuplicateObjects(Exception exception, string message, params object[] objects) : base(exception, message, objects)
        {
        }

        protected TypeResolveDuplicateObjects(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}