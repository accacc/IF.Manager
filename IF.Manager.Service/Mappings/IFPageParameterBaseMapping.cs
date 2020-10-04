using IF.Manager.Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Persistence.EF.Mappings
{
    

    public class IFPageParameterBaseMapping : IEntityTypeConfiguration<IFPageParameter>
    {
        public void Configure(EntityTypeBuilder<IFPageParameter> builder)
        {
            builder.ToTable("IFPageParameter");
            builder.Property(x => x.Id).IsRequired();

            //builder.HasOne<IFPage>(s => s.IFPage).WithMany(s => s.IFPageParameters).HasForeignKey(s => s.IFPageId).OnDelete(DeleteBehavior.Restrict);
            

        }
    }
}
