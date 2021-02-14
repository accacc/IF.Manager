using IF.Core.Data;
using IF.Manager.Service.Model;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace IF.Manager.Contracts.Model
{
    public class IFCommand:Entity
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

        public int ModelId { get; set; }

        public IFModel Model { get; set; }

        public IFProcess Process { get; set; }

        public int ProcessId { get; set; }
        public ICollection<IFCommandFilterItem> CommandFilterItems { get; set; }

        public ICollection<IFPageAction> Actions { get; set; }

        //public bool IsMutli { get; set; }

        //public bool IsList { get; set; }


        public bool IsMultiCommand()
        {

            return this.Childrens.Any();
        }
        public bool IsList()
        {
            if (this.Parent == null) return false;

            var relation = this.Parent.Model.Entity.Relations.SingleOrDefault(r => r.Relation.Name == this.Model.Entity.Name);

            if (relation != null)
            {

                if (relation.Type == Contracts.Enum.EntityRelationType.ManyToMany ||
                        relation.Type == Contracts.Enum.EntityRelationType.OneToMany)
                {
                    return true;
                }
            }

            return false; ;
        }


        public string GetModelPath()
        {
            var pagePath = "";
            var parents = this.GetModelParentPath();

            foreach (var parent in parents)
            {
                    pagePath += parent.Name + ".";
            }
            if (parents.Any())
            {

                pagePath = pagePath.Remove(pagePath.Length - 1);
            }
            return pagePath;
        }
        public List<IFModel> GetModelParentPath()
        {

            List<IFModel> paths = new List<IFModel>();

            if (this.Parent == null)
            {
                return paths;
            }

            var page = this;

            while (page != null)
            {

                if (page.Parent == null) break;


                page = page.Parent;
                paths.Add(page.Model);



            }

            paths.Reverse();

            return paths;
        }


    }
}
