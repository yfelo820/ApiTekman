using Api.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filters
{
    public class HttpExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is HttpException)
            {
                if (context.Exception is BadRequestException badRequestException)
                {
                    context.Result = new BadRequestObjectResult(badRequestException.Code);
                    context.ExceptionHandled = true;
                }
                else if (context.Exception is NotFoundException notFoundException)
                {
                    context.Result = new NotFoundObjectResult(notFoundException.Message);
                    context.ExceptionHandled = true;
                }
            }
        }
    }
}