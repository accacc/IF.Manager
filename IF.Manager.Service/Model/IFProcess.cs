using IF.Core.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IF.Manager.Contracts.Model
{
    public class IFProcess : Entity
    {

        public IFProcess()
        {
            this.Queries = new List<IFQuery>();
            this.Commands = new List<IFCommand>();
            this.Pages = new List<IFPage>();
        }


        public ICollection<IFQuery> Queries { get; set; }
        public ICollection<IFCommand> Commands { get; set; }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public IFProject Project { get; set; }

        public ICollection<IFPage> Pages { get; set; }

        public int ProjectId { get; set; }
    }
}

