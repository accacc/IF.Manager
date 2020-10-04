using IF.Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IF.Manager.Contracts.Model
{
    public class IFPageFormItem: Entity
    {

        public IFPageFormItem()
        {
            this.ModelProperties = new List<IFPageFormItemModelProperty>();
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<IFPageFormItemModelProperty> ModelProperties { get; set; }
    }
}
