using IF.CodeGeneration.Core;
using IF.CodeGeneration.CSharp;
using IF.Manager.Contracts.Model;

using System.Text;

namespace IF.Manager.Service.WebApi
{
    public class WebApiMethodGenerator
    {
        
        private readonly FileSystemCodeFormatProvider fileSystem;
        
        string generatedBasePath;
        IFProcess process;

        public WebApiMethodGenerator(IFProcess process)
        {
        
            this.process = process;

            generatedBasePath = DirectoryHelper.GetTempWebApiControllerDirectory(process.Name);

            this.fileSystem = new FileSystemCodeFormatProvider(generatedBasePath);
        
        }

        public void Generate()
        {
            CSClass controllerClass = new CSClass();
            controllerClass.NameSpace = SolutionHelper.GetProjectNamespace(process.Project);
            controllerClass.Name = $"{process.Name}Controller";
            controllerClass.BaseClass = "ControllerBase";
            controllerClass.Usings.Add("IF.Core.Data");
            controllerClass.Usings.Add("Microsoft.AspNetCore.Mvc");
            controllerClass.Usings.Add("System.Threading.Tasks");
            controllerClass.Usings.Add("IF.Web.Mvc.Exception");
            controllerClass.Usings.Add(SolutionHelper.GetProcessNamaspace(process));

            var dispatcherProperty = new CSProperty("private", "dispatcher", false);
            dispatcherProperty.PropertyTypeString = "IDispatcher";
            dispatcherProperty.IsReadOnly = true;
            controllerClass.Properties.Add(dispatcherProperty);


            CSMethod constructorMethod = new CSMethod(controllerClass.Name, "", "public");
            constructorMethod.Parameters.Add(new CsMethodParameter() { Name = "dispatcher", Type = "IDispatcher" });
            StringBuilder methodBody = new StringBuilder();
            methodBody.AppendFormat("this.dispatcher = dispatcher;");
            methodBody.AppendLine();
            constructorMethod.Body = methodBody.ToString();
            controllerClass.Methods.Add(constructorMethod);

            foreach (var command in process.Commands)
            {
                switch (command.CommandGetType)
                {
                    case Core.Data.CommandType.Insert:
                        controllerClass.Methods.Add(AddMethodGenerate(command));
                        break;
                    case Core.Data.CommandType.Update:
                        controllerClass.Methods.Add(UpdateMethodGenerate(command));
                        break;
                    case Core.Data.CommandType.Delete:
                        break;
                    default:
                        break;
                }
            }


            foreach (var query in process.Queries)
            {

                switch (query.QueryGetType)
                {
                    case Contracts.Enum.QueryGetType.Single:
                        controllerClass.Methods.Add(GetMethodGenerate(query));
                        break;
                    case Contracts.Enum.QueryGetType.List:
                        controllerClass.Methods.Add(GetListMethodGenerate(query));
                        break;
                    default:
                        break;
                }
            }

            this.fileSystem.FormatCode(controllerClass.GenerateCode(), "cs","","");
        }

        private CSMethod GetListMethodGenerate(IFQuery query)
        {
            CSMethod getMethod = new CSMethod($"{query.Name}", "IActionResult", "public");
            getMethod.IsAsync = true;
            getMethod.Attirubites.Add("HttpGet");
            getMethod.Attirubites.Add($"Route(\"api/{process.Name}Controller/{query.Name}\")");
            getMethod.Parameters.Add(new CsMethodParameter() { Type = $"{query.Name}Request", Name = "request", Attirubite = "FromQuery" });


            StringBuilder getMethodBody = new StringBuilder();
            ModelValidation(getMethodBody);

            getMethodBody.AppendLine($"var response = await dispatcher.QueryAsync<{query.Name}Request, {query.Name}Response>(request);");
            getMethodBody.AppendLine($"return Ok(response);");
            getMethod.Body = getMethodBody.ToString();
            return getMethod;
        }

        private CSMethod GetMethodGenerate(IFQuery query)
        {
            CSMethod getMethod = new CSMethod($"{query.Name}", "IActionResult", "public");
            getMethod.IsAsync = true;
            getMethod.Attirubites.Add("HttpGet");
            getMethod.Attirubites.Add($"Route(\"api/{process.Name}Controller/{query.Name}\")");
            getMethod.Parameters.Add(new CsMethodParameter() { Type = $"{query.Name}Request", Name = "request", Attirubite = "FromQuery" });


            StringBuilder getMethodBody = new StringBuilder();
            
            ModelValidation(getMethodBody);

            getMethodBody.AppendLine($"var response = await dispatcher.QueryAsync<{query.Name}Request, {query.Name}Response>(request);");
            getMethodBody.AppendLine($"return Ok(response);");
            getMethod.Body = getMethodBody.ToString();
            return getMethod;
        }

        private CSMethod UpdateMethodGenerate(IFCommand command)
        {
            CSMethod postMethod = new CSMethod($"{command.Name}", "IActionResult", "public");

            AddCommandParameters(command, postMethod);

            postMethod.IsAsync = true;
            postMethod.Attirubites.Add("HttpPost");
            postMethod.Attirubites.Add($"Route(\"api/{process.Name}Controller/{command.Name}\")");

            StringBuilder postMethodBody = new StringBuilder();

            ModelValidation(postMethodBody);

            if (command.IsMultiCommand() && command.Parent == null)
            {
                //IcraMultiDataCommand command = new IcraMultiDataCommand();

                //IcramultiDummyIFClassMapper mapper = new IcramultiDummyIFClassMapper();

                //command.Data = mapper.IcramultiDummyIFClassMapperMap(icara);

                postMethodBody.AppendLine($"{command.Name} command = new {command.Name}();");
                postMethodBody.AppendLine($"{command.IFClassMapper.Name} mapper = new {command.IFClassMapper.Name}();");

                postMethodBody.AppendLine($"command.Data = mapper.{command.IFClassMapper.Name}Map({command.IFClassMapper.IFClass.Name});");

            }

            postMethodBody.AppendLine($"await dispatcher.CommandAsync(command);");
            postMethodBody.AppendLine($"return Ok(command);");

            postMethod.Body = postMethodBody.ToString();
            return postMethod;

        }

        public CSMethod AddMethodGenerate(IFCommand command)
        {
            CSMethod postMethod = new CSMethod($"{command.Name}", "IActionResult", "public");

            AddCommandParameters(command, postMethod);

            postMethod.IsAsync = true;
            postMethod.Attirubites.Add("HttpPost");
            postMethod.Attirubites.Add($"Route(\"api/{process.Name}Controller/{command.Name}\")");

            StringBuilder postMethodBody = new StringBuilder();

            ModelValidation(postMethodBody);

            if (command.IsMultiCommand() && command.Parent == null)
            {
                //IcraMultiDataCommand command = new IcraMultiDataCommand();

                //IcramultiDummyIFClassMapper mapper = new IcramultiDummyIFClassMapper();

                //command.Data = mapper.IcramultiDummyIFClassMapperMap(icara);

                postMethodBody.AppendLine($"{command.Name} command = new {command.Name}();");
                postMethodBody.AppendLine($"{command.IFClassMapper.Name} mapper = new {command.IFClassMapper.Name}();");

                postMethodBody.AppendLine($"command.Data = mapper.{command.IFClassMapper.Name}Map({command.IFClassMapper.IFClass.Name});");

            }

            postMethodBody.AppendLine($"await dispatcher.CommandAsync(command);");
            postMethodBody.AppendLine($"return Ok(command);");

            postMethod.Body = postMethodBody.ToString();


            return postMethod;


        }

        private static void ModelValidation(StringBuilder postMethodBody)
        {
            postMethodBody.AppendLine("if(!ModelState.IsValid){throw new ModelStateException(ModelState);}");
        }

        private static void AddCommandParameters(IFCommand command, CSMethod postMethod)
        {
            if (command.IsMultiCommand() && command.Parent == null)
            {
                postMethod.Parameters.Add(new CsMethodParameter() { Type = $"{command.IFClassMapper.IFClass.Type}", Name = command.IFClassMapper.IFClass.Name, Attirubite = "FromBody" });
            }
            else
            {

                postMethod.Parameters.Add(new CsMethodParameter() { Type = $"{command.Name}", Name = "command", Attirubite = "FromBody" });
            }
        }
    }
}
