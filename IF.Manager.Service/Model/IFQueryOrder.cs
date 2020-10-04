using IF.Core.Data;
using IF.Manager.Contracts.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IF.Manager.Contracts.Model
{
    public class IFQueryOrder:Entity
    {
        [Key]
        public int Id { get; set; }


        public IFEntityProperty EntityProperty { get; set; }

        public IFQuery Query { get; set; }

        public int QueryId { get; set; }

        //public int EntityId { get; set; }

        public int EntityPropertyId { get; set; }

        public QueryOrderOperator QueryOrderOperator { get; set; }
    }
}
