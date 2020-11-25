using IF.Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IF.Manager.Service.Model
{
    public class CustomClassGroup: Entity
    {

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Prefix { get; set; }

        public ICollection<CustomClass> CustomClasses { get; set; }
    }
}
