using IF.Core.Data;
using System.ComponentModel.DataAnnotations;

namespace IF.Manager.Contracts.Model
{
    public class IFPublish:Entity
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int? SolutionId { get; set; }

        

        public int? ProjectId { get; set; }


        public int? ProcessId { get; set; }


        public IFSolution Solution { get; set; }



        public IFSolution Project { get; set; }


        public IFProcess Process { get; set; }



    }
}
