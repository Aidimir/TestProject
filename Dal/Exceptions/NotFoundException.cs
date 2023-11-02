using System;
using System.Runtime.Serialization;

namespace Dal.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException() { }

        public NotFoundException(string message) : base(message) { }

        public NotFoundException(string message, Exception inner) : base(message, inner) { }
    }
}

