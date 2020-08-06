using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using DatingApi.Data.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace DatingApi
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
            services.AddDbContext<DatingDbContext>((options) => {
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnectionString"));
            });

            // services.AddDbContext<DatingDbContext>((options) => {
            //     options.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionString"));
            // });

            services.AddControllers();
            services.AddScoped<IAuthorization, Authorization>();
            services.AddScoped<IUserManager, UserManager>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options => {
                
                        var securityKey = Configuration.GetSection("AppSettings:AuthenticationSecretKey").Value;
                        var securityKeyInBytes = System.Text.Encoding.UTF8.GetBytes(securityKey);
                        var IssuerSigningKey = new SymmetricSecurityKey(securityKeyInBytes);
                        
                        options.TokenValidationParameters = new TokenValidationParameters()
                        {
                            ValidateIssuerSigningKey = true,
                            ValidateAudience = false,
                            ValidateIssuer = false,
                            IssuerSigningKey = IssuerSigningKey  
                        };
                    });
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
                app.UseExceptionHandler(builder =>
                {
                    builder.Run(AddCustomErrorHandler());
                });

            }

            //app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(p => {
                p.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin();
            });
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private RequestDelegate AddCustomErrorHandler()
        {
            return async context =>
            {
                context.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;

                var error = context.Features.Get<IExceptionHandlerFeature>();
                if (error != null)
                {
                    context.Response.Headers.Add("Application-Error", error.Error.Message);
                    context.Response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
                    context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

                    await context.Response.WriteAsync(error.Error.Message);
                }
            };
        }
    }
}
