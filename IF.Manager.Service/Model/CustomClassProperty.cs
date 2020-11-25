using IF.Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IF.Manager.Service.Model
{
    public class CustomClassProperty : Entity
    {

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }


        public CustomClass CustomClass { get; set; }

        public int CustomClassId { get; set; }

        public string Type { get; set; }

        public bool IsNullable { get; set; }
    
    }
}
