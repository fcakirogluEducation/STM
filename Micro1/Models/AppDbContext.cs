using Microsoft.EntityFrameworkCore;

namespace Micro1.Models
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; } = default!;
    }
}