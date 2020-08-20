using IF.Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IF.Manager.Contracts.Model
{
    public class IFPageFormLayout: Entity
    {
        public IFPageFormLayout()
        {
            this.PageForms = new List<IFPageForm>();
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<IFPageForm> PageForms { get; set; }
    }
}
