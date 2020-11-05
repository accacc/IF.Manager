using IF.Manager.Service.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Service.Mappings
{
   public  class IFCustomClassMapping: IEntityTypeConfiguration<IFCustomClass>
    {

        public void Configure(EntityTypeBuilder<IFCustomClass> builder)
        {
            builder.ToTable("IFCustomClass");
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Description).IsRequired();
        }
    }
}
