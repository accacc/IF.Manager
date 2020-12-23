using IF.CodeGeneration.Core;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using System.Collections.Generic;
using System.Linq;

namespace IF.Manager.Service.Model
{
    public class ModelGenerator
    {

        FileSystemCodeFormatProvider fileSystem;
        public ModelGenerator(FileSystemCodeFormatProvider fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public  void GenerateModels(IFModel model, string nameSpace , ModelClassTreeDto entityTree)
        {
            

            string name = DirectoryHelper.AddAsLastWord(model.Name,"DataModel");

            ModelClass modelClass = new ModelClass(nameSpace,name, model);

            modelClass.Build(entityTree);

            this.fileSystem.FormatCode(modelClass.GenerateCode(), "cs");

            var relations = entityTree.Childs.Where(c => c.IsRelation).ToList();

            if (relations.Any())
            {
                GenerateRelatedModels(relations, model, nameSpace);

            }
        }        
        private void GenerateRelatedModels(List<ModelClassTreeDto> relations, IFModel model, string nameSpace)
        {
            foreach (var relation in relations)
            {
                bool IsModelProperty = ModelClassTreeDto.IsModelProperty(relation, model);

                if (!IsModelProperty) continue;

                string name = DirectoryHelper.AddAsLastWord(relation.Name, "DataModel");

                ModelClass modelClass = new ModelClass(nameSpace, name, model);

                modelClass.Build(relation);

                this.fileSystem.FormatCode(modelClass.GenerateCode(), "cs");

                if (relation.Childs.Any(c => c.IsRelation))
                {
                    GenerateRelatedModels(relation.Childs.Where(c => c.IsRelation).ToList(), model, nameSpace);

                }
            }


        }
    }
}
