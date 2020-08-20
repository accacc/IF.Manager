using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IF.Test.Contracts;

public class UserMapping : IEntityTypeConfiguration<User>{


public  void Configure (EntityTypeBuilder<User> builder) 
{


builder.ToTable("User");
builder.Property(x => x.Name);
builder.Property(x => x.Email);
builder.Property(x => x.Password);
builder.Property(x => x.Surname);
builder.Property(x => x.BirthDate);
builder.Property(x => x.LoginCount);

}



}

