using IF.Core.Data;
using IF.Manager.Contracts.Model;

using System.ComponentModel.DataAnnotations;

namespace IF.Manager.Service.Model
{
    public class IFClassMapping: Entity
    {
        [Key]
        public int Id { get; set; }


        public IFClassMapper IFClassMapper { get; set; }

        public int IFClassMapperId { get; set; }
        public int? FromPropertyId { get; set; }

        public int? ToPropertyId { get; set; }

        public IFClass FromProperty { get; set; }

        public IFModelProperty ToProperty { get; set; }



    }
}
