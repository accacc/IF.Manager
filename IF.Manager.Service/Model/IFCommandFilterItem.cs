using IF.Core.Data;
using IF.Manager.Contracts.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IF.Manager.Contracts.Model
{
    public class IFCommandFilterItem:Entity
    {
        [Key]
        public int Id { get; set; }

        public QueryConditionOperator ConditionOperator { get; set; }
        public QueryFilterOperator FilterOperator { get; set; }

        public string Value { get; set; }

        public int CommandId { get; set; }

        public int EntityPropertyId { get; set; }

        public IFEntityProperty EntityProperty { get; set; }

        public IFCommand Command { get; set; }

        public int? FormModelPropertyId { get; set; }

    }
}
