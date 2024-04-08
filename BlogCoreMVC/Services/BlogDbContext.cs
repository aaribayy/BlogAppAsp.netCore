using BlogCoreMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogCoreMVC.Services
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<AuthorTb> authors { get; set; }
        public DbSet<BlogTb> blogs { get; set; }
    }
}
