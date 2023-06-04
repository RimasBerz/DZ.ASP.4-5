using Microsoft.EntityFrameworkCore;
using WebApplication1.Data.Entities;
namespace WebApplication1.Data
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set;}
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("asp111");
        }
    }
}
