using System.Net;

namespace Api.Exceptions
{
    public class NotFoundException : HttpException
    {
        public NotFoundException(string message) : base(HttpStatusCode.NotFound, message)
        {
        }
    }
}
