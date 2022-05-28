using Hao.Authentication.Persistence.Database;
using Hao.Authentication.Persistence.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hao.Authentication.Persistence.Configuration
{
    public class CustomerCfg : BaseEntityCfg<Customer>
    {
        public override void EntityConfigure(EntityTypeBuilder<Customer> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(32).IsRequired();

            builder.Property(x => x.Password).IsRequired();

            builder.Property(x => x.PasswordSalt).IsRequired();

            builder.Property(x => x.Remark).HasMaxLength(512);

            builder.HasIndex(x => x.Name).IsUnique();
        }
    }
}
