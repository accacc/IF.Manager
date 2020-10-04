using IF.CodeGeneration.CSharp;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;

namespace IF.Manager.Service
{
    public class ModelClass: CSClass
    {

        
        IFModel model;
        
        public ModelClass(string nameSpace,string name,IFModel model)
        {            
            this.model = model;
            this.Name = $"{name}";
            this.NameSpace = nameSpace;            
        }


        public void Build(EntityTreeDto entityTree)
        {
            this.Usings.Add("System");

            foreach (var childEntityTree in entityTree.Childs)
            {               
                bool IsModelProperty = EntityTreeDto.IsModelProperty(childEntityTree,model);

                if (IsModelProperty)
                {
                    string name = childEntityTree.Name;

                    if (childEntityTree.IsRelation)
                    {
                        name = $"{childEntityTree.Name}";
                    }

                    var p = new CSProperty("public", name, childEntityTree.IsNullable);
                    p.PropertyTypeString = childEntityTree.Type;
                    this.Properties.Add(p);
                }

            }

        }

        
    }
}
