using IF.Manager.Service.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Service.Mappings
{
    public class IFCustomClassPropertyMapping : IEntityTypeConfiguration<IFCustomClassProperty>
    {
        public void Configure(EntityTypeBuilder<IFCustomClassProperty> builder)
        {

            builder.ToTable("IFCustomClassProperty");
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.IsNullable).HasDefaultValue(false);
            builder.Property(x => x.Type);
        }
    }
}
