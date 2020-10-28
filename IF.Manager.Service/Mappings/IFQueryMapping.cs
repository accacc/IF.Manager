using IF.Manager.Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Persistence.EF.Mappings
{

    public class IFQueryMapping : IEntityTypeConfiguration<IFQuery>
    {
        public void Configure(EntityTypeBuilder<IFQuery> builder)
        {
            builder.ToTable("IFQuery");
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Description).IsRequired();

            builder.HasOne(s => s.Model).WithMany(s => s.Queries).HasForeignKey(s => s.ModelId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(s => s.Process).WithMany(s => s.Queries).HasForeignKey(s => s.ProcessId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
