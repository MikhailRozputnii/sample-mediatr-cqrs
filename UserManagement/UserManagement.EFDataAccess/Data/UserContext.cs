using Microsoft.EntityFrameworkCore;
using UserManagement.Domain;
using UserManagement.EFDataAccess.Configs;

namespace UserManagement.EFDataAccess.Data
{
    /// <summary>
    /// This class use for fill data to DbSet and set configuration to using migrations
    /// </summary>
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public UserContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserConfigs());
        }
    }
}
