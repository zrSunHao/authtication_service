using Hao.Authentication.Persistence.Database;
using Hao.Authentication.Persistence.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hao.Authentication.Persistence.Configuration
{
    public class ProgramSectionCfg : BaseEntityCfg<ProgramSection>
    {
        public override void EntityConfigure(EntityTypeBuilder<ProgramSection> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(32).IsRequired();

            builder.Property(x => x.Code).HasMaxLength(32).IsRequired();

            builder.Property(x => x.Type).HasMaxLength(32).IsRequired();

            builder.Property(x => x.ProgramId).HasMaxLength(32).IsRequired();

            builder.Property(x => x.Remark).HasMaxLength(256);

            builder.HasIndex(x => x.ProgramId);
        }
    }
}
