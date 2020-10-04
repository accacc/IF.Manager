using IF.Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IF.Manager.Contracts.Model
{
    public class IFSolution : Entity
    {

        public IFSolution()
        {
            this.Projects = new List<IFProject>();
        }

        [Key]
        public int Id { get; set; }

        public string SolutionName { get; set; }

        public string Description { get; set; }


        public string Path { get; set; }

        public ICollection<IFProject> Projects { get; set; }
    }
}
