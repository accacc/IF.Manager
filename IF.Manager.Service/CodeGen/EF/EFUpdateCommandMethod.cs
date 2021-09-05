using IF.CodeGeneration.Language.CSharp;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using System.Linq;

using System;
using System.Text;

namespace IF.Manager.Service.EF
{
    public class EFUpdateCommandMethod
    {

        ModelClassTreeDto entityTree;
        IFCommand command;
        CSMethod method;

        public EFUpdateCommandMethod(string name, ModelClassTreeDto entityTree, IFCommand command)
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

            StringBuilder methodBodyBuilder = new StringBuilder();

            methodBodyBuilder.AppendLine($"{command.Name}Context context = new  {command.Name}Context();");
            methodBodyBuilder.AppendLine($"context.Command = command;");
            methodBodyBuilder.AppendLine($"context.Model = command.Data;");
            methodBodyBuilder.AppendLine();


            var identityProperty = entityTree.Childs.FirstOrDefault(c => c.IsIdentity);

            methodBodyBuilder.AppendLine($"var entity = await this.repository.GetQuery<{entityTree.Name}>().SingleOrDefaultAsync(k => k.{identityProperty.Name} == command.Data.{identityProperty.Name});");
            methodBodyBuilder.AppendLine();
            methodBodyBuilder.AppendLine($"if (entity == null){{ throw new BusinessException(\"{entityTree.Name} : No such entity exists\");}}");
            methodBodyBuilder.AppendLine();



            foreach (var property in this.entityTree.Childs)
            {
                if (property.IsRelation) continue;

                bool IsModelProperty = ModelClassTreeDto.IsModelProperty(property, command.Model);

                if (!IsModelProperty) continue;

                methodBodyBuilder.AppendLine($"entity.{property.Name} = command.Data.{property.Name};");
                methodBodyBuilder.AppendLine();
            }

            methodBodyBuilder.AppendLine($"context.Entity = entity;");
            methodBodyBuilder.AppendLine();
            methodBodyBuilder.AppendLine($"await this.BeforeExecute(context);");
            methodBodyBuilder.AppendLine();
            methodBodyBuilder.AppendLine($"this.repository.Update(entity);");
            methodBodyBuilder.AppendLine();
            methodBodyBuilder.AppendLine($"await this.repository.UnitOfWork.SaveChangesAsync();");
            methodBodyBuilder.AppendLine();



            //if(command.IsAfterExecuteOverride)
            {
                methodBodyBuilder.AppendLine($"await this.AfterExecute(context);");
            }
            methodBodyBuilder.AppendLine();


            this.method.Body = methodBodyBuilder.ToString();

            return this.method;
        }




    }
}
