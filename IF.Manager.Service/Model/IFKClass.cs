using IF.Core.Data;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IF.Manager.Service.Model
{
    public class IFKClass : Entity
    {
        public IFKClass()
        {
            this.Childrens = new List<IFKClass>();
        }

        [Key]
        public int Id { get; set; }
        public int? ParentId { get; set; }

        public IFKClass Parent { get; set; }

        public ICollection<IFKClass> Childrens { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

        public string GenericType { get; set; }

        public bool IsNullable { get; set; }
        public bool IsPrimitive { get; set; }


    }
}
