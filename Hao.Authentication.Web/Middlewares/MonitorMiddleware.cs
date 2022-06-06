using Hao.Authentication.Domain.Interfaces;
using Hao.Authentication.Persistence.Database;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Hao.Authentication.Web.Middlewares
{
    public class MonitorMiddleware
    {
        private readonly RequestDelegate _next;

        public MonitorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            // Do work that can write to the Response.
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            // Do logging or other work that doesn't write to the Response.
            watch.Stop();
            Console.WriteLine($"本次请求耗时{watch.ElapsedMilliseconds}毫秒");
        }
    }
}
