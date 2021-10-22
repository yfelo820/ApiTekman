using System;

namespace Api.Exceptions
{
    public class TKCoreException : Exception
    {
        public TKCoreException(string message) : base(message)
        {
        }
    }
}
