using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Data.Common;
using System.Threading.Tasks;

namespace AspNet7.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class SaveUserInfo
    {
        private readonly RequestDelegate _next;

        public SaveUserInfo(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            string teamp = httpContext.Request.Headers["User-Agent"];
            httpContext.Items["IsChromeBrowser"] = httpContext.Request.Headers["User-Agent"].Any(p => p.ToLower().Contains("chrome"));

            await _next(httpContext);

            if (httpContext.Items["IsChromeBrowser"].ToString().ToLower() == "true")
            {
                httpContext.Response.StatusCode = 401;
            }

            if (httpContext.Response.StatusCode == 404)
            {
                await httpContext.Response.WriteAsync("This Page is 404");
            }

            if (httpContext.Request.Headers["User-Agent"].Any(p => p.ToLower().Contains("chrome")))
            {
                httpContext.Response.StatusCode = 403;
            }

            if (httpContext.Request.Path.ToString().ToLower().Contains("/content"))
            {
                await httpContext.Response.WriteAsync("This message From Content ...!");
            }
            string ip = httpContext.Connection.RemoteIpAddress.ToString();
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class SaveUserInfoExtensions
    {
        public static IApplicationBuilder UseSaveUserInfo(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SaveUserInfo>();
        }
    }
}
