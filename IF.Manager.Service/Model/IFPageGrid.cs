using IF.Core.Data;
using IF.Manager.Contracts.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IF.Manager.Contracts.Model
{
    public class IFPageGrid: IFPageControl
    {

        public IFPageGrid()
        {
            this.IFPageParameters = new List<IFPageParameter>();
            this.IFPageFormItemModelProperties = new List<IFPageControlItemModelProperty>();
        }


        public int? QueryId { get; set; }


        public IFPageGridLayout GridLayout { get; set; }


        public int? GridLayoutId { get; set; }


        public IFQuery Query { get; set; }

        public ICollection<IFPageParameter> IFPageParameters { get; set; }

        public ICollection<IFPageControlItemModelProperty> IFPageFormItemModelProperties { get; set; }

        public override IFQuery GetQuery()
        {
            return this.Query;
        }


    }
}
