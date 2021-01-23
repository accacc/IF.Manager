using IF.CodeGeneration.Core;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IF.Manager.Service.Model
{
    public class ModelGenerator
    {

        FileSystemCodeFormatProvider fileSystem;
        public ModelGenerator(FileSystemCodeFormatProvider fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public void GenerateModels(IFModel model, string nameSpace, ModelClassTreeDto entityTree)
        {
            List<ModelClass> alls = new List<ModelClass>();

            string name = DirectoryHelper.AddAsLastWord(model.Name, "DataModel");

            ModelClass modelClass = new ModelClass(nameSpace, name, model);

            modelClass.Usings.Add("System");
            modelClass.Usings.Add("System.Collections.Generic");

            modelClass.Build(entityTree);

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

                this.fileSystem.FormatCode(builder.ToString(), "cs", name);
            }
        }     
        private void GenerateRelatedModels(List<ModelClassTreeDto> relations,List<ModelClass> alls, IFModel model, string nameSpace)
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

                modelClass.Build(relation);

                //this.fileSystem.FormatCode(modelClass.GenerateCode(), "cs");

                if (relation.Childs.Any(c => c.IsRelation))
                {
                    GenerateRelatedModels(relation.Childs.Where(c => c.IsRelation).ToList(),alls ,model, nameSpace);

                }
            }


        }
    }
}
