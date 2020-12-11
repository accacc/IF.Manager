using IF.Core.RuleEngine;
using IF.Manager.Contracts.Model;

using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Service.CodeGen.Rules.Filters
{

    public class FilterContext : IIFRuleContext
    {
        public List<IFQueryFilterItem> FilterItems;
        public IFQueryFilterItem CurrentFilterItem { get; set; }
        
        public StringBuilder FilterBuilder { get; set; }

        public string PropertyName { get; set; }

        public string PropertyValue { get; set; }

        public string ConditionOperator { get; set; }
    }
}
