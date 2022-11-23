using IF.Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IF.Manager.Contracts.Model
{
    public class IFLanguageEntity: Entity
    {

        [Key]
        public int Id { get; set; }

        public int EntityPropertyId { get; set; }
    }
}
