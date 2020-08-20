using IF.Manager.Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Persistence.EF.Mappings
{
    public class IFPageParameterMapping : IEntityTypeConfiguration<IFPageParameter>
    {

        public void Configure(EntityTypeBuilder<IFPageParameter> builder)
        {
            //base.Configure(builder);

            builder.ToTable("IFPageParameter");            
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Type).IsRequired();
        }
    }
}