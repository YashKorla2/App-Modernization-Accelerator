
    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore;
    
    namespace WebApplication
    {
        public class Program
        {
            public static void Main(string[] args)
            {
                var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);
                
                // Store configuration in static ConfigurationManager
                ConfigurationManager.Configuration = builder.Configuration;
                
                // Add services to the container (formerly ConfigureServices)
                builder.Services.AddControllersWithViews(options => {
                    options.EnableEndpointRouting = true;
                })
                .AddMvcOptions(options => {
                    // Add client-side validation (from Web.config ClientValidationEnabled)
                    // This is enabled by default in Core, but explicitly setting for clarity
                });

                // Unity container registration
                // TODO: Implement Unity container configuration with ASP.NET Core DI
                // builder.Services.AddUnity(); // Add appropriate Unity configuration here

                // Add configuration values from Web.config appSettings
                builder.Services.Configure<MvcOptions>(options => {
                    // ClientValidationEnabled from Web.config is true by default in ASP.NET Core
                });

                //Added Services
                
                var app = builder.Build();
                
                // Configure the HTTP request pipeline (formerly Configure method)
                if (app.Environment.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }
                else
                {
                    app.UseExceptionHandler("/Home/Error");
                    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                    app.UseHsts();
                }
                
                app.UseHttpsRedirection();
                app.UseStaticFiles();
                
                //Added Middleware
                
                app.UseRouting();

                app.UseAuthorization();

                // Register areas
                app.UseEndpoints(endpoints =>
                {
                    // Register all areas
                    endpoints.MapControllerRoute(
                        name: "areas",
                        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                    // Default route from RouteConfig
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}/{id?}");
                });
                
                app.Run();
            }
        }
        
        public class ConfigurationManager
        {
            public static IConfiguration Configuration { get; set; }
        }
    }