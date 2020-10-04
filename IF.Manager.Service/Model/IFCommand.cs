using IF.Core.Data;
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

        public CommandType CommandGetType { get; set; }

        

        public string Name { get; set; }

        public string Description { get; set; }

        public int ModelId { get; set; }

        public IFModel Model { get; set; }

        public IFFormModel FormModel { get; set; }

        public int FormModelId { get; set; }

        public IFProcess Process { get; set; }

        public int ProcessId { get; set; }
        public ICollection<IFCommandFilterItem> CommandFilterItems { get; set; }

        public ICollection<IFPageAction> Actions { get; set; }


    }
}
