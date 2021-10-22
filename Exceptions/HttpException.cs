using System;
using System.Net;

namespace Api.Exceptions
{
    public class HttpException : Exception
	{
		public int StatusCode { get; }

		public HttpException(HttpStatusCode httpStatusCode)
		{
			StatusCode = (int)httpStatusCode;
		}

		public HttpException(HttpStatusCode httpStatusCode, string message) : base(message)
		{
			StatusCode = (int)httpStatusCode;
		}

		public HttpException(HttpStatusCode httpStatusCode, string message, Exception inner) : base(message, inner)
		{
			StatusCode = (int)httpStatusCode;
		}
    }
}
