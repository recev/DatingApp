using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class DatingDbContext: DbContext
    {
        public DatingDbContext(DbContextOptions<DatingDbContext> options)
        : base(options)
        {
            
        }

        public DbSet<Value> Values { get; set; }
    }
}