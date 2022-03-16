using IF.Manager.Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Persistence.EF.Mappings
{
    

    public class IFPageFormLayoutMapping : IEntityTypeConfiguration<IFPageFormLayout>
    {
        public void Configure(EntityTypeBuilder<IFPageFormLayout> builder)
        {
            builder.ToTable("IFPageFormLayout");
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Description).IsRequired();


            builder.HasData(

            new IFPageFormLayout { Name = "Textbox", Description = "Textbox" }

           );
        }
    }
}
