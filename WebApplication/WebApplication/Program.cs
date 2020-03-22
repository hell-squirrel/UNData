using Microsoft.AspNetCore.Hosting;
using Serilog;
using System;
using Microsoft.Extensions.Hosting;
using Serilog.Sinks.Elasticsearch;

namespace WebApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Information("Starting...");
            CreateHostBuilder(args).Build().Run();
            Log.Information("Started Successfully");
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((ctx, config) =>
                {
                    config.ReadFrom.Configuration(ctx.Configuration);
                    config.WriteTo.Elasticsearch(
                        new ElasticsearchSinkOptions(
                            new Uri(ctx.Configuration["ConnectionStrings:ElasticSearchConnection"]))
                        {
                            AutoRegisterTemplate = true,
                            EmitEventFailure = 
                                EmitEventFailureHandling.WriteToSelfLog |
                                EmitEventFailureHandling.RaiseCallback|
                                EmitEventFailureHandling.ThrowException,
                            FailureCallback = e => { Log.Error("Unable to submit event " + e.MessageTemplate); }

                        });
                })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        
    }
}