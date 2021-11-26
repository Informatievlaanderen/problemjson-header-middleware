namespace Be.Vlaanderen.Basisregisters.Middleware.AddProblemJsonHeader
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Add 'application/problem+json' to the request's Accept header if the request's Accept header contains 'application/ld+json'.
    /// </summary>
    public class AddProblemJsonHeaderMiddleware
    {
        private const string HeaderName = "Accept";
        private const string JsonLd = "application/ld+json";
        private const string JsonProblem = "application/problem+json";

        private readonly RequestDelegate _next;

        public AddProblemJsonHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            var acceptHeader = context.Request.GetTypedHeaders().Accept;
            if (acceptHeader.Any(x => x.MediaType.Value.Contains(JsonLd, StringComparison.InvariantCultureIgnoreCase))
                && !acceptHeader.Any(x => x.MediaType.Value.Contains(JsonProblem, StringComparison.InvariantCultureIgnoreCase)))
            {
                context.Request.Headers[HeaderName] = $"{context.Request.Headers[HeaderName]};{JsonProblem}";
            }
            return _next(context);
        }
    }
}
