using System;
using IF.Core.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using IF.Test.Contracts;

namespace IF.Test.Contracts
{
public class Address:Entity{


[Key]
public  int Id { get; set; }
public  String Description { get; set; }
public  String Street { get; set; }
public  String Country { get; set; }
public  String State { get; set; }
public  User User { get; set; }
public   Address () 
{


User = new User();

}



}
}

