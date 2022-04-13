﻿using Microsoft.EntityFrameworkCore;
using CA1.Models;

namespace CA1.Data
{
    public class DBContext: DbContext
    {

        public DBContext(DbContextOptions<DBContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ShopCart> ShopCarts { get; set; }
        public DbSet<ShopCartItem> ShopCartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<InventoryRecord> InventoryRecords { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set;}
        public DbSet<InsufficientStock> InsufficientStocks { get; set; }
        
    }

}