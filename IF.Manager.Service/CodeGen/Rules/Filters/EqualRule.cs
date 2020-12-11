using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Service.CodeGen.Rules.Filters
{
    public class EqualRule : EFFilterRule
    {
        public override bool IsSuccess(FilterContext context)
        {

            if (context.CurrentFilterItem.FilterOperator != Contracts.Enum.QueryFilterOperator.Equal) return true;
            if (context.CurrentFilterItem.IsNullCheck.Value)
            {
                context.FilterBuilder.AppendLine($"(x.{context.PropertyName} == {context.PropertyValue} || x.{context.PropertyName}!=null) {context.ConditionOperator} ");
                
            }
            else
            {
                context.FilterBuilder.AppendLine($"x.{context.PropertyName} == {context.PropertyValue}  {context.ConditionOperator} ");
            }
            return true;
        }
    }
}
