using IF.Manager.Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Persistence.EF.Mappings
{
    

    public class IFEntityRelationMapping : IEntityTypeConfiguration<IFEntityRelation>
    {
        public void Configure(EntityTypeBuilder<IFEntityRelation> builder)
        {
            builder.ToTable("IFEntityRelation");
            builder.Property(x => x.Id).IsRequired();
            //builder.Property(x => x.IFRelatedEntityId).IsRequired();
            builder.Property(x => x.Type).IsRequired();

        }
    }
}
