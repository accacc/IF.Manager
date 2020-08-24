using IF.Core.Control;
using IF.Core.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IF.Manager.Contracts.Model
{
    public abstract class IFPageControl : Entity
    {
        public IFPageControl()
        {
            
        }

        [Key]
        public int Id { get; set; }
        public PageControlType ControlType { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }


        public string CssClass { get; set; }

        public string ClientId { get; set; }

        public IFPageControlMap IFPageControlMap { get; set; }

        public int IFPageControlMapId { get; set; }

        public abstract IFQuery GetQuery();
        
            
        
    }
}
