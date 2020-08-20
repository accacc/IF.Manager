using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IF.Test.Contracts;

public class AddressMapping : IEntityTypeConfiguration<Address>{


public  void Configure (EntityTypeBuilder<Address> builder) 
{


builder.ToTable("Address");
builder.Property(x => x.Description);
builder.Property(x => x.Street);
builder.Property(x => x.Country);
builder.Property(x => x.State);

}



}

