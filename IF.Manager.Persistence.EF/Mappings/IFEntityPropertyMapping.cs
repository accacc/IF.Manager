using IF.Manager.Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Persistence.EF.Mappings
{   
    public class IFEntityPropertyMapping : IEntityTypeConfiguration<IFEntityProperty>
    {
        public void Configure(EntityTypeBuilder<IFEntityProperty> builder)
        {
            builder.ToTable("IFEntityProperty");
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.IsIdentity).HasDefaultValue(false);
            builder.Property(x => x.MaxValue);
            builder.Property(x => x.Type);
        }
    }
}
