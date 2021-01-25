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


            this.method.Body += $"{entityTree.Name} entity = new {entityTree.Name}();" + Environment.NewLine;


            foreach (var property in this.entityTree.Childs)
            {

                if (property.IsRelation) continue;

                bool IsModelProperty = ModelClassTreeDto.IsModelProperty(property,command.Model);

                if (!IsModelProperty) continue;

                //CSProperty classProperty = new CSProperty("public", property.Name, false);
                this.method.Body += $"entity.{property.Name} = command.Data.{property.Name};" + Environment.NewLine;
            }

            this.method.Body += $"this.repository.Add(entity);" + Environment.NewLine;

            this.method.Body += $"await this.repository.UnitOfWork.SaveChangesAsync();" + Environment.NewLine;
            this.method.Body += $"command.Data.Id = entity.Id;" + Environment.NewLine;


            return this.method;
        }

        


    }
}
