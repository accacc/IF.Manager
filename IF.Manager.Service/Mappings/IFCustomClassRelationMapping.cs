using IF.Manager.Service.Model;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Service.Mappings
{
    public class IFCustomClassRelationMapping : IEntityTypeConfiguration<IFCustomClassRelation>
    {
        public void Configure(EntityTypeBuilder<IFCustomClassRelation> builder)
        {
            builder.ToTable("IFCustomClassRelation");
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Name).IsRequired();
        }
    }
}
