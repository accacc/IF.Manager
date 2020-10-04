using IF.Manager.Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Persistence.EF.Mappings
{
    

    public class IFQueryOrderMapping : IEntityTypeConfiguration<IFQueryOrder>
    {
        public void Configure(EntityTypeBuilder<IFQueryOrder> builder)
        {
            builder.ToTable("IFQueryOrder");
            builder.Property(x => x.Id).IsRequired();

            builder.HasOne<IFQuery>(s => s.Query).WithMany(s => s.QueryOrders).HasForeignKey(s => s.QueryId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<IFEntityProperty>(s => s.EntityProperty).WithMany(s => s.QueryOrders).HasForeignKey(s => s.EntityPropertyId).OnDelete(DeleteBehavior.Restrict);

        }
    }
}
