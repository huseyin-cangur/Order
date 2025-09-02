

using Microsoft.EntityFrameworkCore;
using OrderAPI.Entity;

namespace OrderAPI
{
    public class OrderContext : DbContext
    {
          public OrderContext(DbContextOptions<OrderContext> options)
        : base(options)
    {
    }
       
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }
    }
}