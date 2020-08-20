using IF.Core.Data;
using IF.Manager.Contracts.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IF.Manager.Contracts.Model
{
    public class IFEntityProperty : Entity
    {
        public IFEntityProperty()
        {
            this.ModelProperties = new List<IFModelProperty>();
            this.QueryFilterItems = new List<IFQueryFilterItem>();
            this.CommandFilterItems = new List<IFCommandFilterItem>();
            this.QueryOrders = new List<IFQueryOrder>();
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }     
        public bool IsIdentity { get; set; }

        public int? MaxValue { get; set; }

        public int EntityId { get; set; }

        public bool IsAudited { get; set; }

        public bool IsMultiLanguage { get; set; }

        public bool IsNullable { get; set; }

        public IFEntity Entity { get; set; }

        public ICollection<IFModelProperty> ModelProperties { get; set; }

        public ICollection<IFQueryFilterItem> QueryFilterItems { get; set; }
        public ICollection<IFCommandFilterItem> CommandFilterItems { get; set; }

        public ICollection<IFQueryOrder> QueryOrders { get; set; }




    }
}
