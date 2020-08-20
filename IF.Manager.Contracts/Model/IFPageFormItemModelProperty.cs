using IF.Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IF.Manager.Contracts.Model
{
    public class IFPageFormItemModelProperty:Entity
    {
        [Key]
        public int Id { get; set; }

        public IFPageFormItem IFPageFormItem { get; set; }

        public int IFPageFormItemId { get; set; }

        public IFModelProperty IFModelProperty { get; set; }

        public int IFModelPropertyId { get; set; }

        public IFPageForm IFPageForm { get; set; }

        public int IFPageFormId { get; set; }


    }
}
