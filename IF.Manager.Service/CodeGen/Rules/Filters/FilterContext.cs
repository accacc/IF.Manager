using IF.Core.RuleEngine;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;

using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Service.CodeGen.Rules.Filters
{

    public class FilterContext : IIFRuleContext
    {
        public List<QueryFilterTreeDto> FilterItems;
        public QueryFilterTreeDto CurrentFilterItem { get; set; }
        
        public StringBuilder FilterBuilder { get; set; }

        public string NullableCondition { get; set; }
        public string PropertyName { get; set; }

        public string PropertyValue { get; set; }

        public string ConditionOperator { get; set; }
    }
}
