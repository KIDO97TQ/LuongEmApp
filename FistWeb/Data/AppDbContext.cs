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
        public DbSet<Products> Products { get; set; }       
        public DbSet<ListParaUser> ListParaUsers { get; set; }
        public DbSet<ListParaSP> ListParaSP { get; set; }
        public DbSet<ProductStock> ProductStock { get; set; }
        public DbSet<ProductImageDto> ProductImageDto { get; set; }
        public DbSet<OrderDetailDto> OrderDetailDto { get; set; }
        public DbSet<ListParamaterMakeup> ListParamaterMakeup { get; set; }
        public DbSet<RentalSummaryMakeup> RentalSummaryMakeup { get; set; }
        public DbSet<InfoMakeUp> InfoMakeUp { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<users>().HasKey(u => u.UserId);
            modelBuilder.Entity<Products>().HasKey(u => u.productid);

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<users>()
                .ToTable("users", "clothings");

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Order>()
                .ToTable("orders", "clothings");

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Products>()
                .ToTable("products", "clothings");

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Paramater>()
                .ToTable("paramater", "clothings");

            modelBuilder.Entity<DoanhThuThueDoDto>().HasNoKey();
            modelBuilder.Entity<RentalSummary>().HasNoKey();
            modelBuilder.Entity<InfoThueDoDto>().HasNoKey();
            modelBuilder.Entity<ListParaUser>().HasNoKey();
            modelBuilder.Entity<ListParaSP>().HasNoKey();
            modelBuilder.Entity<ProductStock>().HasNoKey();
            modelBuilder.Entity<ProductImageDto>().HasKey(u => u.ProductID);
            modelBuilder.Entity<OrderDetailDto>().HasNoKey();
            modelBuilder.Entity<ListParamaterMakeup>().HasNoKey();
            modelBuilder.Entity<RentalSummaryMakeup>().HasNoKey();
            modelBuilder.Entity<InfoMakeUp>().HasNoKey();
        }
    }
}
