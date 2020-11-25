using IF.Manager.Contracts.Model;
using IF.Manager.Service.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IF.Manager.Persistence.EF.Mappings
{
    public class CustomClassGroupMapping : IEntityTypeConfiguration<CustomClassGroup>
    {
        public void Configure(EntityTypeBuilder<CustomClassGroup> builder)
        {
            builder.ToTable("CustomClassGroup");
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Prefix);

        }
    }
}
