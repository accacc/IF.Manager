using IF.Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IF.Manager.Service.Model
{
    public class CustomClass:Entity
    {

        public CustomClass()
        {
            //this.CustomClassRelation = new List<CustomClassRelation>();
            this.CustomClassProperties = new List<CustomClassProperty>();
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int? CustomClassGroupId { get; set; }

        public bool IsAudited { get; set; }

        public CustomClassGroup Group { get; set; }

        public ICollection<CustomClassProperty> CustomClassProperties { get; set; }

        public ICollection<CustomClassRelation> Relations { get; set; }

        public ICollection<CustomClassRelation> ReverseRelations { get; set; }

        //public ICollection<CustomClassRelation> CustomClassRelation { get; set; }

    }
}
