using IF.Manager.Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IF.Manager.Persistence.EF.Mappings
{
    public class IFModelMapping : IEntityTypeConfiguration<IFModel>
    {
        public void Configure(EntityTypeBuilder<IFModel> builder)
        {
            builder.ToTable("IFModel");
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Name).IsRequired();

            //builder.HasOne<IFEntity>(s => s.Entity).WithMany(s => s.Models).HasForeignKey(s => s.EntityId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
