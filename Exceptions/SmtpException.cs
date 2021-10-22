using System;

namespace Api.Exceptions
{
    public class SmtpException : Exception
    {
        public SmtpException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
