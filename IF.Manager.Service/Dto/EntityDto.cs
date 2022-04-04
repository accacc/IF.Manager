using IF.Manager.Service.Enum;

using System.Collections.Generic;

namespace IF.Manager.Contracts.Dto
{
    public class EntityDto
    {

        public EntityDto()
        {
            this.Properties = new List<EntityPropertyDto>();
        }
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int? GroupId { get; set; }

        //public bool IsAudited { get; set; }

        public IFAuditType AuditType { get; set; }
        public bool IsSoftDeleted { get; set; }
        public string Prefix { get; set; }
        public string GroupName { get; set; }
        public List<EntityPropertyDto> Properties { get; set; }
        public List<EntityRelationDto> Relations { get; set; }

        public List<EntityRelationDto> ReverseRelations { get; set; }

    }


}
