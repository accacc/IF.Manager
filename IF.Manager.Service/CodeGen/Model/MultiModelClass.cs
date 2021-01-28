using IF.CodeGeneration.CSharp;
using IF.Manager.Contracts.Model;

using System.Linq;

namespace IF.Manager.Service.CodeGen.Model
{
    public class MultiModelClass : CSClass
    {


        IFCommand command;

        public MultiModelClass(string nameSpace, string name, IFCommand model)
        {
            this.command = model;
            this.Name = $"{name}";
            this.NameSpace = nameSpace;
        }


        public void Build()
        {


            foreach (var childCommand in command.Childrens)
            {
                string name = childCommand.Model.Name;
                string type = childCommand.Model.Name;

                var relation = command.Model.Entity.Relations.SingleOrDefault(r => r.Relation.Name == childCommand.Model.Entity.Name);

                if (relation != null)
                {

                    if (relation.Type == Contracts.Enum.EntityRelationType.ManyToMany ||
                            relation.Type == Contracts.Enum.EntityRelationType.OneToMany)
                    {
                        type = $"IEnumerable<{name}>";
                    }
                }

                var p = new CSProperty("public", name, false);
                p.PropertyTypeString = type;
                this.Properties.Add(p);

            }

        }


    }
}
