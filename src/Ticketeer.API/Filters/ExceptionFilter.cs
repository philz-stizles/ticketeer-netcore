using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using Ticketeer.Application.Exceptions;

namespace Ticketeer.API.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            if (exception is NotFoundException e)
            {
                context.Result = new JsonResult(e.Message)
                {
                    StatusCode = (int)HttpStatusCode.NotFound
                };
            }
        }
    }
}
