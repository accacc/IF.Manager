using IF.CodeGeneration.CSharp;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Service.CodeGen.Interface;

using System.Linq;
using System.Text;

namespace IF.Manager.Service.EF
{
    public class EFDeleteCommandMethod : ICommandMethodGenerator
    {

        ModelClassTreeDto entityTree;
        IFCommand command;
        CSMethod method;

        public EFDeleteCommandMethod(string name, ModelClassTreeDto entityTree, IFCommand command)
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

            StringBuilder methodBodyBuilder = new StringBuilder();

            methodBodyBuilder.AppendLine($"{command.Name}Context context = new  {command.Name}Context();");
            methodBodyBuilder.AppendLine($"context.Command = command;");
            methodBodyBuilder.AppendLine($"context.Model = command.Data;");
            methodBodyBuilder.AppendLine();

            var identityProperty = entityTree.Childs.SingleOrDefault(c => c.IsIdentity);

            methodBodyBuilder.AppendLine($"var entity = await this.repository.GetQuery<{entityTree.Name}>().SingleOrDefaultAsync(k => k.{identityProperty.Name} == command.Data.{identityProperty.Name});");
            methodBodyBuilder.AppendLine();
            methodBodyBuilder.AppendLine($"if (entity == null){{ throw new BusinessException(\"{entityTree.Name} : No such entity exists\");}}");
            methodBodyBuilder.AppendLine();





            //if (IsList)
            //{
            //    modelPropertyName = "item";
            //    methodBodyBuilder.AppendLine(" foreach (var item in command.Data)");
            //    methodBodyBuilder.AppendLine("{");
            //}

            GenerateMethodBody(modelPropertyName, methodBodyBuilder);

            methodBodyBuilder.AppendLine("");

            //if (IsList)
            //{
            //    methodBodyBuilder.AppendLine();
            //    methodBodyBuilder.AppendLine();
            //    methodBodyBuilder.AppendLine("}");
            //    methodBodyBuilder.AppendLine();
            //}

            methodBodyBuilder.AppendLine();


            //if(command.IsBeforeExecuteOverride)
            {
                methodBodyBuilder.AppendLine($"await this.BeforeExecute(context);");
            }

            methodBodyBuilder.AppendLine();


            methodBodyBuilder.AppendLine($"await this.repository.UnitOfWork.SaveChangesAsync();");

            methodBodyBuilder.AppendLine();


            methodBodyBuilder.AppendLine();


            //if(command.IsAfterExecuteOverride)
            {
                methodBodyBuilder.AppendLine($"await this.AfterExecute(context);");
            }

            //if (!IsList)
            //{
            //    var primaryKey = this.command.Model.Properties.SingleOrDefault(p => p.EntityProperty.IsIdentity);
            //    methodBodyBuilder.AppendLine($"command.Data.{primaryKey.EntityProperty.Name} = entity.{primaryKey.EntityProperty.Name};");
            //}


            this.method.Body = methodBodyBuilder.ToString();

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

        private void GenerateMethodBody(string modelPropertyName, StringBuilder methodBodyBuilder)
        {
            //methodBodyBuilder.AppendLine($"{entityTree.Name} entity = new {entityTree.Name}();");


            //foreach (var property in this.entityTree.Childs)
            //{

            //    if (property.IsRelation || property.Name == "Id") continue;

            //    bool IsModelProperty = ModelClassTreeDto.IsModelProperty(property, command.Model);

            //    if (!IsModelProperty) continue;

            //    methodBodyBuilder.AppendLine($"entity.{property.Name} = {modelPropertyName}.{property.Name};");
            //}

            methodBodyBuilder.AppendLine($"context.Entity = entity;");

            methodBodyBuilder.AppendLine($"this.repository.Delete(entity);");


        }



    }
}
