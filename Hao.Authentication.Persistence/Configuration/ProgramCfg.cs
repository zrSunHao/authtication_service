﻿using Hao.Authentication.Persistence.Database;
using Hao.Authentication.Persistence.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hao.Authentication.Persistence.Configuration
{
    public class ProgramCfg : BaseEntityCfg<Program>
    {
        public override void EntityConfigure(EntityTypeBuilder<Program> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(32).IsRequired();

            builder.Property(x => x.Code).HasMaxLength(32).IsRequired();

            builder.Property(x => x.Type).HasMaxLength(32).IsRequired();

            builder.Property(x => x.Remark).HasMaxLength(256);
        }
    }
}
