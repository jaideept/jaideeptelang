using ContactApi.Middleware;
using Microsoft.AspNetCore.Builder;

namespace ContactApi.Extensions
{
    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionsHandlingMiddlewareExtension
    {
        public static IApplicationBuilder UseExceptionsHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionsHandlingMiddleware>();
        }
    }
}
