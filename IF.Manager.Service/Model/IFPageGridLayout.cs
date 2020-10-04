using IF.Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IF.Manager.Contracts.Model
{
    public class IFPageGridLayout:Entity
    {
        public IFPageGridLayout()
        {
            this.PageGrids = new List<IFPageGrid>();
        }


        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }


        public ICollection<IFPageGrid> PageGrids { get; set; }

        public int LayoutId { get; set; }

        public IFPageLayout Layout { get; set; }
    }
}
