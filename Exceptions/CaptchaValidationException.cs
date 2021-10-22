using System.Net;

namespace Api.Exceptions
{
    public class CaptchaValidationException : BadRequestException
    {
        public CaptchaValidationException(string message) : base(BadRequestCode.RecaptchaError, message)
        {
        }
    }
}
