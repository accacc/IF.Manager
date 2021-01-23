using IF.Manager.Service.Model;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Service.Mappings
{
    public class IFClassMapping : IEntityTypeConfiguration<IFClass>
    {
        public void Configure(EntityTypeBuilder<IFClass> builder)
        {
            builder.ToTable("IFClass");
            builder.Property(x => x.Id).IsRequired();
            //builder.Property(x => x.IFRelatedEntityId).IsRequired();
            builder.Property(x => x.Type).IsRequired();

            builder.HasOne(x => x.Parent)
            .WithMany(x => x.Childs)
            .HasForeignKey(x => x.ParentId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
