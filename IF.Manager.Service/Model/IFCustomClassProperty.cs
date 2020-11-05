using IF.Core.Data;
using IF.Manager.Contracts.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IF.Manager.Service.Model
{
   public  class IFCustomClassProperty :Entity
    {

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }


        public IFCustomClass IFCustomClass { get; set; }

        public int IFCustomClassId { get; set; }

        public string Type { get; set; }

        public bool IsNullable { get; set; }
    }
}
