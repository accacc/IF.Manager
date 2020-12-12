using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Service.CodeGen.Rules.Filters
{
 

    public class StringContainsRule : EFFilterRule
    {

        public override bool IsSuccess(FilterContext context)
        {

            if (context.CurrentFilterItem.FilterOperator != Contracts.Enum.QueryFilterOperator.StringContains) return true;
            if (context.CurrentFilterItem.IsNullCheck.Value)
            {
                context.FilterBuilder.AppendLine($@"(x.{context.PropertyName}.Contains({context.PropertyValue}) {context.IsNullableCondition}) {context.ConditionOperator} ");

            }
            else
            {
                context.FilterBuilder.AppendLine($@"x.{context.PropertyName}.Contains({context.PropertyValue})  {context.ConditionOperator} ");
            }
            return true;
        }
    }
}
