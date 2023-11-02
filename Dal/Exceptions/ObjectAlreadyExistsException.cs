using System;
namespace Dal.Exceptions
{

    public class ObjectAlreadyExistsException : ApplicationException
    {
        public ObjectAlreadyExistsException() { }

        public ObjectAlreadyExistsException(string message) : base(message) { }

        public ObjectAlreadyExistsException(string message, Exception inner) : base(message, inner) { }
    }
}

