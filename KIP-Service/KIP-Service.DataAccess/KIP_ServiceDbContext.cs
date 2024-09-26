using KIP_Service.DataAccess.Configurations;
using KIP_Service.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace KIP_Service.DataAccess
{
    public class KIP_ServiceDbContext(IConfiguration configuration) : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<UserSingInEntity> UserSingIns { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(configuration.GetConnectionString(nameof(KIP_ServiceDbContext)));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserSingInConfiguration());


            base.OnModelCreating(modelBuilder);
        }
    }
}
