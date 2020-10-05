using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Contracts.Model
{
    public class IFPageForm : IFPageControl
    {
        public IFPageForm()
        {
            this.IFPageFormItemModelProperties = new List<IFPageControlItemModelProperty>();
        }

        public IFPageFormLayout FormLayout { get; set; }

        public int FormLayoutId { get; set; }

        public IFModel IFModel { get; set; }

        public int? IFModelId { get; set; }

        public IFQuery IFQuery { get; set; }

        public int? IFQueryId { get; set; }

        public ICollection<IFPageControlItemModelProperty> IFPageFormItemModelProperties { get; set; }

        public override IFQuery GetQuery()
        {
            throw new NotImplementedException();
        }
    }
}
