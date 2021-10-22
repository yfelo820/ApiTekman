using System;

namespace Api.Exceptions
{
    public class AspNetIdentityException : Exception
    {
        public AspNetIdentityException(string message) : base(message) { }
    }
}
