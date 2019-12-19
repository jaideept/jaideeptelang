using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ContactApi.Repository;
using ContactApi.Exceptions;
using ContactApi.Extensions;

namespace ContactApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.(for e.g., DI)
        public void ConfigureServices(IServiceCollection services)
        {
            // profiling
            services.AddMiniProfiler(options =>
               options.RouteBasePath = "/profiler"
            );
            services.AddControllers().AddNewtonsoftJson();
            services.AddCors();
            services.AddResponseCaching();
            //ContactRepository is instantiated using dependency injection
            services.AddTransient<IContactRepository>(x => new ContactRepository(Configuration.GetConnectionString("Default")));
            services.AddMvc()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.WriteIndented = true;
            }).SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddSwaggerGen(c => 
            { 
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Contacts API", Description = "Contact API - with swagger" }); 
            });

            //registering the health check services 
            services.AddHealthChecks();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // profiling, url to see last profile check: http://localhost:xxxxx/profiler/results
            app.UseMiniProfiler();
            //app.UseCors("AllowMyOrigin");
            //enable CORS
            app.UseCors(options =>
               options.WithOrigins("http://localhost:4200")
             .AllowAnyMethod()
             .AllowAnyHeader());

            //Adding standard security headers to the response
            //ToDo - can use NWebSec library instead of applying security heasders in below manner

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Xss-Protection", "1");
                context.Response.Headers.Add("X-Frame-Options", "DENY");
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                context.Response.Headers.Add(
                    "Content-Security-Policy",
                    "default-src 'none'; " +
                    "font-src 'self' use.fontawesome.com; " +
                    "style-src 'self' 'unsafe-inline' https://stackpath.bootstrapcdn.com ; " +
                    "script-src 'self' 'unsafe-inline' https://code.jquery.com https://cdnjs.cloudflare.com https://stackpath.bootstrapcdn.com ;" +
                    "frame-src 'self';" +
                    "img-src 'self' data:;" +
                    "connect-src 'self';");
                await next();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseExceptionsHandlingMiddleware();
            }
            else
            {
                //include ExceptionHandling middleware in the ASP.NET Core pipeline
                app.UseExceptionsHandlingMiddleware();
                app.UseExceptionHandler();
            }

            //app.UseMiddleware<ExceptionsHandlingMiddleware>();


            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //Caching static resources
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = context =>
                {
                    // Cache static file for 1 year
                    if (!string.IsNullOrEmpty(context.Context.Request.Query["v"]))
                    {
                        context.Context.Response.Headers.Add("cache-control", new[] { "public,max-age=31536000" });
                        context.Context.Response.Headers.Add("Expires", new[] { DateTime.UtcNow.AddYears(1).ToString("R") }); // Format RFC1123
                    }
                }
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Contact API V1");
            });
        }
    }
}
