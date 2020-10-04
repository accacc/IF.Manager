using IF.Manager.Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IF.Manager.Persistence.EF.Mappings
{
    public class IFModelPropertyMapping : IEntityTypeConfiguration<IFModelProperty>
    {
        public void Configure(EntityTypeBuilder<IFModelProperty> builder)
        {
            builder.ToTable("IFModelProperty");
            builder.Property(x => x.Id).IsRequired();

            builder.HasOne<IFEntityProperty>(s => s.EntityProperty).WithMany(s => s.ModelProperties).HasForeignKey(s => s.EntityPropertyId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<IFEntity>(s => s.Entity).WithMany(s => s.ModelProperties).HasForeignKey(s => s.EntityId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<IFModel>(s => s.Model).WithMany(s => s.Properties).HasForeignKey(s => s.ModelId).OnDelete(DeleteBehavior.Restrict);

        }
    }
}
