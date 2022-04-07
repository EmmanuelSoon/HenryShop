using Microsoft.EntityFrameworkCore;

namespace CA1.Models
{
    public class DBContext: DbContext
    {

        public DBContext(DbContextOptions<DBContext> options)
            : base(options)
        {
        }


        public DbSet<Product> Products { get; set; }
        

        
    }

}
