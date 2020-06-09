using Application.Dto.Email;
using Application.Services.Implementation;
using Application.Services.Interfaces;
using AutoMapper;
using BookCrossingBackEnd.Filters;
using BookCrossingBackEnd.Validators;
using Domain.NoSQL;
using FluentValidation.AspNetCore;
using Hangfire;
using Infrastructure.NoSQL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCrossingBackEnd.ServiceExtension
{
    public static class ServiceExtension
    {
        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IBookChildCommentService, BookChildCommentService>();
            services.AddScoped<IBookRootCommentService, BookRootCommentService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UsersService>();
            services.AddScoped<IEmailSenderService, EmailSenderService>();
            services.AddScoped<IRequestService, RequestService>();
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IUserResolverService, UserResolverService>();
            services.AddScoped<IGenreService, GenreService>();
            services.AddScoped<IHangfireJobScheduleService, HangfireJobSchedulerService>();
            services.AddSingleton<IImageService, ImageService>();
            services.AddSingleton<IPaginationService, PaginationService>();
            services.AddSingleton<ISmtpClient, SmtpClientService>();
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(Domain.NoSQL.IChildRepository<,>), typeof(Infrastructure.NoSQL.BaseChildRepository<,>));
            services.AddScoped(typeof(Domain.NoSQL.IRootRepository<>), typeof(Infrastructure.NoSQL.BaseRootRepository<>));
            services.AddScoped(typeof(Domain.RDBMS.IRepository<>), typeof(Infrastructure.RDBMS.BaseRepository<>));
        }

        public static void AddJWTAuthenticatoin(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                        ClockSkew = TimeSpan.Zero
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Add("Token-Expired", "true");
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
        }

        public static void AddDbContext(this IServiceCollection services,IConfiguration configuration)
        {
            string localConnection = configuration.GetConnectionString("DefaultConnection");
            // Please download appsettings.json for connecting to Azure DB
            string azureConnection = configuration.GetConnectionString("AzureConnection");
            services.AddDbContext<Infrastructure.RDBMS.BookCrossingContext>(options =>
                options.UseSqlServer(localConnection, x => x.MigrationsAssembly("BookCrossingBackEnd")));
        }

        public static void AddMongoSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoSettings>(
                configuration.GetSection(nameof(MongoSettings)));

            services.AddSingleton<IMongoSettings>(sp =>
                sp.GetRequiredService<IOptions<MongoSettings>>().Value);
        }

        public static void AddMapper(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new Application.MapperProfilers.AuthorProfile());
                mc.AddProfile(new Application.MapperProfilers.BookChildCommentProfile());
                mc.AddProfile(new Application.MapperProfilers.BookRootCommentProfile());
                mc.AddProfile(new Application.MapperProfilers.GenreProfile());
                mc.AddProfile(new Application.MapperProfilers.LocationProfile());
                mc.AddProfile(new Application.MapperProfilers.RequestProfile());
                mc.AddProfile(new Application.MapperProfilers.UserProfile());
                mc.AddProfile(new Application.MapperProfilers.BookProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        public static void AddEmailService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(config =>
               config.UseSqlServerStorage(configuration.GetConnectionString("AzureConnection")));
            services.AddHangfireServer(options => options.SchedulePollingInterval = TimeSpan.FromSeconds(10));

            var emailConfig = configuration
                .GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);
        }
        public static void AddCorsSettings(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().Build());
            });
        }
        public static void AddMVCWithFluentValidatoin(this IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                //options.Filters.Add(new ModelValidationFilter());
            })
          .AddFluentValidation(cfg =>
          {
              cfg.RegisterValidatorsFromAssemblyContaining<AuthorValidator>();
          });
        }

    }
}
