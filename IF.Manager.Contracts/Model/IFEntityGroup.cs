using IF.Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IF.Manager.Contracts.Model
{
    public class IFEntityGroup: Entity
    {
        public IFEntityGroup()
        {
            this.Entities = new List<IFEntity>();
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Prefix { get; set; }

        public ICollection<IFEntity> Entities { get; set; }
    }
}
