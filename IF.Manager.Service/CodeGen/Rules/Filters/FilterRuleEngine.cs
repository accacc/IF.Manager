using IF.Core.Exception;
using IF.Core.RuleEngine;
using IF.Manager.Contracts.Enum;
using IF.Manager.Contracts.Model;

using System;
using System.Collections.Generic;
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
        public void Execute()
        {

            this.context.FilterBuilder = new StringBuilder();            

            foreach (var queryFilterItem in this.context.FilterItems)
            {

                switch (queryFilterItem.ConditionOperator)
                {
                    case QueryConditionOperator.AND:
                      this.context.ConditionOperator= "&&";
                        break;
                    case QueryConditionOperator.OR:
                        this.context.ConditionOperator = "||";
                        break;
                    default:
                        break;
                }

                context.CurrentFilterItem = queryFilterItem;


                if (!String.IsNullOrWhiteSpace(queryFilterItem.Value))
                {
                    if (queryFilterItem.Value.StartsWith("{") && queryFilterItem.Value.EndsWith("}"))
                    {
                        var formProperty = queryFilterItem.Value.Replace("{", "");
                        formProperty = formProperty.Replace("}", "");


                        //whereCon += $"x.{queryFilterItem.EntityProperty.Name} == request.Data.{formProperty}  {conditionOperator} ";

                        context.PropertyName = queryFilterItem.EntityProperty.Name;
                        context.PropertyValue = $"request.Data.{ formProperty}";
                    }
                    else
                    {
                        context.PropertyName = queryFilterItem.EntityProperty.Name;
                        context.PropertyValue = queryFilterItem.Value;
                        //whereCon += $"x.{queryFilterItem.EntityProperty.Name} == {queryFilterItem.Value} {conditionOperator} ";
                    }
                }
                else if (queryFilterItem.IFPageParameterId.HasValue)
                {
                    context.PropertyName = queryFilterItem.EntityProperty.Name;
                    context.PropertyValue = $"request.Data.{ queryFilterItem.IFPageParameter.Name}";
                    //whereCon += $"x.{queryFilterItem.EntityProperty.Name} == request.Data.{queryFilterItem.IFPageParameter.Name} && {conditionOperator}";
                }
                else
                {
                    context.PropertyName = queryFilterItem.EntityProperty.Name;
                    context.PropertyValue = $"request.Data.{ queryFilterItem.EntityProperty.Name}";
                }

                rules.Run(context);

               
            }
        }

        public string GetFilter()
        { 
            string filter = this.context.FilterBuilder.ToString();
            filter = filter.Remove(filter.Length - 6, 6);

            filter = $".Where(x=> {filter})" + Environment.NewLine;
            return filter;
        }

        //public void Execute(IFQueryFilterItem filterItem,StringBuilder filter)
        //{
        //    context.FilterItem = filterItem;
        //    context.Filter = filter;
        //    this.Execute(context);

           

        //}


    }
}
