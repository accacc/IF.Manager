using IF.Core.Data;
using IF.Manager.Contracts.Enum;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IF.Manager.Service.Model
{
    public class IFCustomClassRelation:Entity
    {

        [Key]
        public int Id { get; set; }


       

        public int? MainIFCustomClassId { get; set; }

        public IFCustomClass MainIFCustomClass { get; set; }

        public int? RelatedIFCustomClassId { get; set; }
        public IFCustomClass RelatedIFCustomClass { get; set; }




        public ClassRelationType RelationType { get; set; }

        public string Name { get; set; }


    }
}
