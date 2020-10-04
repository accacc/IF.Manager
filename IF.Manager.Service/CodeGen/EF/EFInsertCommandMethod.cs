using IF.CodeGeneration.CSharp;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Service.EF
{
    public class EFInsertCommandMethod
    {

        EntityTreeDto entityTree;
        IFCommand command;
        CSMethod method;

        public EFInsertCommandMethod(string nameSpace, string name, EntityTreeDto entityTree, IFCommand command)
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


            foreach (var property in this.entityTree.Childs)
            {

                if (property.IsRelation) continue;

                bool IsModelProperty = EntityTreeDto.IsModelProperty(property,command.Model);

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
