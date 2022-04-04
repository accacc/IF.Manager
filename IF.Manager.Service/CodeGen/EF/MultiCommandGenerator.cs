using IF.CodeGeneration.Language.CSharp;
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

        public MultiCommandGenerator(string name, IFCommand parentCommand)
        {
            this.parentCommand = parentCommand;


            this.method = new CSMethod(name, "void", "public");
            this.method.IsAsync = true;
        }



        public CSMethod Build()
        {

            int level = 0;
            this.method.IsAsync = true;
            this.method.Parameters.Add(new CsMethodParameter() { Name = "command", Type = parentCommand.Name });


            bool IsList = false;

            if (parentCommand.Parent != null)
            {
                IsList = parentCommand.IsList;
            }

            string modelPropertyName = "command.Data";

            StringBuilder methodBodyBuilder = new StringBuilder();

            methodBodyBuilder.AppendLine($"{parentCommand.Name}Context context = new  {parentCommand.Name}Context();");
            methodBodyBuilder.AppendLine($"context.Command = command;");
            methodBodyBuilder.AppendLine($"context.Model = command.Data;");
            methodBodyBuilder.AppendLine();

            methodBodyBuilder.AppendLine($"await this.BeforeExecute(context);");
            methodBodyBuilder.AppendLine();

            if (IsList)
            {
                modelPropertyName = $"item{level}";
                methodBodyBuilder.AppendLine($" foreach (var item{level} in command.Data)");
                methodBodyBuilder.AppendLine("{");
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

                methodBodyBuilder.AppendLine($"var {childCommand.Name} = new {childCommand.Name}();");
                methodBodyBuilder.AppendLine($"{childCommand.Name}.Data = {modelPropertyName}.{modelName};");
                methodBodyBuilder.AppendLine($"await dispatcher.CommandAsync({childCommand.Name});");
                methodBodyBuilder.AppendLine();
                methodBodyBuilder.AppendLine();

                var nextCommands = childCommands.Skip(i).ToList();

                if (!childCommand.IsMultiCommand())
                {
                    DispatchForeignKeyToNextCommands(childCommand, nextCommands, parentCommand, methodBodyBuilder, level);
                }
                i++;
            }


            if (IsList)
            {
                methodBodyBuilder.AppendLine();
                methodBodyBuilder.AppendLine();
                methodBodyBuilder.AppendLine("}");
            }

            methodBodyBuilder.AppendLine($"await this.AfterExecute(context);");

            this.method.Body = methodBodyBuilder.ToString();

            return this.method;
        }

        private void DispatchForeignKeyToNextCommands(IFCommand currentCommand, List<IFCommand> nextCommands, IFCommand parentCommand, StringBuilder methodBody, int level)
        {

            level++;

            string indent = new String(' ', level * 4);

            if (currentCommand.Model.Entity.Relations.Any())
            {




                foreach (var nextCommand in nextCommands)
                {

                    if (nextCommand.IsMultiCommand() && nextCommand.Childrens.Any())
                    {
                        DispatchForeignKeyToNextCommands(currentCommand, nextCommand.Childrens.ToList(), parentCommand, methodBody, level);
                    }
                    else
                    {

                        var relation = currentCommand.Model.Entity.Relations.SingleOrDefault(r => r.RelationId == nextCommand.Model.EntityId);


                        if (relation != null)
                        {

                            IFModelProperty foreignProperty = null;

                            IFModelProperty primaryKeyProperty = currentCommand.Model.Properties.FirstOrDefault(p => p.EntityProperty.IsIdentity); ;

                            switch (relation.Type)
                            {
                                case Contracts.Enum.EntityRelationType.None:
                                    break;
                                case Contracts.Enum.EntityRelationType.OneToMany:
                                    foreignProperty = nextCommand.Model.Properties.SingleOrDefault(p => p.EntityPropertyId == relation.ForeignKeyIFEntityPropertyId);
                                    break;
                                case Contracts.Enum.EntityRelationType.OneToOne:
                                    //relation = relation.Relation.Relations.SingleOrDefault(r => r.RelationId == relation.EntityId);
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



                            if (nextCommand.IsList || nextCommand.Parent.IsMultiList())
                            {

                                string foreachName = nextCommand.Model.Name;
                                string leftPropertyModelName = nextCommand.Model.Name + ".";
                                string rightPropertyModelName = currentCommand.Model.Name;


                                if (nextCommand.Parent.IsMultiList() && !nextCommand.IsList)
                                {
                                    foreachName = "command.Data." + nextCommand.Model.Name + "Multi";
                                    rightPropertyModelName = " command.Data." + rightPropertyModelName;
                                }

                                if (!nextCommand.Parent.IsMultiList() && nextCommand.IsList)
                                {
                                    foreachName = "command.Data." + nextCommand.Model.Name;
                                    leftPropertyModelName = "";
                                    rightPropertyModelName = " command.Data." + rightPropertyModelName;
                                }

                                if (nextCommand.Parent.IsMultiList() && nextCommand.IsList)
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

                                var currentModelPath = currentCommand.GetModelRootPathWithoutRoot();
                                var nextModelPath = nextCommand.GetModelRootPathWithoutRoot();


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

    }
}
