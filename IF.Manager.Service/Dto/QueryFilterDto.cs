using IF.Manager.Contracts.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Contracts.Dto
{
    public class QueryFilterDto
    {
        public QueryFilterDto()
        {
            this.Items = new List<QueryFilterItemDto>();
        }
        public int QueryId { get; set; }
        public List<QueryFilterItemDto> Items { get; set; }

        public QueryConditionOperator ConditionOperator { get; set; }
    }

   

    public class QueryFilterItemDto
    {


        public bool? IsNullCheck { get; set; }
        public int Id { get; set; }

        public QueryFilterOperator FilterOperator { get; set; }

        public QueryConditionOperator ConditionOperator { get; set; }
        public string Value { get; set; }

        public int QueryId { get; set; }


        public int EntityPropertyId { get; set; }


        public string PropertyName { get; set; }

        public int? FormModelPropertyId { get; set; }

        public int? PageParameterId { get; set; }
    }
}
