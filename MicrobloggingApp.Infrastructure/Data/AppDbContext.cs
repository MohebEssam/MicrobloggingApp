using MicrobloggingApp.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace MicrobloggingApp.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }
    }
}
