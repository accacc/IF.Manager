﻿using IF.Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IF.Manager.Contracts.Model
{
    public class IFPageControlItemModelProperty:Entity, IMoveable
    {
        public IFPageControlItemModelProperty()
        {
        }

        [Key]
        public int Id { get; set; }

        public int Sequence { get; set; }

        public IFPageFormItem IFPageFormItem { get; set; }

        public int IFPageFormItemId { get; set; }

        public IFModelProperty IFModelProperty { get; set; }

        public int IFModelPropertyId { get; set; }

        public IFPageForm IFPageForm { get; set; }

        public IFPageGrid IFPageGrid { get; set; }

        public int? ObjectId { get; set; }


        public IFQuery IFQuery { get; set; }

        public int? IFQueryId { get; set; }

        public int? NameIFModelPropertyId { get; set; }

        public IFModelProperty NameIFModelProperty { get; set; }

        public int? ValueIFModelPropertyId { get; set; }

        public IFModelProperty ValueIFModelProperty { get; set; }


    }
}
