using AutoMapper;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services;
using Services.Repository;
using Services.Repository.Config;
using Services.Repository.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddControllers();
            services.AddScoped<IProductsRepository, ProductsRepository>();
            services.AddScoped<IReleaseNotesRepository, ReleaseNotesRepository>();
            services.AddScoped<IWorkItemRepository, WorkItemRepository>();
            services.Configure<SqlDbConnection>(Configuration.GetSection("SqlDbConfiguration"));
            services.AddAutoMapper(typeof(Startup), typeof(Services.Repository.Models.DatabaseModels.Product));

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                /*.AddIdentityServerAuthentication(options =>
                {
                    options.Authority = Configuration["Authentication:IdentityServerUrl"];
                    options.RequireHttpsMetadata = false;
                    options.ApiName = Configuration["Authentication:ApiName"];
                });*/
                .AddIdentityServerAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = Configuration["Authentication:IdentityServerUrl"];
                    options.RequireHttpsMetadata = !IsDevelopment();
                    options.Audience = Configuration["Authentication:ApiName"];
                    options.TokenValidationParameters.ValidIssuers = GetValidIssuersForEnvironment();
                    var handler = new JwtSecurityTokenHandler
                    {
                        MapInboundClaims = false
                    };
                    options.TokenValidationParameters.NameClaimType = "name";
                    options.TokenValidationParameters.RoleClaimType = "role";
                    options.SecurityTokenValidators.Clear();
                    options.SecurityTokenValidators.Add(handler);
                },
                    _ => { }
                    );
        }

        public static string GetEnvironmentName()
        {
            return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
        }
        public static bool IsDevelopment()
        {
            return GetEnvironmentName().Equals("Development", StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsTest()
        {
            return GetEnvironmentName().Equals("Test", StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsProduction()
        {
            return GetEnvironmentName().Equals("Prod", StringComparison.OrdinalIgnoreCase) || GetEnvironmentName().Equals("Production", StringComparison.OrdinalIgnoreCase);
        }

        public static string[] GetValidIssuersForEnvironment()
        {
            if (IsDevelopment())
            {
                return new[]
                {
                    "https://localhost:44333"
                };
            }

            if (IsTest())
            {
                return new[]
                {
                    "https://test-login.hrmts.net",
                    "https://test-login.talentech.io"
                };
            }

            if (IsProduction())
            {
                return new[]
                {
                    "https://login.hrmts.net",
                    "https://login.talentech.io"
                };
            }

            throw new ApplicationException("GetValidIssuersForEnvironment failed. Valid environment not found.");
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
