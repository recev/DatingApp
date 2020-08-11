using DatingApi.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class DatingDbContext: DbContext
    {
        public DatingDbContext(DbContextOptions<DatingDbContext> options)
        : base(options)
        {}

        public DbSet<User> Users { get; set; }
        public DbSet<Photo> Photos { get; set; }
    }
}