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
}
