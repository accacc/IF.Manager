using IF.Core.Data;
using IF.Manager.Contracts.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IF.Manager.Contracts.Model
{
    public class IFEntityRelation : Entity
    {
        [Key]
        public int Id { get; set; }

        public string Prefix { get; set; }

        public int? ForeignKeyIFEntityPropertyId  { get; set; }

        public IFEntityProperty ForeignKeyIFEntityProperty  { get; set; }

        public bool IsDbFirst { get; set; }

        public int EntityId { get; set; }

        public int RelationId { get; set; }

        //public EntityRelationDirectionType From { get; set; }

        //public EntityRelationDirectionType To { get; set; }

        public EntityRelationType Type { get; set; }

        public IFEntity Entity { get; set; }

        public IFEntity Relation { get; set; }
    }
}