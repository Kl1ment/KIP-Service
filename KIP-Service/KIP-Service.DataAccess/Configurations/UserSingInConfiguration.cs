
using KIP_Service.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KIP_Service.DataAccess.Configurations
{
    internal class UserSingInConfiguration : IEntityTypeConfiguration<UserSingInEntity>
    {
        public void Configure(EntityTypeBuilder<UserSingInEntity> builder)
        {
            builder.HasKey(s => s.Id);
        }
    }
}
