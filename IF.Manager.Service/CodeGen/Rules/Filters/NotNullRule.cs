using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Service.CodeGen.Rules.Filters
{
    public class NotNullRule : EFFilterRule
    {
        public override bool IsSuccess(FilterContext context)
        {
            if (context.CurrentFilterItem.FilterOperator != Contracts.Enum.QueryFilterOperator.NotNull) return true;
            context.FilterBuilder.AppendLine($"x.{context.PropertyName} != null  {context.ConditionOperator} ");
            return true;
        }
    }
}
