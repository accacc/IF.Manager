using IF.Manager.Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IF.Manager.Persistence.EF.Mappings
{
    public class IFProcessMapping : IEntityTypeConfiguration<IFProcess>
    {
        public void Configure(EntityTypeBuilder<IFProcess> builder)
        {
            builder.ToTable("IFProcess");
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Name).IsRequired();
            //builder.Property(x => x.Nam).IsRequired();
            builder.Property(x => x.Description).IsRequired();

        }
    }
}
