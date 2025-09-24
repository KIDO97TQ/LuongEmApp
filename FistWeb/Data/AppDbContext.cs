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

        public DbSet<users> Users { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Paramater> Paramater { get; set; }
        public DbSet<ListParaUser> ListParaUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<users>().HasKey(u => u.UserId);

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<users>()
                .ToTable("users", "clothings");

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Order>()
                .ToTable("orders", "clothings");

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Paramater>()
                .ToTable("paramater", "clothings");

            modelBuilder.Entity<DoanhThuThueDoDto>().HasNoKey();
            modelBuilder.Entity<RentalSummary>().HasNoKey();
            modelBuilder.Entity<InfoThueDoDto>().HasNoKey();
            modelBuilder.Entity<ListParaUser>().HasNoKey();
        }
    }
}
