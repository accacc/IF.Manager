using IF.Manager.Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Persistence.EF.Mappings
{


    public class IFQueryFilterItemMapping : IEntityTypeConfiguration<IFQueryFilterItem>
    {
        public void Configure(EntityTypeBuilder<IFQueryFilterItem> builder)
        {
            builder.ToTable("IFQueryFilterItem");
            builder.Property(x => x.Id).IsRequired();

            builder.HasOne<IFEntityProperty>(s => s.EntityProperty).WithMany(s => s.QueryFilterItems).HasForeignKey(s => s.EntityPropertyId).OnDelete(DeleteBehavior.Restrict);

        }
    }
}
