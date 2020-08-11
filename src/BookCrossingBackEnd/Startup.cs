using System;
using System.IO;
using Application.Services.Implementation;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Application;
using Hangfire;
using Hangfire.MemoryStorage;
using BookCrossingBackEnd.ServiceExtension;
using Infrastructure.RDBMS;
using Microsoft.Data.SqlClient;
using System.Linq;
using Hangfire.Dashboard;

namespace BookCrossingBackEnd
{
    public class Startup
    {
        public Startup(IConfiguration configuration, ILogger<Startup> logger, IWebHostEnvironment environment)
        {
            Environment = environment;
            Configuration = configuration;
            _logger = logger;
        }

        private IConfiguration Configuration { get; }

        private IWebHostEnvironment Environment { get; }

        private readonly ILogger _logger;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext(Configuration, Environment);

            services.AddGoodreadsSource(Configuration);

            services.AddMongoSettings(Configuration, Environment);

            services.AddEmailService(Configuration, Environment);

            services.AddMapper();

            services.AddScoped<ICommentOwnerMapper, CommentOwnerMapper>();

            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            services.AddRepositories();

            services.AddCustomServices();

            services.AddLogging();

            services.AddApplicationInsightsTelemetry();

            services.AddCorsSettings();

            services.AddMVCWithFluentValidatoin();

            services.AddJWTAuthenticatoin(Configuration);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SoftServe BookCrossing", Version = "v1" });
            });

            services.AddHangfire(config =>
            config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseDefaultTypeSerializer()
            .UseMemoryStorage()
            );

            services.AddScoped<IAphorismService, AphorismService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IRecurringJobManager recurringJobManager, IServiceProvider serviceProvider)
        {
            if (Environment.IsDevelopment())
            {
                _logger.LogInformation("Configuring for Development environment");
                app.UseDeveloperExceptionPage();
            }
            else
            {
                _logger.LogInformation("Configuring for Production environment");
            }

            try
            {
                using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
                {
                    var context = serviceScope.ServiceProvider.GetRequiredService<BookCrossingContext>();
                    context.Database.Migrate();
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex.ToString());
            }

            app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
                    ctx.Context.Response.Headers.Append("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
                },
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot")),
                RequestPath = new PathString("")
            });

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = Enumerable.Empty<IDashboardAuthorizationFilter>()
            });
            app.UseHangfireServer(new BackgroundJobServerOptions
            {
                WorkerCount = 1
            });

            recurringJobManager.AddOrUpdate(
                "Run every day",
                () => serviceProvider.GetService<IAphorismService>().MoveToNextAsync(),
                "59 23 * * *",
                TimeZoneInfo.Local
            );

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SoftServe BookCrossing");
            });
        }

    }
}