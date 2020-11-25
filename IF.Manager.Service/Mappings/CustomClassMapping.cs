using IF.Manager.Service.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Service.Mappings
{
   public  class CustomClassMapping: IEntityTypeConfiguration<CustomClass>
    {

        public void Configure(EntityTypeBuilder<CustomClass> builder)
        {
            builder.ToTable("CustomClass");
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Description).IsRequired();
        }
    }
}
