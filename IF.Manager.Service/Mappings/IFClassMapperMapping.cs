using IF.Manager.Service.Model;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IF.Manager.Service.Mappings
{
    public class IFClassMapperMapping : IEntityTypeConfiguration<IFClassMapper>
    {
        public void Configure(EntityTypeBuilder<IFClassMapper> builder)
        {
            builder.ToTable("IFClassMapper");
            builder.Property(x => x.Id).IsRequired();
        }
    }
}
