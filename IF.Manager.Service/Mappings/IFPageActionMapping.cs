using IF.Manager.Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Persistence.EF.Mappings
{
    

    public class IFPageActionMapping : IEntityTypeConfiguration<IFPageAction>
    {
        public void Configure(EntityTypeBuilder<IFPageAction> builder)
        {
            //builder.ToTable("IFPageGridRowAction");
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Description).IsRequired();

            builder.HasOne<IFQuery>(s => s.Query).WithMany(s => s.Actions).HasForeignKey(s => s.QueryId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<IFCommand>(s => s.Command).WithMany(s => s.Actions).HasForeignKey(s => s.CommandId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
