using IF.Manager.Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IF.Manager.Persistence.EF.Mappings
{
    public class IFPageLayoutMapping : IEntityTypeConfiguration<IFPageLayout>
    {
        public void Configure(EntityTypeBuilder<IFPageLayout> builder)
        {
            builder.ToTable("IFPageLayout");
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Description).IsRequired();

            builder.HasData(

               new IFPageLayout { Name = "Two Column", Description = "Two Column", ColumSize = 2 }

              );
        }
    }
}
