using System.Collections.Immutable;
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
        public DbSet<Like> Likes { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Like>()
                .HasKey( l => new { l.SenderId, l.ReceivedId});

            modelBuilder.Entity<Like>()
                .HasOne(l => l.Sender)
                .WithMany(u => u.SendedLikes)
                .HasForeignKey(l => l.SenderId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Like>()
                .HasOne(l => l.Receiver)
                .WithMany(u => u.ReceivedLikes)
                .HasForeignKey(l => l.ReceivedId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.SentMessages)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Recipient)
                .WithMany(u => u.ReceivedMessages)
                .OnDelete(DeleteBehavior.NoAction);

            //base.OnModelCreating(modelBuilder);
        }
    }
}