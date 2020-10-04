using IF.Manager.Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Persistence.EF.Mappings
{
    public class IFFormModelPropertyMapping : IEntityTypeConfiguration<IFFormModelProperty>
    {
        public void Configure(EntityTypeBuilder<IFFormModelProperty> builder)
        {
            builder.ToTable("IFFormModelProperty");
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Name).IsRequired();

            builder.HasOne<IFFormModel>(s => s.FormModel).WithMany(s => s.Properties).HasForeignKey(s => s.FormModelId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
