using EcommerceApi.Model;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {

        }
        public DbSet<Product> products { get; set; }
        public DbSet<Seller> sellers { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<Cart> cart { get; set; }
        public DbSet<Orders>  orders { get; set; }
    }
}
