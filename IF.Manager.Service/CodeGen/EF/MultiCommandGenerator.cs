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
            this.method.Parameters.Add(new CsMethodParameter() { Name = "command", Type = command.Name + "Command" });


            this.method.Body += $"{entityTree.Name} entity = new {entityTree.Name}();" + Environment.NewLine;


            foreach (var command in this.command.Childrens)
            {
                this.method.Body +=  $"await dispatcher.CommandAsync(command.{command.Model.Name});" + Environment.NewLine;
            }

            return this.method;
        }
    }
}
