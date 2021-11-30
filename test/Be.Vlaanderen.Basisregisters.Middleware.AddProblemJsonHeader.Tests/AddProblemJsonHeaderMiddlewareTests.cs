namespace Be.Vlaanderen.Basisregisters.Middleware.AddProblemJsonHeader.Tests
{
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.AspNetCore.Http;
    using Xunit;

    public class AddProblemJsonHeaderMiddlewareTests
    {
        [Fact]
        public async Task AddsProblemJsonToRequestHeaders()
        {
            var middleware = new AddProblemJsonHeaderMiddleware(innerContext => Task.CompletedTask);
            var context = new DefaultHttpContext();
            context.Request.Headers.Add("Accept", "application/ld+json");

            await middleware.Invoke(context);

            context.Request.GetTypedHeaders().Accept
                .Select(x => x.MediaType.Value)
                .Should()
                .Contain("application/problem+json")
                .And
                .Contain("application/ld+json");
        }

        [Fact]
        public async Task DoesNotAddProblemJsonToRequestHeaders()
        {
            var middleware = new AddProblemJsonHeaderMiddleware(innerContext => Task.CompletedTask);
            var context = new DefaultHttpContext();
            context.Request.Headers.Add("Accept", "application/json");

            await middleware.Invoke(context);

            context.Request.GetTypedHeaders().Accept
                .Select(x => x.MediaType.Value)
                .Should()
                .NotContain("application/problem+json");
        }
    }
}
