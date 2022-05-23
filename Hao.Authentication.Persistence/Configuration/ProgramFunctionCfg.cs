using Hao.Authentication.Persistence.Database;
using Hao.Authentication.Persistence.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hao.Authentication.Persistence.Configuration
{
    public class ProgramFunctionCfg : BaseEntityCfg<ProgramFunction>
    {
        public override void EntityConfigure(EntityTypeBuilder<ProgramFunction> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(32).IsRequired();

            builder.Property(x => x.Code).HasMaxLength(32).IsRequired();

            builder.Property(x => x.SectionId).HasMaxLength(32).IsRequired();

            builder.Property(x => x.Remark).HasMaxLength(256);

            builder.HasIndex(x => x.SectionId);
        }
    }
}
