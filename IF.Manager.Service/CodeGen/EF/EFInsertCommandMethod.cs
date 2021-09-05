using IF.CodeGeneration.Language.CSharp;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Service.CodeGen.Interface;

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

            bool IsList = command.IsList;

            string modelPropertyName = "command.Data";

            StringBuilder methodBodyBuilder = new StringBuilder();

            methodBodyBuilder.AppendLine($"{command.Name}Context context = new  {command.Name}Context();");
            methodBodyBuilder.AppendLine($"context.Command = command;");
            methodBodyBuilder.AppendLine($"context.Model = command.Data;");
            methodBodyBuilder.AppendLine();

            if (IsList)
            {
                modelPropertyName = "item";
                methodBodyBuilder.AppendLine(" foreach (var item in command.Data)");
                methodBodyBuilder.AppendLine("{");
            }

            GenerateMethodBody(modelPropertyName, methodBodyBuilder);



            if (IsList)
            {
                methodBodyBuilder.AppendLine();
                methodBodyBuilder.AppendLine();
                methodBodyBuilder.AppendLine("}");
                methodBodyBuilder.AppendLine();
            }

            methodBodyBuilder.AppendLine();

            methodBodyBuilder.AppendLine();


            methodBodyBuilder.AppendLine($"await this.repository.UnitOfWork.SaveChangesAsync();");

            methodBodyBuilder.AppendLine();


            methodBodyBuilder.AppendLine();

            methodBodyBuilder.AppendLine($"await this.AfterExecute(context);");

            try
            {
                if (!IsList)
                {
                    var primaryKey = this.command.Model.Properties.FirstOrDefault(p => p.EntityProperty.IsIdentity);
                    methodBodyBuilder.AppendLine($"command.Data.{primaryKey.EntityProperty.Name} = entity.{primaryKey.EntityProperty.Name};");
                }
            }
            catch (System.Exception ex)
            {

                
            }


            this.method.Body = methodBodyBuilder.ToString();

            return this.method;
        }

        public CSMethod BuildOverridenQuery()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("throw new NotImplementedException();");

            this.method.Body = builder.ToString();

            return this.method;
        }

        private void GenerateMethodBody(string modelPropertyName, StringBuilder methodBodyBuilder)
        {
            methodBodyBuilder.AppendLine($"{entityTree.Name} entity = new {entityTree.Name}();");


            foreach (var property in this.entityTree.Childs)
            {

                if (property.IsRelation || property.Name == "Id") continue;

                bool IsModelProperty = ModelClassTreeDto.IsModelProperty(property, command.Model);

                if (!IsModelProperty) continue;

                methodBodyBuilder.AppendLine($"entity.{property.Name} = {modelPropertyName}.{property.Name};");
            }
            
            methodBodyBuilder.AppendLine($"context.Entity = entity;");

            methodBodyBuilder.AppendLine($"await this.BeforeExecute(context);");

            methodBodyBuilder.AppendLine($"this.repository.Add(entity);");


        }



    }
}
