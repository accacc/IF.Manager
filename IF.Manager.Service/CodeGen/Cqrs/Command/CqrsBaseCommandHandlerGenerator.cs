using IF.CodeGeneration.Core;
using IF.CodeGeneration.CSharp;
using IF.Manager.Contracts.Model;
using IF.Manager.Service.CodeGen.EF;

using System;
using System.Text;

namespace IF.Manager.Service.CodeGen.Cqrs.Command
{
    public abstract class CqrsBaseCommandHandlerGenerator
    {

        protected readonly FileSystemCodeFormatProvider fileSystem;
        string generatedBasePath;
        IFProcess process;
        protected readonly IFCommand command;
        string nameSpace;
        public CqrsBaseCommandHandlerGenerator(IFProcess process, IFCommand command)
        {
            this.process = process;
            this.command = command;
            this.nameSpace = SolutionHelper.GetProcessNamaspace(process);
            this.generatedBasePath = DirectoryHelper.GetTempProcessDirectory(process);
            this.fileSystem = new FileSystemCodeFormatProvider(generatedBasePath);
        }
        public CSClass GetCommandHandlerClass()
        {
            string commandName = $"{command.Name}";
            CSClass commandHandlerClass = new CSClass();
            //if (command.IsAfterExecuteOverride || command.IsBeforeExecuteOverride)
            commandHandlerClass.IsPartial = true;
            commandHandlerClass.Name = $"{commandName}CommandHandler";
            commandHandlerClass.NameSpace = nameSpace + ".Commands.Cqrs";
            commandHandlerClass.Usings.Add("IF.Core.Data");
            commandHandlerClass.Usings.Add("IF.Core.Exception");
            commandHandlerClass.Usings.Add("Microsoft.EntityFrameworkCore");
            commandHandlerClass.Usings.Add("System.Threading.Tasks");
            commandHandlerClass.Usings.Add("IF.Core.Persistence");
            commandHandlerClass.Usings.Add($"{SolutionHelper.GetCoreNamespace(command.Process.Project)}");


            commandHandlerClass.InheritedInterfaces.Add($"ICommandHandlerAsync<{commandName}>");

            var repositoryProperty = new CSProperty("private", "repository", false);
            repositoryProperty.PropertyTypeString = $"IRepository";
            repositoryProperty.IsReadOnly = true;
            commandHandlerClass.Properties.Add(repositoryProperty);

            var dispatcher = new CSProperty("private", "dispatcher", false);
            dispatcher.PropertyTypeString = $"IDispatcher";
            dispatcher.IsReadOnly = true;
            commandHandlerClass.Properties.Add(dispatcher);



            CSMethod constructorMethod = new CSMethod(commandHandlerClass.Name, "", "public");
            constructorMethod.Parameters.Add(new CsMethodParameter() { Name = "repository", Type = "IRepository" });
            constructorMethod.Parameters.Add(new CsMethodParameter() { Name = "dispatcher", Type = "IDispatcher" });
            StringBuilder methodBody = new StringBuilder();
            methodBody.AppendLine("this.repository = repository;");
            methodBody.AppendLine("this.dispatcher = dispatcher;");
            methodBody.AppendLine();
            constructorMethod.Body = methodBody.ToString();
            commandHandlerClass.Methods.Add(constructorMethod);

            CSMethod handleMethod = new CSMethod("HandleAsync", "void", "public");
            handleMethod.IsAsync = true;
            handleMethod.Parameters.Add(new CsMethodParameter() { Name = "command", Type = $"{commandName}" });
            handleMethod.Body += $"await this.ExecuteCommand(command);" + Environment.NewLine;

            commandHandlerClass.Methods.Add(handleMethod);
            return commandHandlerClass;
        }

        protected void GenerateCommandContextAndOverrideClass()
        {
            CqrsCommandContextClassGenerator commandContextClassGenerator = new CqrsCommandContextClassGenerator(command, fileSystem);
            commandContextClassGenerator.Generate();

            CqrsCommandOverrideClassGenerator overrideClassGenerator = new CqrsCommandOverrideClassGenerator(command, fileSystem);
            overrideClassGenerator.Generate();
        }

        public void GenerateMultiInsertCqrsHandlerClass()
        {


            CSClass commandHandlerClass = GetCommandHandlerClass();

            MultiCommandGenerator method = new MultiCommandGenerator($"ExecuteCommand", command);

            commandHandlerClass.Methods.Add(method.Build());

            fileSystem.FormatCode(commandHandlerClass.GenerateCode(), "cs","",command.Name);

            GenerateCommandContextAndOverrideClass();
        }



    }
}
