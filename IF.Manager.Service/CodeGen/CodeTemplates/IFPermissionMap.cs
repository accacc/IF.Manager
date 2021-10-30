using System;
using System.Collections.Generic;
using System.Text;

namespace {namespace}
{
    public class IFPermissionMap : Entity
    {
        [Key]
        public int Id { get; set; }
        public int? ParentId { get; set; }

        public int PermissionId { get; set; }

        public int SortOrder { get; set; }

        public int Type { get; set; }

        public int WidgetType { get; set; }

        public byte WidgetRenderType { get; set; }

        public bool IsActive { get; set; }



        public virtual Permission Permission { get; set; }



    }
}