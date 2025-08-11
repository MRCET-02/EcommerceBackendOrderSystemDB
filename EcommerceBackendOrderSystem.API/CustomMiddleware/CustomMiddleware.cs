using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace EcommerceBackendOrderSystem.API.CustomMiddleware
{
    public class CustomMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Before next middleware
            Console.WriteLine($"[Middleware] Request: {context.Request.Method} {context.Request.Path}");

            await _next(context);

            // After next middleware
            Console.WriteLine($"[Middleware] Response: {context.Response.StatusCode}");
        }
    }
}
