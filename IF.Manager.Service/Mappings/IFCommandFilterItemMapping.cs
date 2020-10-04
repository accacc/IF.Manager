using IF.Manager.Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Persistence.EF.Mappings
{


    public class IFCommandFilterItemMapping : IEntityTypeConfiguration<IFCommandFilterItem>
    {
        public void Configure(EntityTypeBuilder<IFCommandFilterItem> builder)
        {
            builder.ToTable("IFCommandFilterItem");
            builder.Property(x => x.Id).IsRequired();

            builder.HasOne<IFFormModelProperty>(s => s.FormModelProperty).WithMany(s => s.CommandFilterItems).HasForeignKey(s => s.FormModelPropertyId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<IFEntityProperty>(s => s.EntityProperty).WithMany(s => s.CommandFilterItems).HasForeignKey(s => s.EntityPropertyId).OnDelete(DeleteBehavior.Restrict);

        }
    }
}
