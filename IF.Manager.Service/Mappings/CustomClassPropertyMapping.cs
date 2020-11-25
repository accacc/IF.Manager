using IF.Manager.Service.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Service.Mappings
{
    public class CustomClassPropertyMapping : IEntityTypeConfiguration<CustomClassProperty>
    {
        public void Configure(EntityTypeBuilder<CustomClassProperty> builder)
        {

            builder.ToTable("CustomClassProperty");
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.IsNullable).HasDefaultValue(false);
            builder.Property(x => x.Type);
        }
    }
}
