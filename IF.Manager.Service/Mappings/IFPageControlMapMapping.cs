﻿using IF.Manager.Contracts.Model;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IF.Manager.Persistence.EF.Mappings
{


    public class IFPageControlMapMapping : IEntityTypeConfiguration<IFPageControlMap>
    {
        public void Configure(EntityTypeBuilder<IFPageControlMap> builder)
        {
            builder.ToTable("IFPageControlMap");
            builder.Property(x => x.Id).IsRequired();

            builder
               .HasOne(x => x.Parent)
               .WithMany(x => x.Childrens)
               .HasForeignKey(x => x.ParentId)
               .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
