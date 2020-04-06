using System;
using Infastructure;
using Infrastructure;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BookCrossingBackEnd
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<BookCrossingContext>();
                    DataInitializer.Initialize(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                 .UseStartup<Startup>()/*(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })*/
            .ConfigureLogging(
                builder =>
                {
                    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                    var isDevelopment = environment == Microsoft.AspNetCore.Hosting.EnvironmentName.Development;
                    if (isDevelopment)
                    {
                       builder.AddApplicationInsights("1efe21aa-574a-49cc-ab53-8e93c75074bf");
                       builder.AddFilter<Microsoft.Extensions.Logging.ApplicationInsights.ApplicationInsightsLoggerProvider>
                                         ("", LogLevel.Information);
                    }
                    else
                    {
                        builder.AddApplicationInsights("1f191e43-3248-4c80-9160-d12ba9f10044");
                        builder.AddFilter<Microsoft.Extensions.Logging.ApplicationInsights.ApplicationInsightsLoggerProvider>
                                         ("", LogLevel.Information);
                    }

                });
    }
}
