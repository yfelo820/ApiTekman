using System.Net;

namespace Api.Exceptions
{
    public class BadRequestException : HttpException
    {
        public string Code { get; }

        public BadRequestException(string code, string message) : base(HttpStatusCode.BadRequest, message)
        {
            Code = code;
        }
    }
}
