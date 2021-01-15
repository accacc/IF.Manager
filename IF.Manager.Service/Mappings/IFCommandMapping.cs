using IF.Manager.Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Persistence.EF.Mappings
{

    public class IFCommandMapping : IEntityTypeConfiguration<IFCommand>
    {
        public void Configure(EntityTypeBuilder<IFCommand> builder)
        {
            builder.ToTable("IFCommand");
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Description).IsRequired();

            builder.HasOne<IFModel>(s => s.Model).WithMany(s => s.Commands).HasForeignKey(s => s.ModelId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<IFProcess>(s => s.Process).WithMany(s => s.Commands).HasForeignKey(s => s.ProcessId).OnDelete(DeleteBehavior.Restrict);


            builder
             .HasOne(x => x.Parent)
             .WithMany(x => x.Childrens)
             .HasForeignKey(x => x.ParentId)
             .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
