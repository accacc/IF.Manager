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
        public void GenerateCqrsHandlerClass(IFQuery query,EntityTreeDto entityTree, FileSystemCodeFormatProvider fileSystem)
        {
            CSClass queryHandlerclass = new CSClass();

            string nameSpace = SolutionHelper.GetProcessNamaspace(query.Process);

            queryHandlerclass.Name = GetDataQueryClassName(query.Name);

            queryHandlerclass.NameSpace = nameSpace;


            queryHandlerclass.Usings.Add("System.Threading.Tasks");
            queryHandlerclass.Usings.Add($"IF.Core.Persistence");
            queryHandlerclass.Usings.Add($"IF.Core.Exception");
            queryHandlerclass.Usings.Add($"System.Linq");
            queryHandlerclass.Usings.Add($"IF.Core.Data");
            queryHandlerclass.Usings.Add($"System.Collections.Generic");
            queryHandlerclass.Usings.Add($"Microsoft.EntityFrameworkCore");
            queryHandlerclass.Usings.Add($"{SolutionHelper.GetCoreNamespace(query.Process.Project)}");


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

            EFQueryMethod method = new EFQueryMethod(entityTree, query);

            queryHandlerclass.Methods.Add(method.Build());

            fileSystem.FormatCode(queryHandlerclass.GenerateCode(), "cs");

        }

        public string GetDataQueryClassName(string name)
        {
            return $"{name}QueryAsync";
        }
    }
}
