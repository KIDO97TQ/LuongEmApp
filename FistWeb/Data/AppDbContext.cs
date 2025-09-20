using FistWeb.Data.DTOs;
using FistWeb.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FistWeb.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Order> Order { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Users>().HasNoKey();
            modelBuilder.Entity<Users>().HasKey(u => u.UserId);
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Users>()
                .ToTable("users", "clothings");

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Order>()
                .ToTable("orders", "clothings");

            modelBuilder.Entity<DoanhThuThueDoDto>().HasNoKey();
            modelBuilder.Entity<RentalSummary>().HasNoKey();
            modelBuilder.Entity<InfoThueDoDto>().HasNoKey();

        }
    }
}
