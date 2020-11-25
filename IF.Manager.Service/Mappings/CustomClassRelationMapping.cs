using IF.Manager.Service.Model;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IF.Manager.Service.Mappings
{
    public class CustomClassRelationMapping : IEntityTypeConfiguration<CustomClassRelation>
    {
        public void Configure(EntityTypeBuilder<CustomClassRelation> builder)
        {
            builder.ToTable("CustomClassRelation");
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Name).IsRequired();
        }
    }
}
