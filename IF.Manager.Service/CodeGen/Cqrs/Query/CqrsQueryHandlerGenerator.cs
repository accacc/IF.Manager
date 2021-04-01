using IF.CodeGeneration.Core;
using IF.CodeGeneration.CSharp;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;

using System;
using System.Text;

namespace IF.Manager.Service.Cqrs
{
    public class CqrsQueryHandlerGenerator
    {
        public void GenerateCqrsHandlerClass(IFQuery query, ModelClassTreeDto entityTree, FileSystemCodeFormatProvider fileSystem)
        {
            CSClass queryHandlerclass = new CSClass();

            if (query.IsQueryOverride) queryHandlerclass.IsPartial = true;

            string nameSpace = SolutionHelper.GetProcessNamaspace(query.Process);

            queryHandlerclass.Name = $"{query.Name}QueryAsync";

            queryHandlerclass.NameSpace = nameSpace;

            AddNameSpaces(query, queryHandlerclass);

            if (query.QueryGetType == Contracts.Enum.QueryGetType.Page)
            {
                queryHandlerclass.InheritedInterfaces.Add($"IQueryHandlerAsync<{query.Name}Request, PagedListResponse<{query.Model.Name}>>");
            }
            else
            {
                queryHandlerclass.InheritedInterfaces.Add($"IQueryHandlerAsync<{query.Name}Request, {query.Name}Response>");
            }

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

            CSMethod handleMethod;

            if (query.QueryGetType == Contracts.Enum.QueryGetType.Page)
            {
                handleMethod = new CSMethod("HandleAsync", $"PagedListResponse<{query.Model.Name}>", "public");

            }
            else
            {
                handleMethod = new CSMethod("HandleAsync", query.Name + "Response", "public");
            }


            handleMethod.IsAsync = true;
            handleMethod.Parameters.Add(new CsMethodParameter() { Name = "request", Type = query.Name + "Request" });

            StringBuilder handleMethodBuilder = new StringBuilder();

            if (query.QueryGetType != Contracts.Enum.QueryGetType.Page)
            {
                handleMethodBuilder.AppendLine($"{query.Name}Response response = new {query.Name}Response();");
            }

            handleMethodBuilder.AppendLine($"var query  =   this.GetQuery(request,repository);");

            if (query.QueryGetType == Contracts.Enum.QueryGetType.Single)
            {
                handleMethodBuilder.AppendLine($"var results = await query.SingleOrDefaultAsync();");
            }
            else if (query.QueryGetType == Contracts.Enum.QueryGetType.Page)
            {
                handleMethodBuilder.AppendLine($"request.PageSize={query.PageSize};");
                handleMethodBuilder.AppendLine();
                handleMethodBuilder.AppendLine($"request.PageNumber={query.PageNumber};");
                handleMethodBuilder.AppendLine();

                handleMethodBuilder.AppendLine($"var response = await this.repository.ToPagedListResponseAsync(query, request);");
            }
            else if (query.QueryGetType == Contracts.Enum.QueryGetType.List || query.QueryGetType == Contracts.Enum.QueryGetType.NameValue)
            {
                handleMethodBuilder.AppendLine($"var results = query.ToListAsync();");
            }
            else
            {
                throw new ApplicationException("unknown QueryGetType");
            }


            if (query.QueryGetType != Contracts.Enum.QueryGetType.Page)
            {
                handleMethodBuilder.AppendLine($"response.Data = results;");
            }

            handleMethodBuilder.AppendLine($"return response;");

            handleMethod.Body = handleMethodBuilder.ToString();

            queryHandlerclass.Methods.Add(handleMethod);

            if (!query.IsQueryOverride)
            {
                EFQueryMethod method = new EFQueryMethod(entityTree, query);
                queryHandlerclass.Methods.Add(method.BuildGeneratedQuery());
            }
            else
            {
                CSClass queryOverrideClass = new CSClass();
                queryOverrideClass.Name = queryHandlerclass.Name;
                queryOverrideClass.IsPartial = true;
                queryOverrideClass.NameSpace = queryHandlerclass.NameSpace;
                AddNameSpaces(query, queryOverrideClass);

                EFQueryMethod queryOverrideMethod = new EFQueryMethod(entityTree, query);
                

                queryOverrideClass.Methods.Add(queryOverrideMethod.BuildOverridenQuery());

                fileSystem.FormatCode(queryOverrideClass.GenerateCode().Template, "cs", queryHandlerclass.Name + "Override");


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

        //public string GetDataQueryClassName(string name)
        //{
        //    return $"{name}QueryAsync";
        //}
    }
}
