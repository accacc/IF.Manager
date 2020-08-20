using IF.Manager.Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Persistence.EF.Mappings
{
    

    public class IFPageGridMapping : IEntityTypeConfiguration<IFPageGrid>
    {
        public void Configure(EntityTypeBuilder<IFPageGrid> builder)
        {
            //base.Configure(builder);

            //builder.ToTable("IFPageGrid");            
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Description).IsRequired();

            builder.HasOne<IFQuery>(s => s.Query).WithMany(s => s.Grids).HasForeignKey(s => s.QueryId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<IFPageGridLayout>(s => s.GridLayout).WithMany(s => s.PageGrids).HasForeignKey(s => s.GridLayoutId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
