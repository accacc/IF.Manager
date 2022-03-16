using IF.Manager.Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Persistence.EF.Mappings
{
    

    public class IFPageGridLayoutMapping : IEntityTypeConfiguration<IFPageGridLayout>
    {
        public void Configure(EntityTypeBuilder<IFPageGridLayout> builder)
        {
            builder.ToTable("IFPageGridLayout");
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Description).IsRequired();

            builder.HasOne(s => s.Layout).WithMany(s => s.GridLayouts).HasForeignKey(s => s.LayoutId).OnDelete(DeleteBehavior.Restrict);



            builder.HasData(

                 new IFPageGridLayout { Name = "Grid Layout", Description = "Grid Layout", LayoutId =1 ,Id=1}

                );
        }
    }
}
