using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Commender.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class BasicAuthentication
    {
        private readonly RequestDelegate _next;

        public BasicAuthentication(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            string authHeader = httpContext.Request.Headers["Authorization"];
            if(authHeader != null && authHeader.StartsWith("Basic"))
            {
                string encodeUsernameAndPassword = authHeader.Substring("Basic ".Length).Trim();
                Encoding encoding = Encoding.GetEncoding("UTF-8");
                string usernameAndPassword = encoding.GetString(Convert.FromBase64String(encodeUsernameAndPassword));
                int index = usernameAndPassword.IndexOf(":");
                var username = usernameAndPassword.Substring(0, index);
                var password = usernameAndPassword.Substring(index + 1);
                if(username.Equals("koushik") && password.Equals("koushiks"))
                {
                    await _next.Invoke(httpContext);
                }

                else
                {
                    httpContext.Response.StatusCode = 401;
                    return;
                }
            }
            else
            {
                httpContext.Response.StatusCode = 401;
                return;
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class BasicAuthenticationExtensions
    {
        public static IApplicationBuilder UseBasicAuthentication(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BasicAuthentication>();
        }
    }
}
