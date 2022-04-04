using IF.CodeGeneration.Language.CSharp;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Service.CodeGen;
using IF.Manager.Service.CodeGen.Rules.Filters;
using IF.Manager.Service.EF;

using System;
using System.Linq;
using System.Text;

namespace IF.Manager.Service
{


    public class EFQueryMethod : EFMethod
    {

        ModelClassTreeDto entityTree;
        IFQuery query;
        CSMethod method;

        public EFQueryMethod(ModelClassTreeDto entityTree, IFQuery query)
        {
            this.entityTree = entityTree;
            this.query = query;

            var methodName = $"GetQuery";

            string returnType = $"IQueryable<{this.query.Model.Name}>";

            this.method = new CSMethod(methodName, returnType, "public");

            var requestParameter = new CsMethodParameter();
            requestParameter.Name = "request";
            requestParameter.Type = this.query.Name + "Request";
            this.method.Parameters.Add(requestParameter);

            this.method.Parameters.Add(new CsMethodParameter() { Name = "repository", Type = "IRepository" });
        }

      
        public CSMethod BuildOverridenQuery()
        {

            StringBuilder builder = new StringBuilder();

            

            builder.AppendLine("throw new NotImplementedException();");

            this.method.Body = builder.ToString();

            return this.method;

        }
        public CSMethod BuildGeneratedQuery()
        {
            StringBuilder queryStringBuilder = new StringBuilder();

            queryStringBuilder.AppendLine($"var queryable = this.repository.GetQuery<{entityTree.Name}>()");

            AddRelations(queryStringBuilder);
            AddFilters(queryStringBuilder);
            AddProjections(queryStringBuilder);

            queryStringBuilder.Append(";");

            queryStringBuilder.AppendLine("return queryable;");


            this.method.Body = queryStringBuilder.ToString();

            return this.method;
        }

        private void AddFilters(StringBuilder builder)
        {


            if (!this.query.QueryFilterItems.Any()) return;
            FilterContext filterContext = new FilterContext();
            filterContext.FilterItems = this.query.QueryFilterItems.Select

               (map =>
                new QueryFilterTreeDto
                {

                    ConditionOperator = map.ConditionOperator,
                    Id = map.Id,
                    ParentId = map.ParentId,
                    FilterOperator = map.FilterOperator,
                    IsNullCheck = map.IsNullCheck,
                    EntityPropertyId = map.EntityPropertyId,
                    Value = map.Value,
                    QueryId = map.QueryId,
                    PropertyName = map.EntityProperty.Name

                })

                .ToList();
            FilterRuleEngine filterRuleEngine = new FilterRuleEngine(filterContext);
            filterRuleEngine.Execute();

            builder.AppendLine(filterRuleEngine.GetFilter());


        }

      

        private void AddProjections(StringBuilder builder)
        {
            
            builder.AppendLine($".Select(x => new {this.query.Model.Name}" + Environment.NewLine);

            builder.AppendLine($"{{" + Environment.NewLine);

            foreach (var childTree in this.entityTree.Childs)
            {
                bool IsModelProperty = ModelClassTreeDto.IsModelProperty(childTree, query.Model);

                if (!IsModelProperty) continue;

                if (childTree.IsRelation)
                {

                    string name = ObjectNamerHelper.AddAsLastWord(childTree.Name, "DataModel");

                    if (childTree.IsList)
                    {
                        builder.AppendLine($@"{name} = x.{childTree.Name}s.Select(a=> new {name}{{" + Environment.NewLine);
                    }
                    else
                    {
                        builder.AppendLine($@"{name} = new {name}{{" + Environment.NewLine);
                    }


                    foreach (var childProperty in childTree.Childs)
                    {
                        IsModelProperty = ModelClassTreeDto.IsModelProperty(childProperty, query.Model);

                        if (!IsModelProperty) continue;

                        //TODO: simdilik two level relation var.bunu recursive yap

                        if (!childProperty.IsRelation)
                        {
                            if (childTree.IsList)
                            {
                                builder.AppendLine($"{childProperty.Name} = a.{childProperty.Name},");

                            }
                            else
                            {
                                builder.AppendLine($"{childProperty.Name} = x.{childTree.Name}.{childProperty.Name},"); }
                        }
                    }

                    if (childTree.IsList)
                    {
                        builder.AppendLine("})");
                    }
                    else
                    {

                        builder.AppendLine("},");
                    }
                }
                else
                {
                    builder.AppendLine($"{childTree.Name} = x.{childTree.Name}," + Environment.NewLine);
                }


            }

            if (query.QueryGetType == Contracts.Enum.QueryGetType.Single)
            {
                builder.AppendLine($"}})" + Environment.NewLine);
            }
            else
            {
                builder.AppendLine($"}})" + Environment.NewLine);
            }
        }

        private void AddRelations(StringBuilder builder)
        {
            var relations = entityTree.Childs.Where(c => c.IsRelation).ToList();


            foreach (var relation in relations)
            {

                bool IsModelProperty = ModelClassTreeDto.IsModelProperty(relation, query.Model);

                if (!IsModelProperty) continue;

                string relationName = relation.Name;

                if(relation.IsList)
                {
                    relationName = relationName + "s";
                }

                builder.AppendLine($".Include(e => e.{relationName})");
            }

        }
    }
}
