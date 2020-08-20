
using IF.Test.Contracts;
using Microsoft.EntityFrameworkCore;

public class TestDbContext:DbContext{


public  DbSet<User> User { get; set; }
public  DbSet<Address> Address { get; set; }
public  void TestDbContext (DbContextOptions<TestDbContext> options): base(options)
{



}

public override void OnModelCreating (ModelBuilder builder) 
{


builder.ApplyConfiguration(new UserMapping());
builder.ApplyConfiguration(new AddressMapping());

}



}

