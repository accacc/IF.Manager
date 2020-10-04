using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Contracts.Dto
{
    public class PageControlMapModel
    {

        public bool IsModal { get; set; }
        public int? RootControlMapId { get; set; }
        public List<PageControlTreeDto> Tree { get; set; }
    }
}
