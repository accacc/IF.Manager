using IF.Core.Data;
using System.ComponentModel.DataAnnotations;

namespace IF.Manager.Contracts.Model
{
    public class IFModelProperty : Entity
    {

        [Key]
        public int Id { get; set; }


        public int EntityId { get; set; }

        public IFEntity Entity { get; set; }

        public int EntityPropertyId { get; set; }

        public int ModelId { get; set; }

        public IFEntityProperty EntityProperty { get; set; }

        public IFModel Model { get; set; }
    }
}
