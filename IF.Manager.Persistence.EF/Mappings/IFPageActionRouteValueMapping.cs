using IF.Manager.Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IF.Manager.Persistence.EF.Mappings
{
    public class IFPageActionRouteValueMapping : IEntityTypeConfiguration<IFPageActionRouteValue>
    {
        public void Configure(EntityTypeBuilder<IFPageActionRouteValue> builder)
        {
            builder.ToTable("IFPageActionRouteValue");
            builder.Property(x => x.Id).IsRequired();
            
        }
    }
}
