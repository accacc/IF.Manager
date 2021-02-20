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

                if (childCommand.IsMultiCommand())
                {
                    name = name + "Multi";
                }

                bool isList = childCommand.IsList();

                string type = name; ;

                if (isList)
                {
                    type = $"List<{name}>";
                }

                var p = new CSProperty("public", name, false);
                p.PropertyTypeString = type;
                this.Properties.Add(p);

            }

        }


    }
}
