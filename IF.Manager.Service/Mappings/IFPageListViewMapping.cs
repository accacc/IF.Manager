using IF.Manager.Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Persistence.EF.Mappings
{
    

    public class IFPageListViewMapping : IEntityTypeConfiguration<IFPageListView>
    {
        public void Configure(EntityTypeBuilder<IFPageListView> builder)
        {            
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Description).IsRequired();

            builder.HasMany(cn => cn.IFPageParameters).WithOne(tl => tl.IFPageListView).HasForeignKey(tl => tl.ObjectId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
