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

        public IFModel Model { get; set; }

        //public bool IsBeforeExecuteOverride { get; set; }
        //public bool IsAfterExecuteOverride { get; set; }

        public IFProcess Process { get; set; }

        public int ProcessId { get; set; }
        public ICollection<IFCommandFilterItem> CommandFilterItems { get; set; }

        public ICollection<IFPageAction> Actions { get; set; }

        //public bool IsMutli { get; set; }

        public bool? IsList { get; set; }

        public int? Sequence { get; set; }


        public bool IsMultiCommand()
        {

            return this.Childrens.Any();
        }
        //public bool IsListCommand()
        //{
        //    return this.IsList.Value;
        //    //if (this.Parent == null) return false;

        //    //var relation = this.Parent.Model.Entity.Relations.SingleOrDefault(r => r.Relation.Name == this.Model.Entity.Name);

        //    //if (relation != null)
        //    //{

        //    //    if (relation.Type == Contracts.Enum.EntityRelationType.ManyToMany ||
        //    //            relation.Type == Contracts.Enum.EntityRelationType.OneToMany)
        //    //    {
        //    //        return true;
        //    //    }
        //    //}

        //    //return false; ;
        //}


        public string GetModelPath()
        {
            var parents = this.GetParents();

            string pagePath = GeneratePaths(parents);

            return pagePath;
        }

        private string GeneratePaths(List<IFCommand> parents)
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

        public string GetModelPathUpTheRoot()
        {
            var parents = this.GetParents();

            parents = parents.Where(p => p.Parent != null).ToList();

            string pagePath = GeneratePaths(parents);

            return pagePath;
        }

        public bool IsMultiList
        {
            get { return this.IsMultiCommand() && this.IsList.Value; }

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
        public List<IFModel> GetModelParents()
        {

            List<IFModel> paths = new List<IFModel>();

            if (this.Parent == null)
            {
                return paths;
            }

            var command = this;

            while (command != null)
            {
                if (command.Parent == null) break;
                command = command.Parent;
                paths.Add(command.Model);
            }

            paths.Reverse();

            return paths;
        }


    }
}
