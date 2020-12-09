using IF.CodeGeneration.Core;
using IF.CodeGeneration.CSharp;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Service.Cqrs
{
    public class CqrsQueryHandlerGenerator
    {
        public void GenerateCqrsHandlerClass(IFQuery query,ClassTreeDto entityTree, FileSystemCodeFormatProvider fileSystem)
        {
            CSClass queryHandlerclass = new CSClass();

            if (query.IsQueryOverride) queryHandlerclass.IsPartial = true;

            string nameSpace = SolutionHelper.GetProcessNamaspace(query.Process);

            queryHandlerclass.Name = GetDataQueryClassName(query.Name);

            queryHandlerclass.NameSpace = nameSpace;

            AddNameSpaces(query, queryHandlerclass);

            queryHandlerclass.InheritedInterfaces.Add($"IQueryHandlerAsync<{query.Name}Request, {query.Name}Response>");

            var repositoryProperty = new CSProperty("private", "repository", false);
            repositoryProperty.PropertyTypeString = "IRepository";
            repositoryProperty.IsReadOnly = true;
            queryHandlerclass.Properties.Add(repositoryProperty);


            CSMethod constructorMethod = new CSMethod(queryHandlerclass.Name, "", "public");
            constructorMethod.Parameters.Add(new CsMethodParameter() { Name = "repository", Type = "IRepository" });
            StringBuilder methodBody = new StringBuilder();
            methodBody.AppendFormat("this.repository = repository;");
            methodBody.AppendLine();
            constructorMethod.Body = methodBody.ToString();
            queryHandlerclass.Methods.Add(constructorMethod);


            CSMethod handleMethod = new CSMethod("HandleAsync", query.Name + "Response", "public");
            handleMethod.IsAsync = true;
            handleMethod.Parameters.Add(new CsMethodParameter() { Name = "request", Type = query.Name + "Request" });

            handleMethod.Body += $"{query.Name}Response response = new {query.Name}Response();" + Environment.NewLine;
            handleMethod.Body += $"response.Data =   await this.GetQuery(request);" + Environment.NewLine;
            handleMethod.Body += $"return response;" + Environment.NewLine;

            queryHandlerclass.Methods.Add(handleMethod);

            if (!query.IsQueryOverride)
            {
                EFQueryMethod method = new EFQueryMethod(entityTree, query);

                queryHandlerclass.Methods.Add(method.BuildGeneratedQuery());
            }
            else
            {
                CSClass overClass = new CSClass();
                overClass.Name = queryHandlerclass.Name;
                overClass.IsPartial = true;
                overClass.NameSpace = queryHandlerclass.NameSpace;
                AddNameSpaces(query, overClass);

                EFQueryMethod overMethod = new EFQueryMethod(entityTree, query);

                overClass.Methods.Add(overMethod.BuildOverridenQuery());

                fileSystem.FormatCode(overClass.GenerateCode().Template, "cs", queryHandlerclass.Name + "Override");


            }

            fileSystem.FormatCode(queryHandlerclass.GenerateCode(), "cs");

        }

        private static void AddNameSpaces(IFQuery query, CSClass queryHandlerclass)
        {
            queryHandlerclass.Usings.Add("System.Threading.Tasks");
            queryHandlerclass.Usings.Add($"IF.Core.Persistence");
            queryHandlerclass.Usings.Add($"IF.Core.Exception");
            queryHandlerclass.Usings.Add($"System.Linq");
            queryHandlerclass.Usings.Add($"IF.Core.Data");
            queryHandlerclass.Usings.Add($"System.Collections.Generic");
            queryHandlerclass.Usings.Add($"Microsoft.EntityFrameworkCore");
            queryHandlerclass.Usings.Add($"System");
            queryHandlerclass.Usings.Add($"{SolutionHelper.GetCoreNamespace(query.Process.Project)}");
        }

        public string GetDataQueryClassName(string name)
        {
            return $"{name}QueryAsync";
        }
    }
}
