using IF.Manager.Service.Model;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Service.Mappings
{
    public class IFKClassMapping : IEntityTypeConfiguration<IFKClass>
    {
        public void Configure(EntityTypeBuilder<IFKClass> builder)
        {
            builder.ToTable("IFKClass");
            builder.Property(x => x.Id).IsRequired();
            //builder.Property(x => x.IFRelatedEntityId).IsRequired();
            builder.Property(x => x.Type).IsRequired();

        }
    }
}
