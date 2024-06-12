using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MVCQLCHTHUOC.Middleware
{
    public class JwtTokenMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtTokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var jwtToken = context.Session.GetString("JwtToken");

            if (!string.IsNullOrEmpty(jwtToken))
            {
                context.Request.Headers["Authorization"] = $"Bearer {jwtToken}";
            }

            await _next(context);
        }
    }
}
