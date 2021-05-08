using IF.Core.Data;
using IF.Manager.Service.Model;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace IF.Manager.Contracts.Model
{
    public class IFCommand : Entity
    {

        [Key]
        public int Id { get; set; }

        public IFCommand()
        {
            this.CommandFilterItems = new List<IFCommandFilterItem>();
            this.Actions = new List<IFPageAction>();
        }

        public int? ParentId { get; set; }

        public IFCommand Parent { get; set; }

        public ICollection<IFCommand> Childrens { get; set; }

        public int? IFClassMapperId { get; set; }

        public IFClassMapper IFClassMapper { get; set; }

        public CommandType CommandGetType { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int? ModelId { get; set; }

        public bool IsQueryOverride { get; set; }
        public IFModel Model { get; set; }
        public IFProcess Process { get; set; }

        public int ProcessId { get; set; }
        public ICollection<IFCommandFilterItem> CommandFilterItems { get; set; }

        public ICollection<IFPageAction> Actions { get; set; }

        public bool IsList { get; set; }

        public int Sequence { get; set; }

        public bool IsMultiList()
        {
            return this.IsMultiCommand() && this.IsList;

        }

        public bool IsMultiCommand()
        {
            return this.Childrens.Any();
        }


        public string GetModelRootPath()
        {
            var parents = this.GetParents();

            string pagePath = GenerateModelRootPathAsString(parents);

            return pagePath;
        }

        private string GenerateModelRootPathAsString(List<IFCommand> parents)
        {
            var pagePath = "";

            foreach (var parent in parents)
            {
                string name = parent.Model.Name;

                if (parent.IsMultiCommand())
                {
                    name += "Multi";
                }

                pagePath += name + ".";
            }


            if (parents.Any())
            {
                pagePath = pagePath.Remove(pagePath.Length - 1);
            }

            return pagePath;
        }

        public string GetModelRootPathWithoutRoot()
        {
            var parents = this.GetParents();

            parents = parents.Where(p => p.Parent != null).ToList();

            string pagePath = GenerateModelRootPathAsString(parents);

            return pagePath;
        }

       

        public List<IFCommand> GetParents()
        {

            List<IFCommand> paths = new List<IFCommand>();

            if (this.Parent == null)
            {
                return paths;
            }

            var command = this;

            while (command != null)
            {
                if (command.Parent == null) break;
                command = command.Parent;
                paths.Add(command);
            }

            paths.Reverse();

            return paths;
        }
    }
}
