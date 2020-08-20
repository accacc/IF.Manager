using IF.Manager.Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Persistence.EF.Mappings
{
    

    public class IFPageControlMapMapping : IEntityTypeConfiguration<IFPageControlMap>
    {
        public void Configure(EntityTypeBuilder<IFPageControlMap> builder)
        {
            builder.ToTable("IFPageControlMap");
            builder.Property(x => x.Id).IsRequired();
            //builder.Property(x => x.Name).IsRequired();
            //builder.Property(x => x.Nam).IsRequired();
            //builder.Property(x => x.ConnectionString).IsRequired();

            builder
               .HasOne(x => x.Parent)
               .WithMany(x => x.Childrens)
               .HasForeignKey(x => x.ParentId)
               .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
