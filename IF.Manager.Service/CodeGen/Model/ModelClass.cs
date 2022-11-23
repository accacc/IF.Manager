using IF.CodeGeneration.Language.CSharp;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using System.Linq;

using System.Collections.Generic;
using IF.Manager.Service.CodeGen;

namespace IF.Manager.Service
{
    public class ModelClass : CSClass
    {


        IFModel model;

        public ModelClass(string nameSpace, string name, IFModel model)
        {
            this.model = model;
            this.Name = $"{name}";
            this.NameSpace = nameSpace;
        }


        public void Build(IList<ModelClassTreeDto> entityTree)
        {


            foreach (var childEntityTree in entityTree)
            {
                bool IsModelProperty = ModelClassTreeDto.IsModelProperty(childEntityTree.IsRelation, childEntityTree.Id, model.Properties);

                if (IsModelProperty)
                {
                    string name = childEntityTree.Name;
                    string type = childEntityTree.Type;

                    if (childEntityTree.IsRelation)
                    {
                        name = ObjectNamerHelper.AddAsLastWord(childEntityTree.Name, "DataModel");
                        name = $"{name}";
                        type = name;

                        if (childEntityTree.IsList)
                        {
                            type = $"IEnumerable<{name}>";
                        }
                    }

                    bool IsNullable = childEntityTree.IsNullable;
                    if (childEntityTree.Type == "string")
                    {
                        IsNullable = false;
                    }

                    var p = new CSProperty("public", name, IsNullable);
                    p.PropertyTypeString = type;
                    this.Properties.Add(p);
                }

            }

        }


    }

  
}
