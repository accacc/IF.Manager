using IF.Core.RuleEngine;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Enum;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IF.Manager.Service.CodeGen.Rules.Filters
{


    public class FilterRuleEngine : IIFRuleEngine<FilterContext>
    {
        public FilterContext context { get; set; }

        EFFilterRule rules;
        public FilterRuleEngine(FilterContext context)
        {
            this.context = context;

            rules = new EqualRule();
            rules.SetNext(new NotEqualRule())
                .SetNext(new GreaterRule())
                .SetNext(new GreatAndEqualRule())
                .SetNext(new LessRule())
                .SetNext(new LessEndEqualRule())
                .SetNext(new EndsWithRule())
                .SetNext(new StartsWithRule())
                .SetNext(new NullRule())
                .SetNext(new NotEqualRule())
                .SetNext(new StringContainsRule())

            ;
        }

        public StringBuilder GetrulesQuery(List<QueryFilterTreeDto> queryFilterItems, StringBuilder builder)

        {

            for (int i = 0; i < queryFilterItems.Count; i++)
            {
                QueryFilterTreeDto queryFilterItem = queryFilterItems[i];

                switch (queryFilterItem.ConditionOperator)
                {
                    case QueryConditionOperator.AND:
                        this.context.ConditionOperator = "&&";
                        break;
                    case QueryConditionOperator.OR:
                        this.context.ConditionOperator = "||";
                        break;
                    default:
                        break;
                }

                if (i == queryFilterItems.Count - 1 && !queryFilterItem.Childs.Any())
                {
                    this.context.ConditionOperator = "";
                }

                context.CurrentFilterItem = queryFilterItem;

                if (!String.IsNullOrWhiteSpace(queryFilterItem.Value))
                {
                    if (queryFilterItem.Value.StartsWith("{") && queryFilterItem.Value.EndsWith("}"))
                    {
                        var formProperty = queryFilterItem.Value.Replace("{", "");
                        formProperty = formProperty.Replace("}", "");


                        //whereCon += $"x.{queryFilterItem.EntityProperty.Name} == request.Data.{formProperty}  {conditionOperator} ";

                        context.PropertyName = queryFilterItem.PropertyName;
                        context.PropertyValue = $"request.Data.{ formProperty}";
                    }
                    else
                    {
                        context.PropertyName = queryFilterItem.PropertyName;
                        context.PropertyValue = queryFilterItem.Value;
                        //whereCon += $"x.{queryFilterItem.EntityProperty.Name} == {queryFilterItem.Value} {conditionOperator} ";
                    }
                }
                else if (queryFilterItem.IFPageParameterId.HasValue)
                {
                    //context.PropertyName = queryFilterItem.PropertyName;
                    //context.PropertyValue = $"request.Data.{ queryFilterItem.IFPageParameter.Name}";
                    //whereCon += $"x.{queryFilterItem.EntityProperty.Name} == request.Data.{queryFilterItem.IFPageParameter.Name} && {conditionOperator}";
                }
                else
                {
                    context.PropertyName = queryFilterItem.PropertyName;
                    context.PropertyValue = $"request.Data.{ queryFilterItem.PropertyName}";
                }

                context.IsNullableCondition = $"&& {context.PropertyValue}!=null";

                rules.Run(context);

                if (queryFilterItem.Childs.Any())
                {
                    this.context.FilterBuilder.Append($"(");

                    builder = GetrulesQuery(queryFilterItem.Childs.ToList(), builder);
                    builder.Append(")");

                    //builder.Append(queryFilterItem.ConditionOperator.ToString() + " ");

                }

            }

            return builder;



        }
        public void Execute()
        {

            this.context.FilterBuilder = new StringBuilder();

            this.GetrulesQuery(this.context.FilterItems, this.context.FilterBuilder);

        }

        public string GetFilter()
        { 
            string filter = this.context.FilterBuilder.ToString();
            filter = filter.Remove(filter.Length - 6, 6);

            filter = $".Where(x=> {filter})" + Environment.NewLine;
            return filter;
        }


    }
}
