using System;
using System.IO;
using Infrastructure.RDBMS;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
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
                .UseStartup<Startup>()
                .ConfigureLogging((hostingContext, logging) =>
                {

                    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                    var isDevelopment = environment == Environments.Development;
                    string appInsightKey;

                    if (isDevelopment)
                    {
                         appInsightKey = hostingContext.Configuration["iKeyForDevelop"];
                    }
                    else
                    {
                        appInsightKey = hostingContext.Configuration["iKeyForProduction"];
                    }
                    logging.AddApplicationInsights(appInsightKey);
                    logging
                        .AddFilter<Microsoft.Extensions.Logging.ApplicationInsights.
                                ApplicationInsightsLoggerProvider>
                            ("", LogLevel.Information);
                });
        
    }
}
