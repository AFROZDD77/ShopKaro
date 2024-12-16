using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ShopKaro.Models.Users;
using System.Security.Cryptography.X509Certificates;


namespace ShopKaro.Models.Users
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

        // Define DbSet for each entity/table
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            // Set default value for the 'EmailConfirmed'
            modelBuilder.Entity<User>().Property(i => i.EmailConfirmed).HasDefaultValue(false); // This will set 'EmailConfirmed' to False by default

            // Set default value for the 'City'
            modelBuilder.Entity<User>().Property(i => i.City).HasDefaultValue("India");  // This will set 'City' to 'India' by default

            base.OnModelCreating(modelBuilder);
        }
    }
}
