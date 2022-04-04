using IF.Core.Data;
using IF.Manager.Service.Enum;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IF.Manager.Contracts.Model
{
    public class IFEntity : Entity
    {
        public IFEntity()
        {
            this.Properties = new List<IFEntityProperty>();
            this.ReverseRelations = new List<IFEntityRelation>();
            this.ModelProperties = new List<IFModelProperty>();
            this.QueryFilterItems = new List<IFQueryFilterItem>();
            this.CommandFilterItems = new List<IFCommandFilterItem>();
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }



        public int? GroupId { get; set; }

        //public bool IsAudited { get; set; }

        public IFAuditType AuditType { get; set; }

        public IFEntityGroup Group { get; set; }

        public bool IsSoftDeleted { get; set; }
        public ICollection<IFEntityProperty> Properties { get; set; }

        public ICollection<IFEntityRelation> Relations { get; set; }

        public ICollection<IFEntityRelation> ReverseRelations { get; set; }

        public ICollection<IFModelProperty> ModelProperties { get; set; }
        public ICollection<IFQueryFilterItem> QueryFilterItems { get; set; }

        public ICollection<IFCommandFilterItem> CommandFilterItems { get; set; }

    }
}
