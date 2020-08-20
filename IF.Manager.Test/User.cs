using System;
using IF.Core.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using IF.Test.Contracts;

namespace IF.Test.Contracts
{
public class User:Entity{


[Key]
public  int Id { get; set; }
public  String Name { get; set; }
public  String Email { get; set; }
public  String Password { get; set; }
public  String Surname { get; set; }
public  DateTime BirthDate { get; set; }
public  Int32 LoginCount { get; set; }
public  ICollection<Address> Addresss { get; set; }
public   User () 
{


Addresss = new List<Address>();

}



}
}

