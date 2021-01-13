using System.Threading.Tasks;
using System.Reflection.Metadata.Ecma335;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Data;
using DatingApi.Data.Models;
using DatingApi.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DatingApi.Data
{
    public class DatabaseSeeder
    {
        IHost _host;

        public DatabaseSeeder(IHost host)
        {
            this._host = host;
        }

        public void Start()
        {
            using (var scope = _host.Services.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

                try
                {
                    var context = scope.ServiceProvider.GetRequiredService<DatingDbContext>();
                    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

                    Migrate(context, logger);

                    SeedRoles(roleManager, logger).Wait();

                    SeedUsers(userManager, logger).Wait();

                    SeedAdminUser(userManager, logger).Wait();

                }
                catch (System.Exception ex)
                {
                    logger.LogError(ex.Message);
                }
            }
        }

        public void Migrate(DatingDbContext context, ILogger<Program> logger)
        {
            logger.LogInformation("Database migration started!");

            context.Database.Migrate();

            logger.LogInformation("Database migration completed!");

        }

        public async Task SeedRoles(RoleManager<Role> roleManager, ILogger<Program> logger)
        {
            if(roleManager.Roles.Any())
                return;

            logger.LogInformation("Seeding roles started!");

            var roles = new List<Role>{
                new Role { Name = "Member" },
                new Role { Name = "Admin" },
                new Role { Name = "VIP"},
                new Role { Name = "Moderator" }
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            logger.LogInformation("Seeding roles completed!");
        }

        public async Task SeedUsers(UserManager<User> userManager, ILogger<Program> logger)
        {
            if (userManager.Users.Any<User>())
                return;
            
            logger.LogInformation("Seeding users started!");
            
            string fileName = Path.Combine(Directory.GetCurrentDirectory(), "Data","DatabaseSeedData.json");
            var jsonString = File.ReadAllText(fileName);
            var seedUsers = JsonConvert.DeserializeObject<IList<User>>(jsonString);

            foreach (var seedUser in seedUsers)
            {
                var createUserResult = await userManager.CreateAsync(seedUser, seedUser.UserName + "p");

                if(createUserResult.Succeeded)
                {
                    var user = await userManager.FindByNameAsync(seedUser.UserName);
                    await userManager.AddToRoleAsync(user, "Member");
                }
            }

            logger.LogInformation("Seeding users completed!");
        }

        public async Task SeedAdminUser(UserManager<User> userManager, ILogger<Program> logger)
        {
            var admin = await userManager.FindByNameAsync("Admin");
            await userManager.AddToRoleAsync(admin, "Admin");
        }

    }
}