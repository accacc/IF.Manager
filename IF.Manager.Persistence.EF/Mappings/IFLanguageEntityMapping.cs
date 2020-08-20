using IF.Manager.Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Persistence.EF.Mappings
{
    public class IFLanguageEntityMapping : IEntityTypeConfiguration<IFLanguageEntity>
    {
        public void Configure(EntityTypeBuilder<IFLanguageEntity> builder)
        {
            builder.ToTable("IFLanguageEntity");
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.EntityPropertyId).IsRequired();
            
        }
    }
}
