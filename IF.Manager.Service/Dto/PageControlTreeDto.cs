using IF.Core.Data;
using IF.Manager.Contracts.Enum;
using IF.Manager.Contracts.Model;
using System.Collections.Generic;

namespace IF.Manager.Contracts.Dto
{
    public class PageControlTreeDto : TreeDto<PageControlTreeDto>
    {
        public string Name { get; set; }
        public int PageControlId { get; set; }
        public IFPageControl    PageControl { get; set; }
    }

    public class CommandControlTreeDto : TreeDto<CommandControlTreeDto>
    {
        public string Name { get; set; }

        public string Description { get; set; }

    }

    public class ClassControlTreeDto : TreeDto<ClassControlTreeDto>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

        public string GenericType { get; set; }



        public bool IsPrimitive { get; set; }

        public bool IsNullable { get; set; }
    }

    public class QueryFilterTreeDto : TreeDto<QueryFilterTreeDto>
    {
        public QueryFilterTreeDto()
        {
            this.Childs = new List<QueryFilterTreeDto>();
        }
        //public string Name { get; set; }

      //  public string Description { get; set; }

        public QueryConditionOperator ConditionOperator { get; set; }
        public QueryFilterOperator FilterOperator { get; set; }

        public string Value { get; set; }

        public int QueryId { get; set; }

        public int EntityPropertyId { get; set; }

        public int? FormModelPropertyId { get; set; }

        public int? IFPageParameterId { get; set; }

        public bool? IsNullCheck { get; set; }

        public string PropertyName { get; set; }
    }
}
