using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace {namespace}
{
    public class IFPermission 
    {
        [Key]
        public int Id { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        //public int? ParentId { get; set; } // ParentId

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } // Name


        [Required]
        [MaxLength(250)]
        public string Text { get; set; } // Name


        [Required]
        [MaxLength(100)]
        public string Description { get; set; } // Description

        [Required]
        [MaxLength(100)]
        public string ResourceKey { get; set; } // ResourceKey

        public string IconName { get; set; }

        public string RouteParameter { get; set; } // ResourceKey
        public bool? IsPermissionEnabled { get; set; }

        public bool IsAdmin { get; set; }

        [MaxLength(6)]
        public string Method { get; set; }

        [ForeignKey("PermissionId")]
        public virtual ICollection<RolePermission> RolePermissions { get; set; } // RolePermission.FK_KsRolePermission_KsPermission

        //public ICollection<Role> Roles { get; set; }

        // Foreign keys

        public Permission()
        {
            //Menus = new List<Menu>();
            //Children = new List<Permission>();
            RolePermissions = new List<RolePermission>();
            //Roles = new HashSet<Role>();
        }
    }
}
