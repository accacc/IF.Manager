using IF.CodeGeneration.Core;
using IF.CodeGeneration.CSharp;
using IF.Core.Exception;
using IF.Manager.Contracts.Model;

using System;

namespace IF.Manager.Service.Cqrs
{
    public class CqrsRequestClass
    {
        public void GenerateRequestClass(IFQuery query, FileSystemCodeFormatProvider fileSystem)
        {

            string nameSpace = SolutionHelper.GetProcessNamaspace(query.Process);

            CSClass requestClass = new CSClass();
            requestClass.NameSpace = nameSpace;

            switch (query.QueryGetType)
            {
                case Contracts.Enum.QueryGetType.Single:
                    requestClass.BaseClass = "BaseRequest";

                    break;
                case Contracts.Enum.QueryGetType.List:
                    requestClass.BaseClass = "BaseRequest";
                    break;
                case Contracts.Enum.QueryGetType.Page:
                    requestClass.BaseClass = "BasePagingRequest";

                    break;
                case Contracts.Enum.QueryGetType.NameValue:
                    requestClass.BaseClass = "BaseRequest";

                    break;
                default:
                    requestClass.BaseClass = "BaseRequest";

                    break;
            }


            requestClass.Name = $"{query.Name}Request";
            requestClass.Usings.Add($"IF.Core.Data");
            requestClass.Usings.Add(nameSpace);

            CSClass requestModelClass = new CSClass();
            requestModelClass.Name = $"{query.Name}Filter";
            requestModelClass.Usings.Add($"IF.Core.Data");
            requestModelClass.Usings.Add($"System");
            requestModelClass.Usings.Add(nameSpace);

            foreach (var queryFilterItem in query.QueryFilterItems)
            {
                if (!String.IsNullOrWhiteSpace(queryFilterItem.Value))
                {
                    if (queryFilterItem.Value.StartsWith("{") && queryFilterItem.Value.EndsWith("}"))
                    {
                        var formProperty = queryFilterItem.Value.Replace("{", "");
                        formProperty = formProperty.Replace("}", "");
                        var property = new CSProperty("public", formProperty, queryFilterItem.IsNullCheck.Value);

                        property.PropertyTypeString = queryFilterItem.EntityProperty.Type;

                        requestModelClass.Properties.Add(property);

                    }
                    else
                    {
                        var property = new CSProperty("public", queryFilterItem.EntityProperty.Name, queryFilterItem.IsNullCheck.Value);

                        property.PropertyTypeString = queryFilterItem.EntityProperty.Type;

                        requestModelClass.Properties.Add(property);
                    }
                }
                else if (queryFilterItem.IFPageParameterId.HasValue)
                {
                    var property = new CSProperty("public", queryFilterItem.IFPageParameter.Name, false);

                    property.PropertyTypeString = queryFilterItem.IFPageParameter.Type;

                    requestModelClass.Properties.Add(property);
                }
                else
                {
                    var property = new CSProperty("public", queryFilterItem.EntityProperty.Name, queryFilterItem.IsNullCheck.Value);

                    property.PropertyTypeString = queryFilterItem.EntityProperty.Type;

                    requestModelClass.Properties.Add(property);
                }
            }



            CSProperty model = new CSProperty(null, "public", "Data", false);


            var modelType = $"{query.Name}Filter";
            model.PropertyTypeString = modelType;
            requestClass.Properties.Add(model);
            fileSystem.FormatCode(requestClass.GenerateCode(), "cs","",query.Name);
            fileSystem.FormatCode(requestModelClass.GenerateCode(), "cs",query.Name);
        }
    }
}
