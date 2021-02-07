using IF.Core.Data;
using IF.Manager.Contracts.Enum;
using IF.Manager.Contracts.Model;
using System.Collections.Generic;
using System.Linq;

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


        public string GetPath()
        {
            var pagePath = "";
            var parents = this.GetParentPath();

            foreach (var parent in parents)
            {
                pagePath += parent.Name + ".";
            }
            if (parents.Any())
            {

                pagePath = pagePath.Remove(pagePath.Length - 1);
            }
            return pagePath;
        }
        public List<ClassControlTreeDto> GetParentPath()
        {

            List<ClassControlTreeDto> paths = new List<ClassControlTreeDto>();

            if (this.Parent == null)
            {
                return paths;
            }

            var @class = this;

            while (@class != null)
            {

                if (@class.Parent == null) break;


                @class = @class.Parent;
                paths.Add(@class);



            }

            paths.Reverse();

            return paths;
        }
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
