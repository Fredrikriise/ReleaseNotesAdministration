using Hrid.Extensions.Builder;
using Hrid.Extensions.Model;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Helpers;
using System;

namespace ReleaseNotes 
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
            services.AddControllersWithViews();
            services.AddHttpClient("ReleaseNotesApiClient", client =>
            {
                //Fredrik:  client.BaseAddress = new Uri("https://localhost:44310");
                //Felix bærbar:  client.BaseAddress = new Uri("https://localhost:44314");
                //Felix stasjonær:  client.BaseAddress = new Uri("https://localhost:44312");
                client.BaseAddress = new Uri("https://localhost:44324");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
                .AddCookie(options =>
                {
                    options.AccessDeniedPath = new PathString("/Home/Error");
                })
                .AddHridWeb(options =>
                {
                    options.UserType = UserType.Employee;
                    options.Environment = GetEnvironmentName();
                    options.ClientId = "hrmts-releasenotes-app";
                    options.ClientSecret = "4700825d-92d3-4148-9f39-4a7c81a47b25";
                });

        }

        public static string GetEnvironmentName()
        {
            return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/ReleaseNotes/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Product}/{action=ListAllProducts}/{id?}");
            });
        }
    }
}
