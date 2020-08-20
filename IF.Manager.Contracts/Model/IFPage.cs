using IF.Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IF.Manager.Contracts.Model
{
    public class IFPage: IFPageControl
    {
        public IFPage()
        {
            this.IFPageParameters = new List<IFPageParameter>();
        }

        public int? ProcessId { get; set; }

        public IFProject IFProject { get; set; }

        public int? IFProjectId { get; set; }

        public IFProcess Process { get; set; }

        public int PageLayoutId { get; set; }

        public IFPageLayout PageLayout { get; set; }

        public ICollection<IFPageParameter> IFPageParameters { get; set; }
    }
}
