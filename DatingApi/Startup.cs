using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

using AutoMapper;
using Newtonsoft.Json;

using Data;
using DatingApi.Data.Repositories;
using DatingApi.Settings;
using DatingApi.Filters;
using DatingApi.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

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
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionString"))
                        .EnableSensitiveDataLogging();
            });

            // services.AddDbContext<DatingDbContext>((options) => {
            //     options.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionString"));
            // });

            var identityBuilder = services.AddIdentityCore<User>(options => {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            });

            identityBuilder = new Microsoft.AspNetCore.Identity.IdentityBuilder(identityBuilder.UserType, typeof(Role), services);

            identityBuilder.AddEntityFrameworkStores<DatingDbContext>();
            identityBuilder.AddRoleValidator<RoleValidator<Role>>();
            identityBuilder.AddRoleManager<RoleManager<Role>>();
            identityBuilder.AddSignInManager<SignInManager<User>>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options => {
                
                        var securityKey = Configuration.GetSection("AuthenticationSettings:SecretKey").Value;
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
            
            services.AddAuthorization(options => {
                options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
                options.AddPolicy("MemberPolicy", policy => policy.RequireRole("Member"));
                options.AddPolicy("ModeratorPolicy", policy => policy.RequireRole("Admin", "Moderator"));
            });

            services.AddControllers(options => {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                options.Filters.Add(new AuthorizeFilter(policy));
            });
            services.AddControllers().AddNewtonsoftJson(setup => {
                setup.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            services.AddScoped<IAuthorization, Authorization>();
            services.AddScoped<IUserRepository, userRepository>();
            services.AddScoped<IPhotoRepository, PhotoRepository>();
            services.AddScoped<ILikeRepository, LikeRepository>();
            services.AddScoped<ImessageRepository, MessageRepository>(); 
            services.AddScoped<LogUserActivity>();
            services.Configure<CloudinarySettings>(this.Configuration.GetSection("CloudinarySettings"));
            services.Configure<AuthenticationSettings>(this.Configuration.GetSection("AuthenticationSettings"));

            services.AddAutoMapper(typeof(userRepository).Assembly);
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

            app.UseDefaultFiles();
            app.UseStaticFiles();
            
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
                endpoints.MapFallbackToController("Index", "Fallback");
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
