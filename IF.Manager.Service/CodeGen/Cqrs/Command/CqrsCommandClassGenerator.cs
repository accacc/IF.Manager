using IF.CodeGeneration.Core;
using IF.CodeGeneration.Language.CSharp;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Service.CodeGen.Interface;

using System;

namespace IF.Manager.Service.CodeGen.Cqrs
{
    public class CqrsCommandClassGenerator : ICqrsCommandClassGenerator
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
            commandClass.Name = $"{command.Name}";

            CSProperty modelProperty = new CSProperty(null, "public", "Data", false);



            string propertyName = command.Model.Name;

            if (command.IsMultiCommand())
            {
                propertyName = propertyName + "Multi";
            }

            bool isList = command.IsList;

            if (isList)
            {
                modelProperty.PropertyTypeString = $"List<{propertyName}>";
            }
            else
            {
                modelProperty.PropertyTypeString = $"{propertyName}";
            }


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

            fileSystem.FormatCode(classes, "cs", commandClass.Name,command.Name);
        }
    }
}
