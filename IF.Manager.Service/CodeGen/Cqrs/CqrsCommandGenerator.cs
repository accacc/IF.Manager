using IF.CodeGeneration.Core;
using IF.CodeGeneration.CSharp;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;
using IF.Manager.Service.CodeGen.EF;
using IF.Manager.Service.CodeGen.Interface;
using IF.Manager.Service.EF;
using IF.Manager.Service.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IF.Manager.Service
{
    public class CqrsCommandGenerator
    {
        private readonly IEntityService entityService;
        private readonly FileSystemCodeFormatProvider fileSystem;
        string generatedBasePath;
        IFProcess process;

        public CqrsCommandGenerator(IEntityService entityService, IFProcess process)
        {
            this.entityService = entityService;
            this.process = process;

            generatedBasePath = DirectoryHelper.GetTempProcessDirectory(process);

            this.fileSystem = new FileSystemCodeFormatProvider(generatedBasePath);
        }


        public async Task Generate()
        {
            string nameSpace = SolutionHelper.GetProcessNamaspace(process);
            var rootCommands = process.Commands.Where(c => !c.ParentId.HasValue).ToList();
            await Recursive(nameSpace,rootCommands);

        }

        private async Task Recursive(string nameSpace,List<IFCommand> commmands)
        {

           
            foreach (var command in commmands)
            {
                if (command.Childrens.Any())
                {
                    await GenerateParentCommand(nameSpace, command);

                    await Recursive(nameSpace, command.Childrens.ToList());
                }
                else
                {
                    await GenerateChildCommand(nameSpace, command);
                }
            }


        }

        private async Task GenerateParentCommand(string nameSpace, IFCommand command)
        {

            var entityTree = await entityService.GetEntityTree(command.Model.EntityId);

            ModelGenerator modelGenerator = new ModelGenerator(fileSystem);

            modelGenerator.GenerateModels(command.Model, nameSpace, entityTree);

            GenerateCqrsCommandClass(command, process, entityTree);

            switch (command.CommandGetType)
            {
                case Core.Data.CommandType.Insert:
                    MultiCommandGenerator method = new MultiCommandGenerator($"ExecuteCommand", entityTree, command);
                    GenerateInsertCqrsHandlerClass(command, process, method);
                    break;
                case Core.Data.CommandType.Update:
                    GenerateUpdateCqrsHandlerClass(command, process, entityTree);
                    break;
                case Core.Data.CommandType.Delete:
                    GenerateDeleteCqrsHandlerClass(command, process, entityTree);
                    break;
                default:
                    throw new ApplicationException("unknow command type");
            }
        }

        private async Task GenerateChildCommand(string nameSpace, IFCommand command)
        {
            var entityTree = await entityService.GetEntityTree(command.Model.EntityId);

            ModelGenerator modelGenerator = new ModelGenerator(fileSystem);

            modelGenerator.GenerateModels(command.Model, nameSpace, entityTree);

            GenerateCqrsCommandClass(command, process, entityTree);

            switch (command.CommandGetType)
            {
                case Core.Data.CommandType.Insert:
                    EFInsertCommandMethod method = new EFInsertCommandMethod($"ExecuteCommand", entityTree, command);
                    GenerateInsertCqrsHandlerClass(command, process,method);
                    break;
                case Core.Data.CommandType.Update:
                    GenerateUpdateCqrsHandlerClass(command, process, entityTree);
                    break;
                case Core.Data.CommandType.Delete:
                    GenerateDeleteCqrsHandlerClass(command, process, entityTree);
                    break;
                default:
                    throw new ApplicationException("unknow command type");
            }
        }

        private void GenerateDeleteCqrsHandlerClass(IFCommand command, IFProcess process, ModelClassTreeDto entityTree)
        {
            throw new NotImplementedException();
        }

        private void GenerateUpdateCqrsHandlerClass(IFCommand command, IFProcess process, ModelClassTreeDto entityTree)
        {

            string nameSpace = SolutionHelper.GetProcessNamaspace(process);

            CSClass commandHandlerClass = GetCommandHandlerClass(command, process, nameSpace);

            EFUpdateCommandMethod method = new EFUpdateCommandMethod(nameSpace, $"ExecuteCommand", entityTree, command);

            commandHandlerClass.Methods.Add(method.Build());

            fileSystem.FormatCode(commandHandlerClass.GenerateCode(), "cs");

        }

        private void GenerateInsertCqrsHandlerClass(IFCommand command, IFProcess process,ICommandMethodGenerator method)
        {

            string nameSpace = SolutionHelper.GetProcessNamaspace(process);

            CSClass commandHandlerClass = GetCommandHandlerClass(command, process, nameSpace);           

            commandHandlerClass.Methods.Add(method.Build());

            fileSystem.FormatCode(commandHandlerClass.GenerateCode(), "cs");


        }

        private static CSClass GetCommandHandlerClass(IFCommand command, IFProcess process, string nameSpace)
        {
            string commandName = $"{command.Name}";
            CSClass commandHandlerClass = new CSClass();
            commandHandlerClass.Name = $"{commandName}CommandHandler";
            commandHandlerClass.NameSpace = nameSpace + ".Commands.Cqrs";
            commandHandlerClass.Usings.Add("IF.Core.Data");
            commandHandlerClass.Usings.Add("IF.Core.Exception");
            commandHandlerClass.Usings.Add("Microsoft.EntityFrameworkCore");
            commandHandlerClass.Usings.Add("System.Threading.Tasks");
            commandHandlerClass.Usings.Add("IF.Core.Persistence");
            commandHandlerClass.Usings.Add($"{SolutionHelper.GetCoreNamespace(command.Process.Project)}");


            commandHandlerClass.InheritedInterfaces.Add($"ICommandHandlerAsync<{commandName}Command>");

            var repositoryProperty = new CSProperty("private", "repository", false);
            repositoryProperty.PropertyTypeString = $"IRepository";
            repositoryProperty.IsReadOnly = true;
            commandHandlerClass.Properties.Add(repositoryProperty);


            CSMethod constructorMethod = new CSMethod(commandHandlerClass.Name, "", "public");
            constructorMethod.Parameters.Add(new CsMethodParameter() { Name = "repository", Type = "IRepository" });
            StringBuilder methodBody = new StringBuilder();
            methodBody.AppendFormat("this.repository = repository;");
            methodBody.AppendLine();
            constructorMethod.Body = methodBody.ToString();
            commandHandlerClass.Methods.Add(constructorMethod);

            CSMethod handleMethod = new CSMethod("HandleAsync", "void", "public");
            handleMethod.IsAsync = true;
            handleMethod.Parameters.Add(new CsMethodParameter() { Name = "command", Type = $"{commandName}Command" });
            handleMethod.Body += $"await this.ExecuteCommand(command);" + Environment.NewLine;

            commandHandlerClass.Methods.Add(handleMethod);
            return commandHandlerClass;
        }

        private void GenerateCqrsCommandClass(IFCommand command, IFProcess process, ModelClassTreeDto entityTree)
        {
            CSClass commandClass = new CSClass();
            commandClass.BaseClass = "BaseCommand";
            commandClass.Name = $"{command.Name}Command";

            CSProperty modelProperty = new CSProperty(null, "public", "Data", false);
            modelProperty.PropertyTypeString = $"{command.Model.Name}";
            commandClass.Properties.Add(modelProperty);

            string nameSpace = SolutionHelper.GetProcessNamaspace(process);

            string classes = "";
            classes += "using IF.Core.Data;";
            classes += Environment.NewLine;
            classes += "using System.Collections.Generic;";
            classes += Environment.NewLine;
            classes += Environment.NewLine;
            classes += "namespace " + nameSpace;
            classes += Environment.NewLine;
            classes += "{";
            classes += Environment.NewLine;
            classes += @commandClass.GenerateCode().Template;
            classes += Environment.NewLine;
            classes += "}";

            fileSystem.FormatCode(classes, "cs", commandClass.Name);
        }
    }


}
