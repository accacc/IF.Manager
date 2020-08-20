using IF.Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IF.Manager.Contracts.Model
{
    public class IFPageActionRouteValue : Entity
    {

        [Key]
        public int Id { get; set; }
        public IFPageParameter IFPageParameter { get; set; }

        public int IFPageParameterId { get; set; }

        public IFModelProperty IFModelProperty { get; set; }

        public int IFModelPropertyId { get; set; }

        public IFPageAction IFPageAction { get; set; }

        public int IFPageActionId { get; set; }
    }
}
