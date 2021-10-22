using System;

namespace Api.Exceptions
{
    public class InvalidSubjectException : Exception
    {
        public InvalidSubjectException(string message) : base(message)
        {
        }
    }
}
