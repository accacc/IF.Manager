using IF.Core.Data;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace IF.Manager.Service.Model
{
    public class IFClass : Entity
    {
        public IFClass()
        {
            this.Childrens = new List<IFClass>();
        }

        [Key]
        public int Id { get; set; }
        public int? ParentId { get; set; }

        public IFClass Parent { get; set; }

        public ICollection<IFClass> Childrens { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

        public string GenericType { get; set; }


        /// <summary>
        /// 
        /// </summary>

        public bool IsNullable { get; set; }
        public bool IsPrimitive { get; set; }

        [NotMapped]

        public int Level { get; set; }

        public List<IFClass> GetParents()
        {

            List<IFClass> paths = new List<IFClass>();

            if (this.Parent == null)
            {
                return paths;
            }

            var @class = this;

            while (@class != null)
            {
                if (@class.Parent == null) break;

                @class = @class.Parent;
                paths.Add(@class);
            }

            paths.Reverse();

            return paths;
        }

        public string GetRootPath()
        {
            var parents = this.GetParents();

            string pagePath = GenerateRootPathAsString(parents);

            return pagePath;
        }

        public string GetRootPathWithoutRoot()
        {
            var parents = this.GetParents();

            parents = parents.Where(p => p.Parent != null).ToList();

            string pagePath = GenerateRootPathAsString(parents);

            return pagePath;
        }

        private string GenerateRootPathAsString(List<IFClass> parents)
        {
            var pagePath = "";

            foreach (var parent in parents)
            {
                string name = parent.Name;

                pagePath += name + ".";
            }


            if (parents.Any())
            {
                pagePath = pagePath.Remove(pagePath.Length - 1);
            }

            return pagePath;
        }

    }


}

