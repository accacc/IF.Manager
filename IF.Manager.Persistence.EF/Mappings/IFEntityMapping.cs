using IF.Manager.Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IF.Manager.Persistence.EF.Mappings
{
    public class IFEntityMapping : IEntityTypeConfiguration<IFEntity>
    {
        public void Configure(EntityTypeBuilder<IFEntity> builder)
        {
            builder.ToTable("IFEntity");
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Description).IsRequired();
        }
    }
}
