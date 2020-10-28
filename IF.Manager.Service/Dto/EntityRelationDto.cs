using IF.Manager.Contracts.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Contracts.Dto
{
    public class EntityRelationDto
    {
        public EntityRelationDto()
        {
            //this.Index = 0;
            this.EntityRelationType = EntityRelationType.None;
        }
        public int Id { get; set; }

        public int IFRelatedEntityId { get; set; }

        public int IFEntityId { get; set; }

        public string Prefix { get; set; }

        public Guid Index { get; set; }
        public int? ForeignKeyPropertyId { get; set; }

        public string ForeignKeyPropertyName { get; set; }

        public bool IsDbFirst { get; set; }

        //public EntityDto IFRelatedEntity { get; set; }

        //public EntityDto IFEntity { get; set; }
        public EntityRelationType EntityRelationType { get; set; }

        public string RelatedEntityName { get; set; }
        public string EntityName { get; set; }

        //public EntityRelationDirectionType From { get; set; }

        //public EntityRelationDirectionType To { get; set; }
        //public string ReverseRelatedEntityName { get; set; }
        //public int Index { get; set; }


    }
}
