using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Contracts.Dto
{
    public class PublishDto
    {

        public string Name { get; set; }

        public string Description { get; set; }

        public int ProcessId { get; set; }

        public int SolutionId { get; set; }

        public int ProjectId { get; set; }
        

        public int PageId { get; set; }



    }
}
