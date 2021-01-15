//using IF.Manager.Service.Model;

//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace IF.Manager.Persistence.EF.Mappings
//{


//    public class IFCommandMultiMapping : IEntityTypeConfiguration<IFCommandMulti>
//    {
//        public void Configure(EntityTypeBuilder<IFCommandMulti> builder)
//        {
//            builder.ToTable("IFCommandMulti");
//            builder.Property(x => x.Id).IsRequired();

//            //builder
//            //   .HasOne(x => x.Parent)
//            //   .WithMany(x => x.Childrens)
//            //   .HasForeignKey(x => x.ParentId)
//            //   .OnDelete(DeleteBehavior.Restrict);

//        }
//    }
//}
