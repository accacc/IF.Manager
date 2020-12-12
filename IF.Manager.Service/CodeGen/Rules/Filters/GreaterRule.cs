using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Service.CodeGen.Rules.Filters
{
   public  class GreaterRule: EFFilterRule
    {

        public override bool IsSuccess(FilterContext context)
        {

            if (context.CurrentFilterItem.FilterOperator != Contracts.Enum.QueryFilterOperator.Greater) return true;
            if (context.CurrentFilterItem.IsNullCheck.Value)
            {
                context.FilterBuilder.AppendLine($"(x.{context.PropertyName} > {context.PropertyValue} {context.IsNullableCondition}) {context.ConditionOperator} ");

            }
            else
            {
                context.FilterBuilder.AppendLine($"x.{context.PropertyName} > {context.PropertyValue}  {context.ConditionOperator} ");
            }
            return true;
        }
    }
}
