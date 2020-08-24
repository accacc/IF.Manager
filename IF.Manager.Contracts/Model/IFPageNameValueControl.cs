using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Contracts.Model
{
    public class IFPageNameValueControl:IFPageControl
    {
        public IFQuery IFQuery { get; set; }

        public int IFQueryId { get; set; }

        public int NamePropertyId { get; set; }


        public int ValuePropertyId { get; set; }

        public override IFQuery GetQuery()
        {
            throw new NotImplementedException();
        }
    }
}
