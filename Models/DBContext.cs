using Microsoft.EntityFrameworkCore;
using exam3.Models;

namespace exam3.Models
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=mydb.db");
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Idea> Ideas {get; set;}
        public DbSet<Like> Likes {get; set;}
    }
}