using IF.Core.Data;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IF.Manager.Service.Model
{
    public class IFClass : Entity
    {
        public IFClass()
        {
            this.Childs = new List<IFClass>();
        }

        [Key]
        public int Id { get; set; }
        public int? ParentId { get; set; }

        public IFClass Parent { get; set; }

        public ICollection<IFClass> Childs { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

        public string GenericType { get; set; }

        public bool IsNullable { get; set; }
        public bool IsPrimitive { get; set; }

        [NotMapped]

        public bool Selected { get; set; }

        [NotMapped]

        public int? SortOrder { get; set; }

        [NotMapped]

        public int Level { get; set; }

        public List<IFClass> GetParentPages()
        {

            List<IFClass> paths = new List<IFClass>();

            if (this.Parent == null)
            {
                return paths;
            }

            var page = this;

            while (page != null)
            {

                if (page.Parent == null) break;

               
                    page = page.Parent;
                    paths.Add(page);
                


            }

            paths.Reverse();

            return paths;
        }

    }

 
}

