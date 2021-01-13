using System;
using DatingApi.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class DatingDbContext: IdentityDbContext<User, Role, string, IdentityUserClaim<string>, UserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public DatingDbContext(DbContextOptions<DatingDbContext> options)
        : base(options)
        {}

        public DbSet<Photo> Photos { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRole>().HasKey(p => new { p.UserId, p.RoleId });

            modelBuilder.Entity<User>(b =>
            {
                // Each User can have many entries in the UserRole join table
                b.HasMany(user => user.UserRoles)
                    .WithOne(userRole => userRole.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            modelBuilder.Entity<Role>(b =>
            {
                // Each Role can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
            });


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


        }
    }
}