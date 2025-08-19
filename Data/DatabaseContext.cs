using WindowMonitorAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WindowMonitorAPI.Models;

namespace WindowMonitorAPI.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Home>()
                .HasMany(H => H.Users)
                .WithMany(U => U.Homes)
                .UsingEntity(
                    "UserHomes",
                    U => U.HasOne(typeof(User)).WithMany()
                    .HasForeignKey("UserID").HasPrincipalKey(nameof(User.UserID)),
                    H => H.HasOne(typeof(Home)).
                    WithMany().HasForeignKey("HomeID").
                    HasPrincipalKey(nameof(Home.HomeID)),
                    HU => HU.HasKey("HomeID", "UserID"));

        }
        public DbSet<Home> Homes { get; set; } = default!;
        public DbSet<User> Users { get; set; } = default!;
    }
}

