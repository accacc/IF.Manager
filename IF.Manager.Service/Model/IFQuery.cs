using IF.Core.Data;
using IF.Manager.Contracts.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IF.Manager.Contracts.Model
{
    public class IFQuery:Entity
    {
        public IFQuery()
        {
            this.QueryFilterItems = new List<IFQueryFilterItem>();
            this.QueryOrders = new List<IFQueryOrder>();
            this.Grids = new List<IFPageGrid>();
            this.Actions = new List<IFPageAction>();
            this.ListViews = new List<IFPageListView>();
        }

        public QueryGetType QueryGetType { get; set; }

        [Key]
        public int Id { get; set; }

        public int? PageSize { get; set; }

        public int? PageNumber { get; set; }

        //public ProcessType ProcessType { get; set; }

        public string Name { get; set; }

        public bool IsQueryOverride { get; set; }



        public string Description { get; set; }

        public int ModelId { get; set; }

        //[Column(TypeName = "nvarchar(MAX)")]
        //public string Filter { get; set; }
        public IFModel Model { get; set; }

        //public IFFormModel FormModel { get; set; }

        //public int FormModelId { get; set; }


        public IFProcess Process { get; set; }

        public int ProcessId { get; set; }
        public ICollection<IFQueryFilterItem> QueryFilterItems  { get; set; }

        public ICollection<IFPageGrid> Grids { get; set; }

        public ICollection<IFPageAction> Actions { get; set; }

        public ICollection<IFPageListView> ListViews { get; set; }

        public ICollection<IFQueryOrder> QueryOrders { get; set; }

    }
}
