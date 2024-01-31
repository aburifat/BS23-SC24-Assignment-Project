using BS23_SC24_Assignment_Backend.Enums;
using BS23_SC24_Assignment_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace BS23_SC24_Assignment_Backend.Context
{
    public class AppDbContext : DbContext
    {
        private IConfiguration _configuration;

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Tasks> Tasks { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration) : base(options) {
            _configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    UserName = _configuration["DefaultAdminUser:UserName"],
                    Email = _configuration["DefaultAdminUser:Email"],
                    Password = BCrypt.Net.BCrypt.HashPassword(_configuration["DefaultAdminUser:Password"]),
                    UserRole = UserRole.Administrator
                }
            );
        }
    }
}
