using IF.CodeGeneration.Core;
using IF.CodeGeneration.CSharp;
using IF.Manager.Contracts.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Service.Cqrs
{
    public class CqrsResponseClass
    {
        public  void GenerateResponseClass(IFQuery query, FileSystemCodeFormatProvider fileSystem)
        {
            string nameSpace = SolutionHelper.GetProcessNamaspace(query.Process);
            CSClass responseClass = new CSClass();
            responseClass.Usings.Add($"IF.Core.Data");
            responseClass.Usings.Add($"System.Collections.Generic");
            responseClass.Usings.Add(nameSpace);

            responseClass.BaseClass = "BaseResponse";
            responseClass.Name = $"{query.Name}Response";
            CSProperty model = new CSProperty(null, "public", "Data", false);

            var modelType = $"List<{query.Model.Name}>";

            if (query.QueryGetType == Contracts.Enum.QueryGetType.Single)
            {
                modelType = $"{query.Model.Name}";
            }

            model.PropertyTypeString = modelType;
            responseClass.Properties.Add(model);



            fileSystem.FormatCode(responseClass.GenerateCode(), "cs");
        }
    }
}
