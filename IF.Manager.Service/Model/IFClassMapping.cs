using IF.Core.Data;
using IF.Manager.Service.Enum;
using IF.Manager.Service.Model;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IF.Manager.Service.Model
{
    public class IFClassMapping: Entity
    {
        [Key]
        public int Id { get; set; }


        public IFClassMapper IFClassMapper { get; set; }

        public int IFClassMapperId { get; set; }
        public int FromPropertyId { get; set; }

        public int ToPropertyId { get; set; }

        public IFClass FromProperty { get; set; }

        public IFClass ToProperty { get; set; }

        public bool IsList { get; set; }


    }
}
