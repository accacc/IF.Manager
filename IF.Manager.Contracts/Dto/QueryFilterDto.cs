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

    //public class QueryFilterGroupDto
    //{

    //    public QueryFilterGroupDto()
    //    {
    //        this.Items = new List<QueryFilterGroupItemDto>();
    //    }
    //    public int QueryFilterId { get; set; }        

    //    public QueryConditionOperator ConditionOperator { get; set; }

    //    public List<QueryFilterGroupItemDto> Items { get; set; }
    //}

    public class QueryFilterItemDto
    {

        

        public int Id { get; set; }

        public QueryFilterOperator FilterOperator { get; set; }

        public QueryConditionOperator ConditionOperator { get; set; }
        public string Value { get; set; }

        public int QueryId { get; set; }

        //public int EntityId { get; set; }

        public int EntityPropertyId { get; set; }

        //public string EntityName { get; set; }

        public string PropertyName { get; set; }

        public int? FormModelPropertyId { get; set; }

        public int? PageParameterId { get; set; }
    }
}
