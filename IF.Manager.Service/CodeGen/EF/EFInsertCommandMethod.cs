using IF.CodeGeneration.CSharp;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Service.CodeGen.Interface;

using System;
using System.Linq;
using System.Text;

namespace IF.Manager.Service.EF
{
    public class EFInsertCommandMethod : ICommandMethodGenerator
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

            bool IsList = command.IsList.Value;

            string modelPropertyName = "command.Data";

            StringBuilder methodBuilder = new StringBuilder();

            if (IsList)
            {
                modelPropertyName = "item";
                methodBuilder.AppendLine(" foreach (var item in command.Data)");
                methodBuilder.AppendLine("{");
            }

            GenerateMethodBody(modelPropertyName, methodBuilder);



            if (IsList)
            {
                methodBuilder.AppendLine();
                methodBuilder.AppendLine();
                methodBuilder.AppendLine("}");
                methodBuilder.AppendLine();
            }

           

            //if(command.IsBeforeExecuteOverride)
            {
                methodBuilder.AppendLine($"this.BeforeExecute(commmand);");
            }

            methodBuilder.AppendLine($"await this.repository.UnitOfWork.SaveChangesAsync();");


            methodBuilder.AppendLine();


            //if(command.IsAfterExecuteOverride)
            {
                methodBuilder.AppendLine($"this.AfterExecute(commmand);");
            }

            if (!IsList)
            {
                var primaryKey = this.command.Model.Properties.SingleOrDefault(p => p.EntityProperty.IsIdentity);
                methodBuilder.AppendLine($"command.Data.{primaryKey.EntityProperty.Name} = entity.{primaryKey.EntityProperty.Name}");
            }


            return this.method;
        }

        public CSMethod BuildOverridenQuery(string name)
        {
            this.method.Name = name;

            StringBuilder builder = new StringBuilder();

            builder.AppendLine("throw new NotImplementedException();");

            this.method.Body = builder.ToString();

            return this.method;
        }

        private void GenerateMethodBody(string modelPropertyName, StringBuilder methodBuilder)
        {
            methodBuilder.AppendLine($"{entityTree.Name} entity = new {entityTree.Name}();");


            foreach (var property in this.entityTree.Childs)
            {

                if (property.IsRelation || property.Name == "Id") continue;

                bool IsModelProperty = ModelClassTreeDto.IsModelProperty(property, command.Model);

                if (!IsModelProperty) continue;

                methodBuilder.AppendLine($"entity.{property.Name} = {modelPropertyName}.{property.Name};");
            }

            methodBuilder.AppendLine($"this.repository.Add(entity);");


        }



    }
}
