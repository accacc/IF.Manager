using IF.CodeGeneration.CSharp;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using System.Linq;

using System;

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

            var identityProperty = entityTree.Childs.SingleOrDefault(c => c.IsIdentity);

            this.method.Body += $"var entity = await this.repository.GetQuery<{entityTree.Name}>().SingleOrDefaultAsync(k => k.{identityProperty.Name} == command.Data.{identityProperty.Name});" + Environment.NewLine + Environment.NewLine;
            this.method.Body += $"if (entity == null){{ throw new BusinessException(\"{entityTree.Name} : No such entity exists\");}}" + Environment.NewLine + Environment.NewLine;


            foreach (var property in this.entityTree.Childs)
            {
                if (property.IsRelation) continue;

                bool IsModelProperty = ModelClassTreeDto.IsModelProperty(property, command.Model);

                if (!IsModelProperty) continue;

                this.method.Body += $"entity.{property.Name} = command.Data.{property.Name};" + Environment.NewLine;
            }


            this.method.Body += $"this.repository.Update(entity);" + Environment.NewLine;
            this.method.Body += $"await this.repository.UnitOfWork.SaveChangesAsync();" + Environment.NewLine;


            return this.method;
        }




    }
}
