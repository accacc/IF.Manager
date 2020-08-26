using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Contracts.Model
{
    public class IFPageListView: IFPageControl
    {
        public IFPageListView()
        {
            this.IFPageParameters = new List<IFPageParameter>();
        }

        public IFPageFormLayout FormLayout { get; set; }

        public int FormLayoutId { get; set; }

        public IFQuery IFQuery { get; set; }

        public int? IFQueryId { get; set; }

        public ICollection<IFPageParameter> IFPageParameters { get; set; }

        public override IFQuery GetQuery()
        {
            throw new NotImplementedException();
        }

    }
}
