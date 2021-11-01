using System;

namespace Ticketeer.API.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false,
                   Inherited = true)] 
    // AttributeTargets.All | AttributeTargets.Property | AttributeTargets.Field
    public class AllowAnonymousAttribute : Attribute
    {
    }
}
