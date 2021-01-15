using IF.Core.Data;
using IF.Manager.Contracts.Model;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace IF.Manager.Service.Model
{
    public class IFCommandMulti : Entity, IMoveable
    {

        [Key]
        public int Id { get; set; }

        public IFCommandMulti()
        {

        }

        public int? ParentId { get; set; }

        public IFCommandMulti Parent { get; set; }

        public ICollection<IFCommandMulti> Childrens { get; set; }

        public int IFCommandId { get; set; }

        public IFCommand IFCommand { get; set; }

        public int? IFMapperId { get; set; }

        public IFCommand IFMapper { get; set; }

        public int Sequence { get; set; }



    }
}
