using IF.Manager.Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IF.Manager.Persistence.EF.Mappings
{
    public class IFSolutionMapping : IEntityTypeConfiguration<IFSolution>
    {
        public void Configure(EntityTypeBuilder<IFSolution> builder)
        {
            builder.ToTable("IFSolution");
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.SolutionName).IsRequired();
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.Path).IsRequired();

        }
    }
}
