using IF.Core.Data;
using IF.Manager.Service.Enum;
using IF.Manager.Service.Model;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IF.Manager.Service.Model
{
    public class IFClassMapper:Entity
    {
        public IFClassMapper()
        {
        }

            [Key]
            public int Id { get; set; }


        //public IFModelProperty IFModelProperty { get; set; }

        //public IFClass IFClass { get; set; }

        //public int? ObjectId { get; set; }


        public string Name { get; set; }

        public string Description { get; set; }

        public IFClassType ToType { get; set; }
        public IFClassType FromType { get; set; }

        public int FromClassId { get; set; }

        public int ToClassId { get; set; }

    }
}
