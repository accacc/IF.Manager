﻿using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Service.CodeGen.Rules.Filters
{
    public class GreatAndEqualRule : EFFilterRule
    {

        public override bool IsSuccess(FilterContext context)
        {
            if (context.CurrentFilterItem.FilterOperator != Contracts.Enum.QueryFilterOperator.GreaterAndEqual) return true;

            if (context.CurrentFilterItem.IsNullCheck.Value)
            {
                context.FilterBuilder.AppendLine($"(x.{context.PropertyName} >= {context.PropertyValue} {context.NullableCondition}) {context.ConditionOperator} ");

            }
            else
            {
                context.FilterBuilder.AppendLine($"x.{context.PropertyName} >= {context.PropertyValue}  {context.ConditionOperator} ");
            }
            return true;
        }
    }
}
