using IF.CodeGeneration.Core;
using IF.CodeGeneration.CSharp;
using IF.Manager.Contracts.Model;

namespace IF.Manager.Service.CodeGen.Cqrs.Command
{
    public class CqrsCommandOverrideClassGenerator
    {
        IFCommand command;
        protected readonly FileSystemCodeFormatProvider fileSystem;
        public CqrsCommandOverrideClassGenerator(IFCommand command, FileSystemCodeFormatProvider fileSystem)
        {
            this.command = command;
            this.fileSystem = fileSystem;
        }

        public void Generate()
        {
            string @namespace = SolutionHelper.GetProcessNamaspace(command.Process);
            CSClass commandHandlerOverrideClass = new CSClass();
            commandHandlerOverrideClass.Usings.Add("IF.Core.Data");
            commandHandlerOverrideClass.Usings.Add("IF.Core.Exception");
            commandHandlerOverrideClass.Usings.Add("Microsoft.EntityFrameworkCore");
            commandHandlerOverrideClass.Usings.Add("System.Threading.Tasks");
            commandHandlerOverrideClass.Usings.Add("IF.Core.Persistence");
            commandHandlerOverrideClass.Usings.Add($"{@namespace}");

            string commandName = $"{command.Name}";
            commandHandlerOverrideClass.IsPartial = true;
            commandHandlerOverrideClass.Name = $"{commandName}CommandHandler";
            commandHandlerOverrideClass.NameSpace = @namespace + ".Commands.Cqrs";
            // AddNameSpaces(query, overClass);
            AddBeforeExecuteMethod(commandHandlerOverrideClass);
            AddAfterExecuteMethod(commandHandlerOverrideClass);

            fileSystem.FormatCode(commandHandlerOverrideClass.GenerateCode().Template, "cs", commandHandlerOverrideClass.Name + "Override",command.Name);
        }

        private void AddBeforeExecuteMethod(CSClass overClass)
        {
            CSMethod method = new CSMethod("BeforeExecute", "void", "public");

            method.IsAsync = true;

            method.Parameters.Add(new CsMethodParameter() { Name = "context", Type =$"{command.Name}Context" });

            overClass.Methods.Add(method);
        }

        private void AddAfterExecuteMethod(CSClass overClass)
        {
            CSMethod method = new CSMethod("AfterExecute", "void", "public");

            method.IsAsync = true;

            method.Parameters.Add(new CsMethodParameter() { Name = "context", Type = $"{command.Name}Context" });

            overClass.Methods.Add(method);
        }
    }
}
