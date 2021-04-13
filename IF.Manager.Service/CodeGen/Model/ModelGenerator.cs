using IF.CodeGeneration.Core;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Service.CodeGen;
using IF.Manager.Service.CodeGen.Interface;

using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IF.Manager.Service.Model
{

 

    public class ModelGenerator:IModelGenerator
    {

        FileSystemCodeFormatProvider fileSystem;
        IFModel model;
        string nameSpace;
        ModelClassTreeDto entityTree;
        
        public ModelGenerator(FileSystemCodeFormatProvider fileSystem, IFModel model, string nameSpace, ModelClassTreeDto entityTree)
        {
            this.fileSystem = fileSystem;
            this.model = model;
            this.nameSpace = nameSpace;
            this.entityTree = entityTree;
        }

        public void Generate(string path)
        {
            List<ModelClass> alls = new List<ModelClass>();

            string name = ObjectNamerHelper.AddAsLastWord(model.Name, "DataModel");

            ModelClass modelClass = new ModelClass(nameSpace, name, model);

            modelClass.Usings.Add("System");
            modelClass.Usings.Add("System.Collections.Generic");

            modelClass.Build(entityTree.Childs);

            alls.Add(modelClass);

            var relations = entityTree.Childs.Where(c => c.IsRelation).ToList();

            if (relations.Any())
            {
                GenerateRelatedModels(relations, alls, model, nameSpace);

            }

            StringBuilder builder = new StringBuilder();

            foreach (var cls in alls)
            {
                builder.AppendLine(cls.GenerateCode().Template);

                this.fileSystem.FormatCode(builder.ToString(), "cs", name, path);
            }
        }     
        private void GenerateRelatedModels(List<ModelClassTreeDto> relations,List<ModelClass> alls, IFModel model, string nameSpace)
        {
            foreach (var relation in relations)
            {
                bool IsModelProperty = ModelClassTreeDto.IsModelProperty(relation, model);

                if (!IsModelProperty) continue;

                string name = ObjectNamerHelper.AddAsLastWord(relation.Name, "DataModel");

                ModelClass modelClass = new ModelClass(nameSpace, name, model);

                if (!alls.Any(a => a.Name == modelClass.Name))
                {

                    alls.Add(modelClass);
                }

                modelClass.Build(relation.Childs);

                if (relation.Childs.Any(c => c.IsRelation))
                {
                    GenerateRelatedModels(relation.Childs.Where(c => c.IsRelation).ToList(),alls ,model, nameSpace);
                }
            }
        }
    }
}
