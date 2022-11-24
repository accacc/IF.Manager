using IF.CodeGeneration.Core;
using IF.CodeGeneration.Language.CSharp;
using IF.Core.Data;
using IF.Core.Localization;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Service.CodeGen;
using IF.Manager.Service.CodeGen.EF;
using IF.Manager.Service.CodeGen.Interface;

using Microsoft.Build.Evaluation;

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
            List<ModelClass> allModelClasses = new List<ModelClass>();

            string name = ObjectNamerHelper.AddAsLastWord(model.Name, "DataModel");

            ModelClass mainModelClass = new ModelClass(nameSpace, name, model);

            mainModelClass.Usings.Add("System");
            mainModelClass.Usings.Add("System.Collections.Generic");

            if(this.model.Entity.Properties.Any(p=>p.IsMultiLanguage) && this.model.Queries.Any())
            {
                foreach (var property in model.Entity.Properties.Where(p => p.IsMultiLanguage).ToList())
                {
                    mainModelClass.InheritedInterfaces.Add($"I{this.model.Entity.Name}{property.Name}Language");
                }

                   
            }

            mainModelClass.Build(entityTree.Childs);

            allModelClasses.Add(mainModelClass);

            var relations = entityTree.Childs.Where(c => c.IsRelation).ToList();

            if (relations.Any())
            {
                GenerateRelatedModels(relations, allModelClasses, model, nameSpace);

            }

            StringBuilder builder = new StringBuilder();

            foreach (var cls in allModelClasses)
            {
                builder.AppendLine(cls.GenerateCode().Template);

                this.fileSystem.FormatCode(builder.ToString(), "cs", name, "Models");
            }
        }     
        private void GenerateRelatedModels(List<ModelClassTreeDto> relations,List<ModelClass> alls, IFModel model, string nameSpace)
        {
            foreach (var relation in relations)
            {
                bool IsModelProperty = ModelClassTreeDto.IsModelProperty(relation.IsRelation,relation.Id, model.Properties);

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
