using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Inkton.Nester;
using Inkton.Nester.Models;

namespace Wppod.Models
{
    public class CafeContext : DbContext
    {
        public CafeContext (DbContextOptions<CafeContext> options)
            : base(options)
        {
        }

        public DbSet<Wppod.Models.User> Users { get; set; }
        public DbSet<Wppod.Models.Menu> Menus { get; set; }        
        public DbSet<Wppod.Models.MenuItem> MenuItems { get; set; }
        public DbSet<Wppod.Models.Order> Orders { get; set; }
        public DbSet<Wppod.Models.OrderItem> OrderItems { get; set; }
        public DbSet<Wppod.Models.Stock> Stocks { get; set; }        
        public DbSet<Wppod.Models.StockItem> StockItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Menu>().ToTable("Menu");            
            modelBuilder.Entity<MenuItem>().ToTable("MenuItem");
            modelBuilder.Entity<Order>().ToTable("Order");
            modelBuilder.Entity<OrderItem>().ToTable("OrderItem");
            modelBuilder.Entity<Stock>().ToTable("Stock");
            modelBuilder.Entity<StockItem>().ToTable("StockItem");

            modelBuilder.Entity<User>()
                .HasKey(user => new { user.Id });

            modelBuilder.Entity<Menu>()
                .HasKey(menu => new { menu.Id });

            modelBuilder.Entity<MenuItem>()
                .HasKey(menuItem => new { menuItem.Id });

            modelBuilder.Entity<Order>()
                .HasKey(order => new { order.Id, order.UserId });

            modelBuilder.Entity<OrderItem>()
                .HasKey(orderItem => new { orderItem.Id });

            modelBuilder.Entity<Stock>()
                .HasKey(stock => new { stock.Id });

            modelBuilder.Entity<StockItem>()
                .HasKey(stockItem => new { stockItem.Id });                
        }
    }

    public static class CafeContextFactory
    {
        public static CafeContext Create(Runtime runtime)
        {
            string connectionString = string.Format(@"Server={0};database={1};uid={2};pwd={3};",
                                    runtime.MySQL.Host,
                                    runtime.MySQL.Resource,
                                    runtime.MySQL.User,                        
                                    runtime.MySQL.Password);

            var optionsBuilder = new DbContextOptionsBuilder<CafeContext>();
            optionsBuilder.UseMySql(connectionString);
            var context = new CafeContext(optionsBuilder.Options);
            context.Database.EnsureCreated();

            return context;
        }
    }    
}
