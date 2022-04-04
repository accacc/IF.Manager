using IF.CodeGeneration.Core;
using IF.CodeGeneration.Language.CSharp;
using IF.Manager.Contracts.Model;

namespace IF.Manager.Service.CodeGen.Cqrs.Command
{


    public class CqrsCommandContextClassGenerator
    {

        IFCommand command;
        FileSystemCodeFormatProvider fileSystem;

        public CqrsCommandContextClassGenerator(IFCommand command, FileSystemCodeFormatProvider fileSystem)
        {
            this.command = command;
            this.fileSystem = fileSystem;
        }

        public void Generate()
        {
            CSClass commandContextClass = new CSClass();
            commandContextClass.Name = $"{command.Name}Context";
            commandContextClass.Usings.Add($"{SolutionHelper.GetProcessNamaspace(command.Process)}");
            commandContextClass.Usings.Add($"{SolutionHelper.GetCoreNamespace(command.Process.Project)}");
            commandContextClass.Usings.Add("System.Collections.Generic");

            CSProperty commandProperty = new CSProperty("public", "Command", false);
            commandProperty.PropertyTypeString = this.command.Name;

            commandContextClass.Properties.Add(commandProperty);

            CSProperty modelProperty = new CSProperty("public", "Model", false);
            modelProperty.PropertyTypeString = this.command.Model.Name;

            if(command.IsMultiCommand())
            {
                modelProperty.PropertyTypeString += "Multi";
            }

            if(command.IsList)
            {
                modelProperty.GenericType = "List";
            }
            commandContextClass.Properties.Add(modelProperty);

            CSProperty entityProperty = new CSProperty("public", "Entity", false);
            entityProperty.PropertyTypeString = this.command.Model.Entity.Name;

            commandContextClass.Properties.Add(entityProperty);

            fileSystem.FormatCode(commandContextClass.GenerateCode().Template, "cs", command.Name + "Context",command.Name);



        }
    }
}