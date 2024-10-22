using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PostalService.models;

namespace PostalService
{
    public class DatabaseContext:DbContext
    {

        public DbSet<Order> Orders{get;set;}
        public DbSet<District> Districts{get;set;}
        public DbSet<deliveryOrder> DeliveryOrder{get;set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=postalservice.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasOne(o => o.District)
                .WithMany(d => d.Orders)
                .HasForeignKey(o => o.DistrictId)
                .HasPrincipalKey(d => d.Id)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<District>()
                .HasIndex(d => d.Name)
                .IsUnique()
                ;
        }
    }
}