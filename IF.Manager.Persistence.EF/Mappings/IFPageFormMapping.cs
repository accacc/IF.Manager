using IF.Manager.Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Persistence.EF.Mappings
{
    

    public class IFPageFormMapping : IEntityTypeConfiguration<IFPageForm>
    {
        public void Configure(EntityTypeBuilder<IFPageForm> builder)
        {
            //base.Configure(builder);

            //builder.ToTable("IFPageGrid");            
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Description).IsRequired();

            builder.HasOne<IFPageFormLayout>(s => s.FormLayout).WithMany(s => s.PageForms).HasForeignKey(s => s.FormLayoutId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<IFModel>(s => s.IFModel).WithMany(s => s.PageForms).HasForeignKey(s => s.IFModelId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
