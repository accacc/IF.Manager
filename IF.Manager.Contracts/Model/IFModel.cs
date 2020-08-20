using IF.Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IF.Manager.Contracts.Model
{
    public class IFModel : Entity
    {

        public IFModel()
        {
            this.Properties = new List<IFModelProperty>();
            this.Queries = new List<IFQuery>();
            this.Commands = new List<IFCommand>();
            this.PageForms = new List<IFPageForm>();
        }

        [Key]
        public int Id { get; set; }
        
        public int EntityId { get; set; }

        public IFEntity Entity { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<IFModelProperty> Properties { get; set; }
        public ICollection<IFQuery> Queries { get; set; }
        public ICollection<IFCommand> Commands { get; set; }

        public ICollection<IFPageForm>  PageForms { get; set; }

    }
}
