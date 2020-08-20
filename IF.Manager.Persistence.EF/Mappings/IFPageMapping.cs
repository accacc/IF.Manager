using IF.Manager.Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Persistence.EF.Mappings
{
    

    public class IFPageMapping : IEntityTypeConfiguration<IFPage>
    {
        public  void Configure(EntityTypeBuilder<IFPage> builder)
        {
            //base.Configure(builder);
            //builder.ToTable("IFPage");
            
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Description).IsRequired();

            builder.HasOne<IFProcess>(s => s.Process).WithMany(s => s.Pages).HasForeignKey(s => s.ProcessId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(cn => cn.IFPageParameters).WithOne(tl => tl.IFPage).HasForeignKey(tl => tl.ObjectId);
        }
    }
}
