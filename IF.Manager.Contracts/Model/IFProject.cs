using IF.Core.Data;
using IF.Manager.Contracts.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IF.Manager.Contracts.Model
{
    public class IFProject : Entity
    {
        public IFProject()
        {
            this.Processes = new List<IFProcess>();
            this.IFPages = new List<IFPage>();
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public ProjectType  ProjectType { get; set; }

        public string ConnectionString { get; set; }

        public string Description { get; set; }
        
        public int SolutionId { get; set; }

        public IFSolution Solution { get; set; }

        public ICollection<IFProcess> Processes { get; set; }
        public ICollection<IFPage> IFPages { get; set; }


    }
}