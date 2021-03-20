using IF.CodeGeneration.CSharp;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Service.CodeGen.Interface;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IF.Manager.Service.CodeGen.EF
{
    public class MultiCommandGenerator : ICommandMethodGenerator
    {

        IFCommand parentCommand;
        CSMethod method;

        public MultiCommandGenerator(string name,IFCommand parentCommand)
        {
            this.parentCommand = parentCommand;


            this.method = new CSMethod(name, "void", "public");
            this.method.IsAsync = true;
        }
        private void ForeignKeyDispatchNextCommands(IFCommand currentCommand, List<IFCommand> nextCommands, IFCommand parentCommand,StringBuilder methodBody, int level)
        {

            level++;

            string indent = new String(' ', level * 4);

            if (currentCommand.Model.Entity.Relations.Any())
            {

              


                foreach (var nextCommand in nextCommands)
                {

                    if (nextCommand.IsMultiCommand() && nextCommand.Childrens.Any())
                    {
                        ForeignKeyDispatchNextCommands(currentCommand, nextCommand.Childrens.ToList(),parentCommand, methodBody,level);
                    }
                    else
                    {

                        var relation = currentCommand.Model.Entity.Relations.SingleOrDefault(r => r.RelationId == nextCommand.Model.EntityId);
                       

                        if (relation != null)
                        {

                            IFModelProperty foreignProperty = null;

                            IFModelProperty primaryKeyProperty = currentCommand.Model.Properties.SingleOrDefault(p => p.EntityProperty.IsIdentity); ;

                            switch (relation.Type)
                            {
                                case Contracts.Enum.EntityRelationType.None:
                                    break;
                                case Contracts.Enum.EntityRelationType.OneToMany:
                                   foreignProperty  = nextCommand.Model.Properties.SingleOrDefault(p => p.EntityPropertyId == relation.ForeignKeyIFEntityPropertyId);
                                    break;
                                case Contracts.Enum.EntityRelationType.OneToOne:
                                    relation = relation.Relation.Relations.SingleOrDefault(r => r.RelationId == relation.EntityId);
                                    foreignProperty = nextCommand.Model.Properties.SingleOrDefault(p => p.EntityPropertyId == relation.ForeignKeyIFEntityPropertyId);
                                    break;
                                case Contracts.Enum.EntityRelationType.ManyToMany:
                                    break;
                                default:
                                    foreignProperty = nextCommand.Model.Properties.SingleOrDefault(p => p.EntityPropertyId == relation.ForeignKeyIFEntityPropertyId);
                                    break;
                            }

                            methodBody.AppendLine();
                            methodBody.AppendLine();

                          

                            if (nextCommand.IsList.Value || nextCommand.Parent.IsMultiList)
                            {

                                string foreachName = nextCommand.Model.Name;
                                string leftPropertyModelName = nextCommand.Model.Name+".";
                                string rightPropertyModelName = currentCommand.Model.Name;


                                if (nextCommand.Parent.IsMultiList && !nextCommand.IsList.Value)
                                {
                                    foreachName = "command.Data." + nextCommand.Model.Name + "Multi";
                                    rightPropertyModelName = " command.Data." + rightPropertyModelName;
                                }
                                
                                if(!nextCommand.Parent.IsMultiList && nextCommand.IsList.Value)
                                {
                                    foreachName = "command.Data." + nextCommand.Model.Name;
                                    leftPropertyModelName = "";
                                    rightPropertyModelName = " command.Data." + rightPropertyModelName;
                                }

                                if(nextCommand.Parent.IsMultiList && nextCommand.IsList.Value)
                                {
                                    foreachName = $"item{level - 1}.{nextCommand.Model.Name}";
                                    leftPropertyModelName = "";
                                    rightPropertyModelName = $"item0.{rightPropertyModelName}";

                                }

                                methodBody.AppendLine($"foreach (var item{level} in {foreachName})");
                                methodBody.AppendLine($"{{");

                                methodBody.AppendLine($"item{level}.{leftPropertyModelName}{foreignProperty.EntityProperty.Name} = {rightPropertyModelName}.{primaryKeyProperty.EntityProperty.Name};");
                                methodBody.AppendLine($"}}");
                            }
                            else
                            {

                                var currentModelPath = currentCommand.GetModelPathUpTheRoot();
                                var nextModelPath = nextCommand.GetModelPathUpTheRoot();


                                if (!String.IsNullOrWhiteSpace(currentModelPath))
                                {
                                    currentModelPath += ".";
                                }


                                if (!String.IsNullOrWhiteSpace(nextModelPath))
                                {
                                    nextModelPath += ".";
                                }

                                methodBody.AppendLine($"command.Data.{nextModelPath}{nextCommand.Model.Name}.{foreignProperty.EntityProperty.Name} = command.Data.{currentModelPath}{currentCommand.Model.Name}.{primaryKeyProperty.EntityProperty.Name};");
                            }

                            methodBody.AppendLine();
                            methodBody.AppendLine();
                        }


                        //if (nextCommand.Childrens.Any())
                        //{
                        //   // ForeignKeyDispatchNextCommands(currentCommand, nextCommand.Childrens.ToList(),parentCommand, methodBody);
                        //}
                    }

                }

            }




        }

      

        public CSMethod Build()
        {

            int level = 0;
            this.method.IsAsync = true;
            this.method.Parameters.Add(new CsMethodParameter() { Name = "command", Type = parentCommand.Name });


            bool IsList = false;

            if (parentCommand.Parent != null)
            {
                IsList = parentCommand.IsList.Value;
            }

            string modelPropertyName = "command.Data";

            StringBuilder methodBuilder = new StringBuilder();

            if (IsList)
            {
                modelPropertyName = $"item{level}";
                methodBuilder.AppendLine($" foreach (var item{level} in command.Data)");
                methodBuilder.AppendLine("{");
            }


            var childCommands = this.parentCommand.Childrens.OrderBy(c => c.Sequence).ToList();

            int i = 0;
         

            foreach (var childCommand in childCommands)
            {

                string modelName = childCommand.Model.Name;

                bool IsMulti = childCommand.IsMultiCommand();

                if (IsMulti)
                {
                    modelName = modelName + "Multi";
                }

                methodBuilder.AppendLine($"var {childCommand.Name} = new {childCommand.Name}();");
                methodBuilder.AppendLine($"{childCommand.Name}.Data = {modelPropertyName}.{modelName};");
                methodBuilder.AppendLine($"await dispatcher.CommandAsync({childCommand.Name});");
                methodBuilder.AppendLine();
                methodBuilder.AppendLine();

                var nextCommands = childCommands.Skip(i).ToList();

                if (!childCommand.IsMultiCommand())
                {
                    ForeignKeyDispatchNextCommands(childCommand, nextCommands, parentCommand, methodBuilder,level);
                }
                i++;
            }


            if (IsList)
            {
                methodBuilder.AppendLine();
                methodBuilder.AppendLine();
                methodBuilder.AppendLine("}");
            }

            this.method.Body = methodBuilder.ToString();

            return this.method;
        }
    }
}
