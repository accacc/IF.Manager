using System;
using System.Collections.Generic;
using System.Text;

namespace {namespace}
{
    public class IFRole:Entity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } // Name

        //[Required]
        [MaxLength(500)]
        public string Description { get; set; } // Description

        //[Required]
        //[MaxLength(100)]
        //public string ResourceKey { get; set; } // ResourceKey

        //public bool IsDefault { get; set; } // IsDefault

        //public bool IsStatic { get; set; } // IsStatic



        //// Reverse navigation
        //[ForeignKey("RoleId")]
        //public virtual ICollection<RolePermission> RolePermissions { get; set; } // RolePermission.FK_KsRolePermission_KsRole

        //[ForeignKey("RoleId")]
        //public virtual ICollection<UserRole> UserRoles { get; set; } // UserRole.FK_KsUserRole_KsRole

        //// Foreign keys
        //public virtual User CreatorUser { get; set; } // FK_KsRole_KsUser

        //public virtual User DeleterUser { get; set; } // FK_KsRole_DeleterKsUser

        //public virtual User LastModifierUser { get; set; } // FK_KsRole_LastModifierKsUser

        //public Role()
        //{
        //    RolePermissions = new List<RolePermission>();
        //    UserRoles = new List<UserRole>();
        //}
    }
}
