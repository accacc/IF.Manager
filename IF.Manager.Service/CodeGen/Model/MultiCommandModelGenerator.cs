using System;
using System.Collections.Generic;
using System.Text;

using IF.CodeGeneration.Core;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Service.CodeGen.Interface;

using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace IF.Manager.Service.CodeGen.Model
{

    public class MultiCommandModelGenerator : IModelGenerator
    {

        FileSystemCodeFormatProvider fileSystem;
        IFModel model; string nameSpace;
        //ModelClassTreeDto entityTree;
        IFCommand command;

        public MultiCommandModelGenerator(FileSystemCodeFormatProvider fileSystem, IFModel model, string nameSpace, IFCommand command)
        {
            this.fileSystem = fileSystem;
            this.model = model;
            this.nameSpace = nameSpace;
           // this.entityTree = entityTree;
            this.command = command;
        }

        public void Generate()
        {
            List<MultiModelClass> alls = new List<MultiModelClass>();

            string name = DirectoryHelper.AddAsLastWord(model.Name, "DataModel");

            if(command.Childrens.Any() && command.Parent!=null)
            {
                name = name + "Multi";
            }

            MultiModelClass modelClass = new MultiModelClass(nameSpace, name, command);

            modelClass.Usings.Add("System");
            modelClass.Usings.Add("System.Collections.Generic");

            modelClass.Build();

            alls.Add(modelClass);

           // var relations = command.Childrens

            //if (relations.Any())
            //{
            //    GenerateRelatedModels(relations, alls, model, nameSpace);

            //}

            StringBuilder builder = new StringBuilder();

            foreach (var cls in alls)
            {
                builder.AppendLine(cls.GenerateCode().Template);

                this.fileSystem.FormatCode(builder.ToString(), "cs", name);
            }
        }
        private void GenerateRelatedModels(List<ModelClassTreeDto> relations, List<ModelClass> alls, IFModel model, string nameSpace)
        {
            foreach (var relation in relations)
            {
                bool IsModelProperty = ModelClassTreeDto.IsModelProperty(relation, model);

                if (!IsModelProperty) continue;

                string name = DirectoryHelper.AddAsLastWord(relation.Name, "DataModel");

                ModelClass modelClass = new ModelClass(nameSpace, name, model);

                if (!alls.Any(a => a.Name == modelClass.Name))
                {

                    alls.Add(modelClass);
                }

                modelClass.Build(relation.Childs.Where(c=>c.IsRelation).ToList());

                //this.fileSystem.FormatCode(modelClass.GenerateCode(), "cs");

                //if (relation.Childs.Any(c => c.IsRelation))
                //{
                //    GenerateRelatedModels(relation.Childs.Where(c => c.IsRelation).ToList(), alls, model, nameSpace);

                //}
            }


        }
    }
}
