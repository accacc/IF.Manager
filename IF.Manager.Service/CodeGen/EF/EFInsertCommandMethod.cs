using IF.CodeGeneration.CSharp;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Service.CodeGen.Interface;

using System;

namespace IF.Manager.Service.EF
{
    public class EFInsertCommandMethod: ICommandMethodGenerator
    {

        ModelClassTreeDto entityTree;
        IFCommand command;
        CSMethod method;

        public EFInsertCommandMethod(string name, ModelClassTreeDto entityTree, IFCommand command)
        {
            this.entityTree = entityTree;
            this.command = command;


            this.method = new CSMethod(name, "void", "public");
            this.method.IsAsync = true;
        }


        public CSMethod Build()
        {
            this.method.IsAsync = true;

            this.method.Parameters.Add(new CsMethodParameter() { Name = "command", Type = command.Name });

            bool IsList = command.IsListCommand();            

            string modelPropertyName = "command.Data";

            if (IsList)
            {
                modelPropertyName = "item";
                this.method.Body += " foreach (var item in command.Data)";
                this.method.Body += Environment.NewLine;
                this.method.Body += "{";
                this.method.Body += Environment.NewLine;
            }

            GenerateMethodBody(modelPropertyName);

          

            if (IsList)
            {
                this.method.Body += Environment.NewLine;
                this.method.Body += Environment.NewLine;
                this.method.Body += "}";
                this.method.Body += Environment.NewLine;
            }

            this.method.Body += $"await this.repository.UnitOfWork.SaveChangesAsync();" + Environment.NewLine;
            this.method.Body += Environment.NewLine;
            return this.method;
        }

        private void GenerateMethodBody(string modelPropertyName)
        {
            this.method.Body += $"{entityTree.Name} entity = new {entityTree.Name}();" + Environment.NewLine;


            foreach (var property in this.entityTree.Childs)
            {

                if (property.IsRelation || property.Name == "Id") continue;

                bool IsModelProperty = ModelClassTreeDto.IsModelProperty(property, command.Model);

                if (!IsModelProperty) continue;

                this.method.Body += $"entity.{property.Name} = {modelPropertyName}.{property.Name};" + Environment.NewLine;
            }

            this.method.Body += $"this.repository.Add(entity);" + Environment.NewLine;
          
           
        }



    }
}
