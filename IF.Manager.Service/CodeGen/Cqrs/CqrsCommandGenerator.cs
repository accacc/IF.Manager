﻿using IF.CodeGeneration.Core;
using IF.CodeGeneration.CSharp;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;
using IF.Manager.Service.EF;
using IF.Manager.Service.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IF.Manager.Service
{
    public class CqrsCommandGenerator
    {
        private readonly IEntityService entityService;
        private readonly IModelService modelService;
        private readonly FileSystemCodeFormatProvider fileSystem;
        //private readonly VsManager vs;
        string generatedBasePath;
        IFProcess process;

        public CqrsCommandGenerator(IEntityService entityService, IModelService modelService, IFProcess process)
        {
            this.entityService = entityService;
            this.modelService = modelService;
            this.process = process;

            generatedBasePath = DirectoryHelper.GetTempProcessDirectory(process);

            this.fileSystem = new FileSystemCodeFormatProvider(generatedBasePath);
            //this.vs = new VsManager();
        }


        public async Task Generate()
        {

            foreach (var command in process.Commands)
            {
                var entityTree = await entityService.GetEntityTree(command.Model.EntityId);
                //var properties = await this.modelService.GetModelPropertyList(command.ModelId);
                ModelGenerator modelGenerator = new ModelGenerator(fileSystem);
                modelGenerator.GenerateModels(command.Model, process, entityTree);
                GenerateCqrsCommandClass(command, process, entityTree);

                switch (command.CommandGetType)
                {
                    case Core.Data.CommandType.Insert:
                        GenerateInsertCqrsHandlerClass(command, process, entityTree);
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


        }

        private void GenerateDeleteCqrsHandlerClass(IFCommand command, IFProcess process, ClassTreeDto entityTree)
        {
            throw new NotImplementedException();
        }

        private void GenerateUpdateCqrsHandlerClass(IFCommand command, IFProcess process, ClassTreeDto entityTree)
        {

            string nameSpace = SolutionHelper.GetProcessNamaspace(process);

            CSClass commandHandlerClass = GetCommandHandlerClass(command, process, nameSpace);

            EFUpdateCommandMethod method = new EFUpdateCommandMethod(nameSpace, $"ExecuteCommand", entityTree, command);

            commandHandlerClass.Methods.Add(method.Build());

            fileSystem.FormatCode(commandHandlerClass.GenerateCode(), "cs");

        }

        private void GenerateInsertCqrsHandlerClass(IFCommand command, IFProcess process, ClassTreeDto entityTree)
        {

            string nameSpace = SolutionHelper.GetProcessNamaspace(process);

            CSClass commandHandlerClass = GetCommandHandlerClass(command, process, nameSpace);

            EFInsertCommandMethod method = new EFInsertCommandMethod(nameSpace, $"ExecuteCommand", entityTree, command);

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
            //commandHandlerClass.Usings.Add($"{nameSpace}.Contract.Commands");
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

        private void GenerateCqrsCommandClass(IFCommand command, IFProcess process, ClassTreeDto entityTree)
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