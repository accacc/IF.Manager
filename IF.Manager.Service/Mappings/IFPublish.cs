using IF.Manager.Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IF.Manager.Persistence.EF.Mappings
{
    public class IFPublishMapping : IEntityTypeConfiguration<IFPublish>
    {
        public void Configure(EntityTypeBuilder<IFPublish> builder)
        {
            builder.ToTable("IFPublish");
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Name).IsRequired();
            //builder.Property(x => x.Nam).IsRequired();
            builder.Property(x => x.Description).IsRequired();

        }
    }
}
