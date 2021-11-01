/*using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using Moq;
using Xunit;

namespace Ticketeer.API.Tests
{
    public class AuthorizeAttribute_Tests
    {
        [Fact]
        public void OnResultExecuting_ShoultAddTheExpectedHeaderAtTheResponse()
        {
            // Arrange (Initialize the necessary objects)

            string headersName = "aheadername";
            string headersValue = "a test header value";

            // Create a default ActionContext (depending on our case-scenario)

            var actionContext = new ActionContext()
            {
                HttpContext = new DefaultHttpContext(),
                RouteData = new RouteData(),
                ActionDescriptor = new ActionDescriptor()
            };

            // Create the filter input parameters (depending on our case-scenario)

            var resultExecutingContext = new AuthorizationContext(
                actionContext,
                    new List<IFilterMetadata>(),
                    new ObjectResult("A dummy result from the action method."),
                    Mock.Of<Controller>()
                );

            // Act (Call the method under test with the arranged parameters)

            AddHeaderAttribute addHeaderAttribute = new AddHeaderAttribute(headersName, headersValue);
            addHeaderAttribute.OnResultExecuting(resultExecutingContext);

            // Assert (Verify that the action of the method under test behaves as expected)

            Assert.Equal(1, resultExecutingContext.HttpContext.Response.Headers.Count);
            Assert.True(resultExecutingContext.HttpContext.Response.Headers.ContainsKey(headersName));
            Assert.Equal(headersValue, resultExecutingContext.HttpContext.Response.Headers[headersName]);
        }
    }
}
*/