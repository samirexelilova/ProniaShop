using Microsoft.EntityFrameworkCore;
using ProniaShop.Models;

namespace ProniaShop.DAL
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Slide> slides { get; set; }
    }
}
