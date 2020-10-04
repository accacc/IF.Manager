using IF.Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IF.Manager.Contracts.Model
{
    public class IFPageLayout:Entity
    {
        public IFPageLayout()
        {
            this.GridLayouts = new List<IFPageGridLayout>();
        }


        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int ColumSize { get; set; }

        public ICollection<IFPageGridLayout> GridLayouts { get; set; }
    }
}
