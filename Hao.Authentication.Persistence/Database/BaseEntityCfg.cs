using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hao.Authentication.Persistence.Database
{
    public abstract class BaseEntityCfg<T> : IEntityTypeConfiguration<T> where T : BaseEntity
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.ToTable(typeof(T).Name);

            builder.HasKey(c => c.Id);

            this.EntityConfigure(builder);

            builder.Property(x => x.Id).HasMaxLength(32);
            builder.Property(x => x.CreatedById).HasMaxLength(32);
            builder.Property(x => x.LastModifiedById).HasMaxLength(32);
            builder.Property(x => x.DeletedById).HasMaxLength(32);
            builder.Property(x => x.Deleted).HasDefaultValue(false);
        }

        public abstract void EntityConfigure(EntityTypeBuilder<T> builder);
    }
}
