using IF.CodeGeneration.CSharp;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Service.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IF.Manager.Service
{


    public class EFQueryMethod : EFMethod
    {

        ClassTreeDto entityTree;
        IFQuery query;
        CSMethod method;

        public EFQueryMethod(ClassTreeDto entityTree, IFQuery query)
        {
            this.entityTree = entityTree;
            this.query = query;

            var methodName = $"GetQuery";

            var returnType = $"List<{query.Model.Name}>";

            if (this.query.QueryGetType == Contracts.Enum.QueryGetType.Single)
            {
                returnType = $"{query.Model.Name}";
            }

            this.method = new CSMethod(methodName, returnType, "public");
            this.method.IsAsync = true;

            var parameter = new CsMethodParameter();
            parameter.Name = "request";
            parameter.Type = this.query.Name + "Request";

            this.method.Parameters.Add(parameter);
        }


        public CSMethod Build()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"var results = await this.repository.GetQuery<{entityTree.Name}>()");

            AddRelations(builder);
            AddFilters(builder);
            AddProjections(builder);

            if (query.QueryGetType == Contracts.Enum.QueryGetType.Single)
            {
                builder.AppendLine($".SingleOrDefaultAsync();" + Environment.NewLine);
            }
            else
            {
                builder.AppendLine($".ToListAsync();" + Environment.NewLine);
            }

            builder.AppendLine($"return results;" + Environment.NewLine);

            this.method.Body = builder.ToString();

            return this.method;
        }

        private void AddFilters(StringBuilder builder)
        {
            if (!this.query.QueryFilterItems.Any()) return;

            string whereCon = String.Empty;

            foreach (var queryFilterItem in this.query.QueryFilterItems)
            {
                if (!String.IsNullOrWhiteSpace(queryFilterItem.Value))
                {
                    //if (queryFilterItem.Value.StartsWith("{") && queryFilterItem.Value.EndsWith("}"))
                    //{
                    //    var formProperty = queryFilterItem.Value.Replace("{", "");
                    //    formProperty = formProperty.Replace("}", "");
                    //    whereCon += $"x.{formProperty} == request.Data.{formProperty}";
                    //}
                    //else
                    //{
                    whereCon += $"x.{queryFilterItem.EntityProperty.Name} == {queryFilterItem.Value} && ";
                    ///}
                }
                else if (queryFilterItem.IFPageParameterId.HasValue)
                {
                    whereCon += $"x.{queryFilterItem.EntityProperty.Name} == request.Data.{queryFilterItem.IFPageParameter.Name} && ";
                }
            }

            whereCon = whereCon.Remove(whereCon.Length - 3, 3);


            builder.AppendLine($".Where(x=> {whereCon})" + Environment.NewLine);
        }

        private void AddProjections(StringBuilder builder)
        {
            builder.AppendLine($".Select(x => new {this.query.Model.Name}" + Environment.NewLine);

            builder.AppendLine($"{{" + Environment.NewLine);

            foreach (var property in this.entityTree.Childs)
            {
                bool IsModelProperty = ClassTreeDto.IsModelProperty(property, query.Model);

                if (!IsModelProperty) continue;

                if (property.IsRelation)
                {
                    builder.AppendLine($@"{property.Name} = new {property.Name}{{" + Environment.NewLine);

                    foreach (var childProperty in property.Childs)
                    {
                        IsModelProperty = ClassTreeDto.IsModelProperty(childProperty, query.Model);

                        if (!IsModelProperty) continue;

                        //TODO: simdilik two level relation var.bunu recursive yap

                        if (!childProperty.IsRelation)
                        {

                            builder.AppendLine($"{childProperty.Name} = x.{property.Name}.{childProperty.Name},");
                        }
                    }

                    builder.AppendLine("},");
                }
                else
                {
                    builder.AppendLine($"{property.Name} = x.{property.Name}," + Environment.NewLine);
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

                bool IsModelProperty = ClassTreeDto.IsModelProperty(relation, query.Model);

                if (!IsModelProperty) continue;

                builder.AppendLine($".Include(e => e.{relation.Name})");
            }

        }
    }
}
