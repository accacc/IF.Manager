using IF.Manager.Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IF.Manager.Persistence.EF.Mappings
{
    public class IFEntityGroupMapping : IEntityTypeConfiguration<IFEntityGroup>
    {
        public void Configure(EntityTypeBuilder<IFEntityGroup> builder)
        {
            builder.ToTable("IFEntityGroup");
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Prefix);

        }
    }
}
