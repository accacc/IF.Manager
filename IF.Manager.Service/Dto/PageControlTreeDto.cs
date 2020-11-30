using IF.Core.Data;
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

    public class ClassControlTreeDto : TreeDto<ClassControlTreeDto>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }


        public bool IsPrimitive { get; set; }

        public bool IsNullable { get; set; }
    }
}
