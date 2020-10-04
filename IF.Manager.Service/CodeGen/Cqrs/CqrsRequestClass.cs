using IF.CodeGeneration.Core;
using IF.CodeGeneration.CSharp;
using IF.Manager.Contracts.Model;

namespace IF.Manager.Service.Cqrs
{
    public class CqrsRequestClass
    {
        public void GenerateRequestClass(IFQuery query,FileSystemCodeFormatProvider fileSystem)
        {

            string nameSpace = SolutionHelper.GetProcessNamaspace(query.Process);

            CSClass requestClass = new CSClass();
            requestClass.NameSpace = nameSpace;
            requestClass.BaseClass = "BaseRequest";
            requestClass.Name = $"{query.Name}Request";
            requestClass.Usings.Add($"IF.Core.Data");
            requestClass.Usings.Add(nameSpace);

            CSClass requestModelClass = new CSClass();
            requestModelClass.Name = $"{query.Name}Filter";
            requestModelClass.Usings.Add($"IF.Core.Data");
            requestModelClass.Usings.Add(nameSpace);

            foreach (var queryFilterItem in query.QueryFilterItems)
            {
                if (queryFilterItem.IFPageParameterId.HasValue)
                {
                    var property = new CSProperty("public", queryFilterItem.IFPageParameter.Name, false);

                    property.PropertyTypeString = queryFilterItem.IFPageParameter.Type;

                    requestModelClass.Properties.Add(property);
                }
                else
                {
                    var property = new CSProperty("public", queryFilterItem.EntityProperty.Name, queryFilterItem.EntityProperty.IsNullable);

                    property.PropertyTypeString = queryFilterItem.EntityProperty.Type;

                    requestModelClass.Properties.Add(property);
                }
            }



            CSProperty model = new CSProperty(null, "public", "Data", false);

            
            var  modelType = $"{query.Name}Filter";           
            model.PropertyTypeString = modelType;
            requestClass.Properties.Add(model);
            fileSystem.FormatCode(requestClass.GenerateCode(), "cs");
            fileSystem.FormatCode(requestModelClass.GenerateCode(), "cs");






        }
    }
}
