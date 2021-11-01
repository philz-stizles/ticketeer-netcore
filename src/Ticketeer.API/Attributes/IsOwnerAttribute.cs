using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Ticketeer.API.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false,
                   Inherited = true)]
    // AttributeTargets.All | AttributeTargets.Property | AttributeTargets.Field
    public class IsOwnerAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
