using IF.Core.Data;
using IF.Manager.Contracts.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IF.Manager.Contracts.Dto
{
    public class EntityPropertyDto
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }
        public bool IsIdentity { get; set; }
        public bool IsAutoNumber { get; set; }

        public bool IsNullable { get; set; }

        public bool IsAudited { get; set; }

        public bool IsMultiLanguage { get; set; }
        public int? MaxValue { get; set; }
        public int IFEntityId { get; set; }



        //public int? IFRelationEntityId { get; set; }
        //public EntityRelationType? EntityRelationType { get; set; }

        //public int Index { get; set; }


    }
}
