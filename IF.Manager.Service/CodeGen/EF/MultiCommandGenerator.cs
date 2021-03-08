using IF.CodeGeneration.CSharp;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Service.CodeGen.Interface;

using System;

namespace IF.Manager.Service.CodeGen.EF
{
   public class MultiCommandGenerator: ICommandMethodGenerator
    {

        ModelClassTreeDto entityTree;
        IFCommand command;
        CSMethod method;

        public MultiCommandGenerator(string name, ModelClassTreeDto entityTree, IFCommand command)
        {
            this.entityTree = entityTree;
            this.command = command;


            this.method = new CSMethod(name, "void", "public");
            this.method.IsAsync = true;
        }


        public CSMethod Build()
        {
            this.method.IsAsync = true;
            this.method.Parameters.Add(new CsMethodParameter() { Name = "command", Type = command.Name  });


            bool IsList = false;

            if(command.Parent!=null)
            {
                IsList = command.IsListCommand();
            }

            string modelPropertyName = "command.Data";

            if (IsList)
            {
                modelPropertyName = "item";
                this.method.Body += " foreach (var item in command.Data)";
                this.method.Body += Environment.NewLine;
                this.method.Body += "{";
                this.method.Body += Environment.NewLine;
            }


            foreach (var command in this.command.Childrens)
            {
                string modelName = command.Model.Name;

                bool IsMulti = command.IsMultiCommand();
               
              

                if (IsMulti)
                {
                    modelName = modelName + "Multi";
                }

              

                this.method.Body += $"var {command.Name} = new {command.Name}();" + Environment.NewLine;
                this.method.Body += $"{command.Name}.Data = {modelPropertyName}.{modelName};" + Environment.NewLine;
                this.method.Body += $"await dispatcher.CommandAsync({command.Name});" + Environment.NewLine;
                this.method.Body += Environment.NewLine;

            }


            if (IsList)
            {
                this.method.Body += Environment.NewLine;
                this.method.Body += Environment.NewLine;
                this.method.Body += "}";
            }

            return this.method;
        }
    }
}
