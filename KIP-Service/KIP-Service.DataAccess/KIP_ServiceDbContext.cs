using KIP_Service.DataAccess.Configurations;
using KIP_Service.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace KIP_Service.DataAccess
{
    public class KIP_ServiceDbContext(DbContextOptions<KIP_ServiceDbContext> options) : DbContext(options)
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<UserSingInEntity> UserSingIns { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserSingInConfiguration());


            base.OnModelCreating(modelBuilder);
        }
    }
}
