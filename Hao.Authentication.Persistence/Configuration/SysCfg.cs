using Hao.Authentication.Persistence.Database;
using Hao.Authentication.Persistence.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hao.Authentication.Persistence.Configuration
{
    public class SysCfg : BaseEntityCfg<Sys>
    {
        public override void EntityConfigure(EntityTypeBuilder<Sys> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(32).IsRequired();

            builder.Property(x => x.Code).HasMaxLength(32).IsRequired();

            builder.Property(x => x.Intro).HasMaxLength(256);

            builder.Property(x => x.Remark).HasMaxLength(256);
        }
    }
}
