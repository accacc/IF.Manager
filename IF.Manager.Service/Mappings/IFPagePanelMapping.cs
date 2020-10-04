using IF.Manager.Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Persistence.EF.Mappings
{
    public class IFPagePanelMapping : IEntityTypeConfiguration<IFPagePanel>
    {
        public void Configure(EntityTypeBuilder<IFPagePanel> builder)
        {            
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Description).IsRequired();
            
        }
    }
}
