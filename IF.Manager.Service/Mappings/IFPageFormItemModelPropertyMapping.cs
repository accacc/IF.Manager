using IF.Manager.Contracts.Model;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IF.Manager.Persistence.EF.Mappings
{
    public class IFPageFormItemModelPropertyMapping : IEntityTypeConfiguration<IFPageControlItemModelProperty>
    {
        public void Configure(EntityTypeBuilder<IFPageControlItemModelProperty> builder)
        {
            builder.ToTable("IFPageFormItemModelProperty");
            builder.Property(x => x.Id).IsRequired();
            //builder.HasMany(cn => cn.IFPageControlItemModelProperties).WithOne(tl => tl.IFPageFormItemModelProperty).HasForeignKey(tl => tl.ObjectId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
