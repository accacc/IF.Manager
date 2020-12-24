using IF.Manager.Service.Model;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Service.Mappings
{
    public class IFClassMappingMapping : IEntityTypeConfiguration<IF.Manager.Service.Model.IFClassMapping>
    {
        public void Configure(EntityTypeBuilder<IF.Manager.Service.Model.IFClassMapping> builder)
        {
            builder.ToTable("IFClassMapping");
            builder.Property(x => x.Id).IsRequired();
        }
    }
}
