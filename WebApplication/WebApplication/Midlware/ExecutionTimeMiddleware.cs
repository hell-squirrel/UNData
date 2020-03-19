using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using WebApplication.Controllers.v1;

namespace WebApplication.Midlware
{
    public class ExecutionTimeMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExecutionTimeMiddleware> _logger;
        public ExecutionTimeMiddleware(RequestDelegate next,ILogger<ExecutionTimeMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            await _next(context);
            stopwatch.Stop();
            _logger.Log(LogLevel.Information,"Execution time of {1}: {2}ms",
                new object[]{context.Request.Path,stopwatch.ElapsedMilliseconds});
        }
    }
}