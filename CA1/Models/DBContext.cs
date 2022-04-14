using Microsoft.EntityFrameworkCore;

namespace CA1.Models
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
        public DbSet<ProductReview> ProductReviews { get; set; }
        public DbSet<WishList> WishLists { get; set; }
        public DbSet<WishListItem> WishListItems { get; set; }
    }

}
