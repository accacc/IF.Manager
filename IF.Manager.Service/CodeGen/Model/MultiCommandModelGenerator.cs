using System;
using System.Collections.Generic;
using System.Text;

using IF.CodeGeneration.Core;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Service.CodeGen.Interface;

using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace IF.Manager.Service.CodeGen.Model
{

    public class MultiCommandModelGenerator : IModelGenerator
    {

        FileSystemCodeFormatProvider fileSystem;
        IFModel model; string nameSpace;
        IFCommand command;

        public MultiCommandModelGenerator(FileSystemCodeFormatProvider fileSystem, IFModel model, string nameSpace, IFCommand command)
        {
            this.fileSystem = fileSystem;
            this.model = model;
            this.nameSpace = nameSpace;
            this.command = command;
        }

        public void Generate()
        {
            List<MultiModelClass> alls = new List<MultiModelClass>();

            string name = DirectoryHelper.AddAsLastWord(model.Name, "DataModel");

            if(command.IsMultiCommand())
            {
                name = name + "Multi";
            }

            MultiModelClass modelClass = new MultiModelClass(nameSpace, name, command);

            modelClass.Usings.Add("System");
            modelClass.Usings.Add("System.Collections.Generic");

            modelClass.Build();

            alls.Add(modelClass);

            StringBuilder builder = new StringBuilder();

            foreach (var cls in alls)
            {
                builder.AppendLine(cls.GenerateCode().Template);

                this.fileSystem.FormatCode(builder.ToString(), "cs", name);
            }
        }
       
    }
}
