using BS23_SC24_Assignment_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace BS23_SC24_Assignment_Backend.Context
{
    public class AppDbContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Tasks> Tasks { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }  
    }
}
