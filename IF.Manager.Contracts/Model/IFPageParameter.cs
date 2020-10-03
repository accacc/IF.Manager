using IF.Core.Data;
using IF.Manager.Contracts.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IF.Manager.Contracts.Model
{
    public class IFPageParameter:Entity
    {
        [Key]
        public int Id { get; set; }

        public int? ObjectId { get; set; }

        public IFPage IFPage { get; set; }

        public IFPageGrid IFPageGrid { get; set; }

        public IFPageListView IFPageListView { get; set; }

        public string Type { get; set; }

        public PageParameterType ObjectType { get; set; }

        public string Name { get; set; }
    }
}
