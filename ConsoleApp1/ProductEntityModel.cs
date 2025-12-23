using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;

namespace ConsoleApp1
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = "";
        public short UnitsInStock { get; set; }
        public decimal UnitPrice {  get; set; }
        public bool Discontinued { get; set; }
        public int CategoryId { get; set; }
        public override string ToString()
        => $"{ProductId:00}|{UnitsInStock:00}|{UnitPrice:N2}|{ProductName}";

        public void Deconstruct(out int productId, out string productName)
        {
            productId = ProductId;
            productName = ProductName;
        }

    }
    public class ProductDbContext: DbContext
    {
        public DbSet<Product> Products { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.LogTo(Console.WriteLine);
            optionsBuilder.UseSqlServer(
                "server=.;database=northwind;integrated security=sspi;trustservercertificate=true"
                );//.LogTo(Console.Write);
           
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().Property(p => p.UnitPrice).HasColumnType("decimal");
        }
    }
}
