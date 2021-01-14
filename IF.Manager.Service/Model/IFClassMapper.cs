using IF.Core.Data;
using IF.Manager.Contracts.Model;

using System.ComponentModel.DataAnnotations;

namespace IF.Manager.Service.Model
{
    public class IFClassMapper : Entity
    {
        public IFClassMapper()
        {
        }

        [Key]
        public int Id { get; set; }



        public string Name { get; set; }

        public string Description { get; set; }

        public int? IFClassId { get; set; }

        public int? IFModelId { get; set; }

        public IFClass IFClass { get; set; }

        public IFModel IFModel { get; set; }

    }
}
