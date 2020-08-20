using IF.Manager.Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Persistence.EF.Mappings
{
    

    public class IFFormModelMapping : IEntityTypeConfiguration<IFFormModel>
    {
        public void Configure(EntityTypeBuilder<IFFormModel> builder)
        {
            builder.ToTable("IFFormModel");
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Name).IsRequired();

            
        }
    }
}
