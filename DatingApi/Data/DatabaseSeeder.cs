using System.Collections.Generic;
using System.IO;
using System.Linq;
using Data;
using DatingApi.Data.Models;
using DatingApi.Data.Repositories;
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
        string fileName = @"C:\Work\learn\DatingApp\DatingApi\Data\DatabaseSeedData.json";

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
                    var authorization = scope.ServiceProvider.GetRequiredService<IAuthorization>();

                    context.Database.Migrate();

                    if (context.Users.Any())
                        return;
                    
                    logger.LogInformation("Seeding database started!");
                    
                    var jsonString = File.ReadAllText(fileName);
                    var seedUsers = JsonConvert.DeserializeObject<IList<User>>(jsonString);

                    foreach (var seedUser in seedUsers)
                    {
                        var createdUser = authorization.CreateUser(seedUser.Username, seedUser.Username + "p");

                        seedUser.PasswordHash = createdUser.PasswordHash;
                        seedUser.PasswordSaltKey = createdUser.PasswordSaltKey;

                        context.Users.Add(seedUser);
                    }

                    context.SaveChanges();
                    logger.LogInformation("Seeding database completed!");
                }
                catch (System.Exception ex)
                {
                    logger.LogError(ex.Message);
                }
            }
        }
    }
}