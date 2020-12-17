using IF.Core.Data;
using IF.Manager.Contracts.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IF.Manager.Contracts.Model
{
    public class IFQueryFilterItem:Entity
    {

        public IFQueryFilterItem()
        {
            this.Childrens = new List<IFQueryFilterItem>();
        }

        [Key]
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public IFQueryFilterItem Parent { get; set; }

        public ICollection<IFQueryFilterItem> Childrens { get; set; }
        public QueryConditionOperator ConditionOperator { get; set; }
        public QueryFilterOperator FilterOperator { get; set; }

        public string Value { get; set; }

        public int QueryId { get; set; }

        public int EntityPropertyId { get; set; }

        public IFEntityProperty EntityProperty { get; set; }

        public IFQuery Query { get; set; }

        public int? FormModelPropertyId { get; set; }

        public int? IFPageParameterId { get; set; }

        public IFPageParameter IFPageParameter { get; set; }

        public bool? IsNullCheck { get; set; }


    }
}
