using IF.Manager.Contracts.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Contracts.Dto
{
    public class QueryDto
    {
        public int Id { get; set; }

        //public ProcessType ProcessType { get; set; }

        public QueryGetType QueryGetType { get; set; }

        public int? PageSize { get; set; }

        public int? PageNumber { get; set; }

        public string Filter { get; set; }
        public int ModelId { get; set; }

        public int ProcessId { get; set; }

        public int FormModelId { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsQueryOverride { get; set; }


    }
}
