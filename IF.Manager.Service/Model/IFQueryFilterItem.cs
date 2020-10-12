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
        [Key]
        public int Id { get; set; }

        public QueryConditionOperator ConditionOperator { get; set; }
        public QueryFilterOperator FilterOperator { get; set; }

        public string Value { get; set; }

        public int QueryId { get; set; }

        //public int EntityId { get; set; }

        public int EntityPropertyId { get; set; }

        //public IFEntity Entity { get; set; }

        public IFEntityProperty EntityProperty { get; set; }

        public IFQuery Query { get; set; }

        //public int FormModelId { get; set; }

        public int? FormModelPropertyId { get; set; }

        public int? IFPageParameterId { get; set; }

        public IFPageParameter IFPageParameter { get; set; }

        //public IFFormModel FormModel { get; set; }

    }
}
