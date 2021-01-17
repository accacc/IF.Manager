using System.Collections.Generic;

namespace IF.Manager.Contracts.Dto
{
    public class PageControlMapModel
    {

        public bool IsModal { get; set; }
        public int? RootControlMapId { get; set; }
        public List<PageControlTreeDto> Tree { get; set; }
    }

    public class CommandMapModel
    {

        public bool IsModal { get; set; }
        public int? CommandId { get; set; }
        public List<CommandControlTreeDto> Tree { get; set; }
    }


    public class ClassMapModel
    {

        public bool IsModal { get; set; }
        public int? ClassId { get; set; }
        public List<ClassControlTreeDto> Tree { get; set; }
    }


    public class QueryFilterMapModel
    {

        public int? QueryId { get; set; }
        public List<QueryFilterTreeDto> Tree { get; set; }
    }
}
