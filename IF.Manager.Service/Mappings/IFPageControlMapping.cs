using IF.Manager.Contracts.Model;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IF.Manager.Persistence.EF.Mappings
{
    public class IFPageControlMapping : IEntityTypeConfiguration<IFPageControl>
    {
        public void Configure(EntityTypeBuilder<IFPageControl> builder)
        {
            builder.ToTable("IFPageControl");
            builder.Property(x => x.Id);
            builder.Property(x => x.ControlType);

            //one-to-one
            builder.HasOne(a => a.IFPageControlMap).WithOne(b => b.IFPageControl).HasForeignKey<IFPageControlMap>(b => b.IFPageControlId);
        }
    }
}
