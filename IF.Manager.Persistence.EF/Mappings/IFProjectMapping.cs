using IF.Manager.Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IF.Manager.Persistence.EF.Mappings
{
    public class IFProjectMapping : IEntityTypeConfiguration<IFProject>
    {
        public void Configure(EntityTypeBuilder<IFProject> builder)
        {
            builder.ToTable("IFProject");
            builder.Property(x => x.Id).IsRequired();
            //builder.Property(x => x.Name).IsRequired();
            //builder.Property(x => x.Nam).IsRequired();
            builder.Property(x => x.ConnectionString).IsRequired();

        }
    }
}
