using IF.CodeGeneration.Core;
using IF.CodeGeneration.CSharp;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Service.CodeGen.Interface;

using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Service.CodeGen.Cqrs
{
    public class CqrsCommandClassGenerator: ICqrsCommandClassGenerator
    {
        IFCommand command;
        IFProcess process;
        ModelClassTreeDto entityTree;
        FileSystemCodeFormatProvider fileSystem;
        public CqrsCommandClassGenerator(IFCommand command, IFProcess process, ModelClassTreeDto entityTree, FileSystemCodeFormatProvider fileSystem)
        {
            this.command = command;
            this.process = process;
            this.entityTree = entityTree;
            this.fileSystem = fileSystem;
        }
        public void Generate()
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
