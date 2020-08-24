using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Contracts.Model
{
    public class IFPageNavigation: IFPageControl
    {
        public int IFProjectId { get; set; }

        public IFProject IFProject { get; set; }

        public override IFModel GetQuery()
        {
            throw new NotImplementedException();
        }
    }
}
