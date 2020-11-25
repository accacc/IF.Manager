using IF.Core.Data;
using IF.Manager.Contracts.Enum;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IF.Manager.Service.Model
{
    public class CustomClassRelation:Entity
    {

        [Key]
        public int Id { get; set; }


       

        public int? MainCustomClassId { get; set; }

        public CustomClass MainCustomClass { get; set; }

        public int? RelatedCustomClassId { get; set; }
        public CustomClass RelatedCustomClass { get; set; }




        public ClassRelationType RelationType { get; set; }

        public string Name { get; set; }


    }
}
