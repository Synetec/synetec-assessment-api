using System;
using System.Runtime.Serialization;

namespace SynetecAssessmentApi.Exceptions
{
    public class InvalidEmployeeIdException: Exception
    {
        public InvalidEmployeeIdException(string message) : base(message) { }

        public InvalidEmployeeIdException(string message, Exception exception) : base(message, exception) { }

        public InvalidEmployeeIdException(SerializationInfo info, StreamingContext context): base(info,context){}

    }
}
