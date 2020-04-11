using System.Text;
using Application.Services.Implementation;
using Application.Services.Interfaces;
using AutoMapper;
using BookCrossingBackEnd.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using BookCrossingBackEnd.Validators;
using FluentValidation.AspNetCore;
using RequestService = Application.Services.Implementation.RequestService;
using Infrastructure.RDBMS;
using Domain.RDBMS;

namespace BookCrossingBackEnd
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string localConnection = Configuration.GetConnectionString("DefaultConnection");
            // Please download appsettings.json for connecting to Azure DB
            //string azureConnection = Configuration.GetConnectionString("AzureConnection");
            services.AddDbContext<BookCrossingContext>(options =>
                options.UseSqlServer(localConnection, x => x.MigrationsAssembly("BookCrossingBackEnd")));

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new Application.Mapper());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UsersService>();
            services.AddScoped<IRequestService, RequestService>();
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IBookService, BookService>();                     

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().Build());
            });


            services.AddMvc(options =>
            {
                options.Filters.Add(new ModelValidationFilter());
            })
            .AddFluentValidation(cfg =>
            {
                cfg.RegisterValidatorsFromAssemblyContaining<AuthorValidator>();
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    };

                });


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SoftServe BookCrossing", Version = "v1" });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();


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
