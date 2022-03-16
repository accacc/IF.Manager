using IF.Manager.Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IF.Manager.Persistence.EF.Mappings
{
    public class IFPageFormItemMapping : IEntityTypeConfiguration<IFPageFormItem>
    {
        public void Configure(EntityTypeBuilder<IFPageFormItem> builder)
        {
            builder.ToTable("IFPageFormItem");
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Description).IsRequired();

            builder.HasData(

                 new IFPageFormItem { Name = "Textbox", Description = "Textbox",Id=1 },
                 new IFPageFormItem { Name = "Datepicker", Description = "Datepicker" ,Id=2},
                 new IFPageFormItem { Name = "Checkbox", Description = "Checkbox",Id=3 },
                 new IFPageFormItem { Name = "DropDown", Description = "DropDown" ,Id=4},
                 new IFPageFormItem { Name = "MultipleSelect", Description = "MultipleSelect",Id=5 }

                );
        }
    }
}
