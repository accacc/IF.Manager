using IF.Core.Data;
using IF.Manager.Service.Model;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

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

        public int? IFMapperId { get; set; }

        public IFCommand IFMapper { get; set; }

        public CommandType CommandGetType { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int ModelId { get; set; }

        public IFModel Model { get; set; }

        public IFProcess Process { get; set; }

        public int ProcessId { get; set; }
        public ICollection<IFCommandFilterItem> CommandFilterItems { get; set; }

        public ICollection<IFPageAction> Actions { get; set; }


    }
}
